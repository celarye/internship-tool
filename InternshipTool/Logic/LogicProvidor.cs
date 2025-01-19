using System;
using System.Security.Cryptography;

using Globals;
using Globals.Enums;
using LogicInterface;
using Data;
using DataInterface;

namespace Logic;

public class LogicProvidor : ILogicProvidor
{
  private readonly IDataProvidor _dataProvidor;
  private User? _loggedInUser { get; set; }
  private Company? _loggedInCompany { get; set; }

  public LogicProvidor()
  {
    _dataProvidor = new Data.DataProvidor();
    _loggedInUser = null;
    _loggedInCompany = null;
  }

  public User? GetUser(int id)
  {
    return _dataProvidor.GetUser(id);
  }

  public User? GetUserByEmail(string email)
  {
    return _dataProvidor.GetUserByEmail(email);
  }

  public User? GetLoggedInUser()
  {
    return _loggedInUser;
  }

  public void LoginUser(string email, string password)
  {
    User? currentUser = _dataProvidor.GetUserByEmail(email);

    if (currentUser == null)
    {
      throw new UnauthorizedAccessException("No user with this email exists");
    }

    if (VerifyPasswords(currentUser.Password, password))
    {
      _loggedInUser = currentUser;
    }
    else
    {
      throw new UnauthorizedAccessException("Invalid email or password");
    }
  }

  public void RegisterUser(string email, string password, string firstName, string secondName, UserTypes userType)
  { 
    if (_dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("Email is already in use");
    }

    User newUser;

    switch (userType)
    {
      case UserTypes.Admin:
        newUser = new Admin(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType);
        break;
      case UserTypes.Student:
        newUser = new Student(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType, null);
        break;
      case UserTypes.Teacher:
        newUser = new Teacher(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType);
        break;
      default:
        newUser = new CompanyEmployee(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType, _loggedInCompany!.Id);
        break;
    }

    _dataProvidor.AddUser(newUser);
  }

  public void UpdateUser(User user, string? email, string? firstName, string? secondName, string? password, UserTypes? userType, string? cvPath)
  {
    if (email != null && _dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("User with the same email already exists.");
    }

    if (email != null)
    {
      user.Email = email;
    }

    if (firstName != null)
    {
      user.FirstName = firstName;
    }

    if (secondName != null)
    {
      user.SecondName = secondName;
    }

    if (password != null)
    {
      user.Password = HashPassword(null, password);
    }

    if (userType != null)
    {
      user.UserType = (UserTypes)userType;
    }

    switch (user.UserType)
    {
      case UserTypes.Admin:
        break;
      case UserTypes.Student:
        if (cvPath == null)
          return;

        Student student = (Student)user;

        string cvDirectory = "cv";

        if (!Directory.Exists(cvDirectory))
        {
          Directory.CreateDirectory(cvDirectory);
        }

        string destinationPath = Path.Combine(cvDirectory, $"{student.Id}.pdf");

        try
        {
          File.Copy(cvPath, Path.Combine(destinationPath), overwrite: true);

          student.CVPath = destinationPath;
          user = student;
        }
        catch (IOException ex)
        {
          System.Diagnostics.Debug.WriteLine($"Error copying file: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
          System.Diagnostics.Debug.WriteLine($"Permission error: {ex.Message}");
        }
        break;
      case UserTypes.Teacher:
        break;
      case UserTypes.CompanyEmployee:
        if (_loggedInCompany == null)
          return;

        CompanyEmployee companyEmployee = ((CompanyEmployee)user);

        companyEmployee.CompanyId = _loggedInCompany.Id;

        user = companyEmployee;
        break;
    }

    _dataProvidor.UpdateUser(user);
  }

  public byte[] GenerateSalt()
  {
    byte[] salt = new byte[16];
    using (var rng = new RNGCryptoServiceProvider())
    {
      rng.GetBytes(salt);
    }
    return salt;
  }

  public string HashPassword(byte[]? salt, string password)
  {
    if (salt == null)
    {
      salt = GenerateSalt();
    }

    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
    byte[] hash = pbkdf2.GetBytes(20);

    byte[] hashBytes = new byte[36];
    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 20);

    return Convert.ToBase64String(hashBytes);
  }

  public bool VerifyPasswords(string savedPasswordHash, string inputPassword)
  {
    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

    byte[] salt = new byte[16];
    Array.Copy(hashBytes, 0, salt, 0, 16);

    return HashPassword(salt, inputPassword) == savedPasswordHash;
  }

  // Company methods
  public Company? GetCompany(int id)
  {
    return _dataProvidor.GetCompany(id);
  }

  public Company? GetLoggedInCompany()
  {
    return _loggedInCompany;
  }

  public void LoginCompany()
  {
    _loggedInCompany = _dataProvidor.GetCompany((int)((CompanyEmployee)_loggedInUser!).CompanyId!);
  }

  public void RegisterCompany(string email, string password, string firstName, string secondName, string companyName)  
  {
    if (_dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("Email is already in use");
    }

    int userId = _dataProvidor.GetNextUserId();
    int companyId = _dataProvidor.GetNextCompanyId();

    User newCompanyEmployee = new CompanyEmployee(userId, email, firstName, secondName, HashPassword(null, password), UserTypes.CompanyEmployee, companyId);

    _dataProvidor.AddUser(newCompanyEmployee);

    var newCompany = new Company(companyId, companyName, userId);

    _loggedInCompany = newCompany;

    _dataProvidor.AddCompany(newCompany);
  }

  // Internshup methods
  public List<Internship> GetAdminInternships(bool approved)
  {
    return _dataProvidor.GetInternships(approved);
  }

