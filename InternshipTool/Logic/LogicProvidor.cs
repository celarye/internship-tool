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

  public User GetUser(int id)
  {
    return _dataProvidor.GetUser(id);
  }

  public void LoginUser(string email, string password)
  {
    User? currentUser = _dataProvidor.GetUserByEmail(email);

    if (currentUser == null)
    {
      throw new UnauthorizedAccessException("No user with this email exists.");
    }

    if (VerifyPasswords(currentUser.Password, password))
    {
      _loggedInUser = currentUser;
    }

    if (_loggedInUser == null)
    {
      throw new UnauthorizedAccessException("Invalid email or password.");
    }
  }

  public void RegisterUser(string email, string firstName, string secondName, string password, UserTypes userType)
  {
    if (email.Length < 3)
    {
      throw new ArgumentException("Email is too short. Should be longer than 3 characters.");
    }

    if (password.Length < 3)
    {
      throw new ArgumentException("Password is too short. Should be longer than 3 characters.");
    }

    if (_dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("User with the same email already exists.");
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
        newUser = new CompanyEmployee(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType, null);
        break;
    }

    _dataProvidor.AddUser(newUser);
  }

  public void UpdateUser(string email, string firstName, string secondName, string password, UserTypes userType)
  {
    if (email.Length < 3)
    {
      throw new ArgumentException("Email is too short. Should be longer than 3 characters.");
    }
    else if (_loggedInUser.Email != email && _dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("User with the same email already exists.");
    }

    if (password.Length < 3)
    {
      throw new ArgumentException("Password is too short. Should be longer than 3 characters.");
    }

    _loggedInUser.UserType = userType;

    if (_loggedInUser.UserType == UserTypes.CompanyEmployee)
    {
      CompanyEmployee companyEmployee = ((CompanyEmployee)_loggedInUser);

      companyEmployee.CompanyId = _loggedInCompany.Id;

      _loggedInUser = companyEmployee;
    }

    _loggedInUser.Email = email;

    _loggedInUser.FirstName = firstName;

    _loggedInUser.SecondName = secondName;

    if (_loggedInUser.Password != password)
    {
      _loggedInUser.Password = HashPassword(null, password);
    }

    _dataProvidor.UpdateUser(_loggedInUser);
  }

  public void AddCVToStudent(string cvPath)
  {
    // TODO: Save PDF

    Student student = ((Student)_loggedInUser);

    student.CVPath = cvPath;

    _loggedInUser = student;

    _dataProvidor.UpdateUser(_loggedInUser);
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
  public Company GetCompany(int id)
  {
    return _dataProvidor.GetCompany(id);
  }

  public void RegisterCompany(string companyName)
  {
    if (companyName.Length < 3)
    {
      throw new ArgumentException("Company name is too short. Should be longer than 3 characters.");
    }

    var newCompany = new Company(_dataProvidor.GetNextCompanyId(), companyName, _loggedInUser.Id);

    // Unlike with users, registering companies sets them as the _loggedInCompany
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
    return _dataProvidor.GetInternshipsByStudent(null);
  }

  public Internship? GetStudentInternship()
  {
    return _dataProvidor.GetInternshipsByStudent(_loggedInUser.Id)[0];
  }

  public List<Internship> GetTeacherInternships()
  {
    return _dataProvidor.GetInternshipsByTeacher(_loggedInUser.Id);
  }

  public List<Internship> GetCompanyInternships(bool approved)
  {
    return _dataProvidor.GetInternshipsByCompany(approved, _loggedInCompany.Id);
  }

  public List<Internship> GetMentorInternships()
  {
    return _dataProvidor.GetInternshipsByMentor(_loggedInUser.Id);
  }

  public void AddInternship(string title, string description, int mentorId)
  {
    Internship internship = new Internship(_dataProvidor.GetNextInternshipId(), _loggedInCompany.Id, title, description, mentorId);

    _dataProvidor.AddInternship(internship);
  }

  public void ApproveInternship(Internship internship)
  {
    internship.Approved = true;

    _dataProvidor.UpdateInternship(internship);
  }

  public void AssignInternship(Internship internship, int studentId, int teacherId)
  {
    internship.StudentId = studentId;
    internship.TeacherId = teacherId;

    _dataProvidor.UpdateInternship(internship);
  }

  // InternshipCandidate methods
  public List<InternshipCandidate> GetInternshipCandidates(int internshipId)
  {
    return _dataProvidor.GetInternshipCandidatesByInternship(internshipId);
  }

  public List<InternshipCandidate> GetStudentInternshipCandidacies()
  {
    return _dataProvidor.GetInternshipCandidatesByStudent(_loggedInUser.Id);
  }

  public void AddInternshipCandidate(int internshipId)
  {
    if (((Student)_loggedInUser).CVPath == null)
    {
      throw new UnauthorizedAccessException("You need to have a CV to be able to put yourself as a candidate for an internship");
    }

    InternshipCandidate internshipCandidate = new InternshipCandidate(_loggedInUser.Id, internshipId);

    _dataProvidor.AddInternshipCandidate(internshipCandidate);
  }

  public void RemoveInternshipCandidates(int internshipId)
  {
    _dataProvidor.RemoveInternshipCandidates(internshipId);
  }

  public void RemoveInternshipCandidacies(int studentId)
  {
    _dataProvidor.RemoveInternshipCandidacies(studentId);
  }

  public InternshipEvaluation GetInternshipEvaluation(int internshipId)
  {
    return _dataProvidor.GetInternshipEvaluationByInternship(internshipId);
  }

  public void AddInternshipEvaluation(int internshipId)
  {
    InternshipEvaluation internshipEvaluation = new InternshipEvaluation(_dataProvidor.GetNextInternshipEvaluationId(), internshipId);

    _dataProvidor.AddInternshipEvaluation(internshipEvaluation);
  }

  public void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation, InternshipEvaluations? mentorEvaluation1, InternshipEvaluations? teacherEvaluation1, InternshipEvaluations? mentorEvaluation2, InternshipEvaluations? teacherEvaluation2, string note)
  {
    internshipEvaluation.MentorEvaluation1 = mentorEvaluation1;
    internshipEvaluation.TeacherEvaluation1 = teacherEvaluation1;
    internshipEvaluation.MentorEvaluation2 = mentorEvaluation2;
    internshipEvaluation.TeacherEvaluation2 = teacherEvaluation2;

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
    totalScore += teacherEvaluation1.HasValue ? scoreMapping[teacherEvaluation1.Value] : 0;
    totalScore += mentorEvaluation2.HasValue ? scoreMapping[mentorEvaluation2.Value] : 0;
    totalScore += teacherEvaluation2.HasValue ? scoreMapping[teacherEvaluation2.Value] : 0;

    internshipEvaluation.OveralScore = totalScore;

    internshipEvaluation.Note = note;

    _dataProvidor.UpdateInternshipEvaluation(internshipEvaluation);
  }
}

