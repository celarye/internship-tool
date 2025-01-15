using Globals;
using Globals.Enums;

namespace LogicInterface;

public interface ILogicProvidor
{
  // User methods
  void LoginUser(string email, string password);
  void RegisterUser(string email, string password, string firstName, string secondName, UserTypes userType);
  // TODO: Allow adding a CV
  void UpdateUser(string email, string password, string firstName, string secondName, UserTypes userType);

  // Company methods
  Company getCompany(int id);
  void RegisterCompany(string companyName);

  // Internship methods
  List<Internship>? getAdminUnapprovedInternships();
  List<Internship>? getAdminApprovedInternships();
  List<Internship>? getStudentAvailableInternships();
  Internship? getStudentInternship();
  List<Internship>? getTeacherInternships();
  List<Internship>? getCompanyUnapprovedInternships();
  List<Internship>? getCompanyApprovedInternships();
  List<Internship>? getMentorInternships();
  void AddInternship(string title, string description, int mentorId);
  void UpdateInternship(string title, string description, int mentorId, int studentId, int teacherId);

  // InternshipCandidates methods
  List<InternshipCandidate>? GetInternshipCandidates(int internshipId);
  List<InternshipCandidate>? GetStudentInternshipCandidacies();
  void AddInternshipCandidate(int internshipId);
  void RemoveInternshipCandidates(int internshipId);

  // InternshipEvaluation methods
  InternshipEvaluation GetInternshipEvaluation(int internshipId);
  void AddInternshipEvaluation(int internshipId, InternShipEvaluations mentorEvaluation1, Internshipevaluations teacherEvaluation1, Internshipevaluations mentorEvaluation2, Internshipevaluations teacherEvaluation2, int overalScore, string note);
  void UpdateInternshipEvaluation(int internshipId, InternShipEvaluations mentorEvaluation1, Internshipevaluations teacherEvaluation1, Internshipevaluations mentorEvaluation2, Internshipevaluations teacherEvaluation2, int overalScore, string note);
}