  public List<Internship> GetStudentAvailableInternships()
  {
    if (_loggedInUser == null)
      return new List<Internship>();

    return _dataProvidor.GetInternshipsNotByStudent();
  }

  public List<Internship> GetStudentInternship()
  {
    if (_loggedInUser == null)
      return new List<Internship>();

    return _dataProvidor.GetInternshipsByStudent(_loggedInUser!.Id);
  }

  public List<Internship> GetTeacherInternships()
  {
    if (_loggedInUser == null)
      return new List<Internship>();

    return _dataProvidor.GetInternshipsByTeacher(_loggedInUser!.Id);
  }

  public List<Internship> GetCompanyInternships(bool approved)
  {
    if (_loggedInCompany == null)
      return new List<Internship>();

    return _dataProvidor.GetInternshipsByCompany(approved, _loggedInCompany!.Id);
  }

  public List<Internship> GetMentorInternships()
  {
    if (_loggedInUser == null)
      return new List<Internship>();

    return _dataProvidor.GetInternshipsByMentor(_loggedInUser!.Id);
  }

  public void AddInternship(string title, string description)
  {
    int internshipId = _dataProvidor.GetNextInternshipId();

    Internship internship = new Internship(internshipId, _loggedInCompany!.Id, title, description);
    _dataProvidor.AddInternship(internship);

    InternshipEvaluation internshipEvaluation = new InternshipEvaluation(_dataProvidor.GetNextInternshipEvaluationId(), internshipId);
    _dataProvidor.AddInternshipEvaluation(internshipEvaluation);
  }

  public void ApproveInternship(Internship internship)
  {
    internship.Approved = true;

    _dataProvidor.UpdateInternship(internship);
  }

  public void AssignInternship(Internship internship, string mentorEmail, string studentEmail, string teacherEmail)
  {
    User? mentor = _dataProvidor.GetUserByEmail(mentorEmail) ?? throw new ArgumentException("Invalid mentor email");
    int mentorCompanyId = ((CompanyEmployee)mentor).CompanyId ?? throw new ArgumentException("Invalid mentor email");
    if (mentorCompanyId != internship.CompanyId)
      throw new ArgumentException("Invalid mentor email");

    User? student = _dataProvidor.GetUserByEmail(studentEmail) ?? throw new ArgumentException("Invalid student email");
    if (student.UserType != UserTypes.Student)
      throw new ArgumentException("Invalid student email");
    if (_dataProvidor.GetInternshipCandidatesByStudent(student.Id) ==  null)
      throw new ArgumentException("Invalid student email");

    User? teacher = _dataProvidor.GetUserByEmail(teacherEmail) ?? throw new ArgumentException("Invalid teacher email");
    if (teacher.UserType != UserTypes.Teacher)
      throw new ArgumentException("Invalid teacher email");

    internship.MentorId = mentor.Id;
    internship.StudentId = student.Id;
    internship.TeacherId = teacher.Id;

    _dataProvidor.UpdateInternship(internship);
    _dataProvidor.RemoveInternshipCandidates(internship.Id);
    _dataProvidor.RemoveInternshipCandidacies(student.Id);
  }

  // InternshipCandidate methods
  public List<InternshipCandidate> GetInternshipCandidates(int internshipId)
  {
    return _dataProvidor.GetInternshipCandidatesByInternship(internshipId);
  }

  public void AddInternshipCandidate(int internshipId)
  {
    if (_loggedInUser == null)
      throw new UnauthorizedAccessException("You are not logged in");

    if (((Student)_loggedInUser).CVPath == null)
      throw new UnauthorizedAccessException("You need to have a CV to be able to put yourself as a candidate for an internship");

    InternshipCandidate internshipCandidate = new InternshipCandidate(internshipId, _loggedInUser.Id);

    _dataProvidor.AddInternshipCandidate(internshipCandidate);
  }

  public InternshipEvaluation? GetInternshipEvaluation(int internshipId)
  {
    return _dataProvidor.GetInternshipEvaluationByInternship(internshipId);
  }

  public void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation, InternshipEvaluations? mentorEvaluation1, InternshipEvaluations? mentorEvaluation2, InternshipEvaluations? teacherEvaluation, string note)
  {
    internshipEvaluation.MentorEvaluation1 = mentorEvaluation1;
    internshipEvaluation.MentorEvaluation2 = mentorEvaluation2;
    internshipEvaluation.TeacherEvaluation = teacherEvaluation;

    var scoreMapping = new Dictionary<InternshipEvaluations, int>
    {
        { InternshipEvaluations.A, 0 },
        { InternshipEvaluations.B, 5 },
        { InternshipEvaluations.C, 10 },
        { InternshipEvaluations.D, 15 },
        { InternshipEvaluations.E, 20 }
    };

    int totalScore = 0;

    totalScore += mentorEvaluation1.HasValue ? scoreMapping[mentorEvaluation1.Value] : 0;
    totalScore += mentorEvaluation2.HasValue ? scoreMapping[mentorEvaluation2.Value] : 0;
    totalScore += teacherEvaluation.HasValue ? scoreMapping[teacherEvaluation.Value] : 0;

    int totalScoreDivision = 0;

    totalScoreDivision += mentorEvaluation1.HasValue ? 1 : 0;
    totalScoreDivision += mentorEvaluation2.HasValue ? 1 : 0;
    totalScoreDivision += teacherEvaluation.HasValue ? 1 : 0;

    internshipEvaluation.OveralScore = totalScoreDivision > 0 ? totalScore / totalScoreDivision : null;    

    internshipEvaluation.Note = note;

    _dataProvidor.UpdateInternshipEvaluation(internshipEvaluation);
  }
}

