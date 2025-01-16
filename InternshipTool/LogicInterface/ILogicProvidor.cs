using Globals;
using Globals.Enums;

namespace LogicInterface;

public interface ILogicProvidor
{
  // User methods
  User GetUser(int id);
  void LoginUser(string email, string password);
  void RegisterUser(string email, string password, string firstName, string secondName, UserTypes userType);
  void UpdateUser(string email, string password, string firstName, string secondName, UserTypes userType);
  void AddCVToStudent(string cvPath);

  // Company methods
  Company GetCompany(int id);
  void RegisterCompany(string companyName);

  // Internship methods
  List<Internship> GetAdminInternships(bool approved);
  List<Internship> GetStudentAvailableInternships();
  Internship? GetStudentInternship();
  List<Internship> GetTeacherInternships();
  List<Internship> GetCompanyInternships(bool approved);
  List<Internship> GetMentorInternships();
  void AddInternship(string title, string description, int mentorId);
  void ApproveInternship(Internship internship);
  void AssignInternship(Internship internship, int studentId, int teacherId);

  // InternshipCandidates methods
  List<InternshipCandidate> GetInternshipCandidates(int internshipId);
  List<InternshipCandidate> GetStudentInternshipCandidacies();
  void AddInternshipCandidate(int internshipId);
  void RemoveInternshipCandidates(int internshipId);
  void RemoveInternshipCandidacies(int studentId);

  // InternshipEvaluation methods
  InternshipEvaluation GetInternshipEvaluation(int internshipId);
  void AddInternshipEvaluation(int internshipId);
  void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation, InternshipEvaluations? mentorEvaluation1, InternshipEvaluations? teacherEvaluation1, InternshipEvaluations? mentorEvaluation2, InternshipEvaluations? teacherEvaluation2, string note);
}

