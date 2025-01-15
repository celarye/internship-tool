using Globals;

namespace DataInterface;

public interface IDataProvidor
{
  // User methods
  User GetUser(int id);
  User? GetUserByEmail(string email);
  int GetNextUserId();
  void AddUser(User user);
  void UpdateUser(User user);

  // Company methods
  Company GetCompany(int id);
  int GetNextCompanyId();
  void AddCompany(Company company);

  // Internship methods
  List<Internship>? GetInternships(bool approved);
  List<Internship>? GetInternshipsByStudent(int studentId); // approved!
  List<Internship>? GetInternshipsByTeacher(int teacherId);
  List<Internship>? GetInternshipsByCompany(bool approved, int companyId);
  List<Internship>? GetInternshipsByMentor(int mentorId);
  void AddInternship(Internship internship);
  void UpdateInternship(Internship internship);

  // InternshipCandidates methods
  List<InternshipCandidate>? GetInternshipCandidatesByInternship(int internshipId);
  List<InternshipCandidate>? GetInternshipCandidatesByStudent(int studentId);
  void AddInternshipCandidate(InternshipCandidate internshipCandidate);
  void RemoveInternshipCandidates(List<InternshipCandidate> internshipCandidates);

  // InternshipEvaluation methods
  InternshipEvaluation GetInternshipEvaluationByInternship(int internshipId);
  void AddInternshipEvaluation(InternshipEvaluation internshipEvaluation);
  void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation);
}

