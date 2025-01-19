using Globals;
using Globals.Enums;

namespace LogicInterface;

public interface ILogicProvidor
{
  // User methods
  User? GetUser(int id);
  User? GetUserByEmail(string email);
  User? GetLoggedInUser();
  void LoginUser(string email, string password);
  void RegisterUser(string email, string password, string firstName, string secondName, UserTypes userType);
  void UpdateUser(User user, string? email, string? password, string? firstName, string? secondName, UserTypes? userType, string? cvPath);

  // Company methods
  Company? GetCompany(int id);
  Company? GetLoggedInCompany();
  void LoginCompany();
  void RegisterCompany(string email, string password, string firstName, string secondName, string companyName);

  // Internship methods
  List<Internship> GetAdminInternships(bool approved);
  List<Internship> GetStudentAvailableInternships();
  List<Internship> GetStudentInternship();
  List<Internship> GetTeacherInternships();
  List<Internship> GetCompanyInternships(bool approved);
  List<Internship> GetMentorInternships();
  void AddInternship(string title, string description);
  void ApproveInternship(Internship internship);
  void AssignInternship(Internship internship, string mentorEmail, string studentEmail, string teacherEmail);

  // InternshipCandidates methods
  List<InternshipCandidate> GetInternshipCandidates(int internshipId);
  void AddInternshipCandidate(int internshipId);

  // InternshipEvaluation methods
  InternshipEvaluation? GetInternshipEvaluation(int internshipId);
  void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation, InternshipEvaluations? mentorEvaluation1, InternshipEvaluations? mentorEvaluation2, InternshipEvaluations? teacherEvaluation, string note);
}

