using Globals;

namespace DataInterface;

public interface IDataProvidor
{
  // User methods
  User? GetUser(int id);
  User? GetUserByEmail(string email);
  int GetNextUserId();
  void AddUser(User user);
  void UpdateUser(User user);

  // Company methods
  Company? GetCompany(int id);
  int GetNextCompanyId();
  void AddCompany(Company company);

  // Internship methods
  List<Internship> GetInternships(bool approved);
  List<Internship> GetInternshipsByStudent(int? studentId);

  List<Internship> GetInternshipsNotByStudent();
  List<Internship> GetInternshipsByTeacher(int teacherId);
  List<Internship> GetInternshipsByCompany(bool approved, int companyId);
  List<Internship> GetInternshipsByMentor(int mentorId);
  int GetNextInternshipId();
  void AddInternship(Internship internship);
  void UpdateInternship(Internship internship);

  // InternshipCandidate methods
  List<InternshipCandidate> GetInternshipCandidatesByInternship(int internshipId);
  List<InternshipCandidate> GetInternshipCandidatesByStudent(int studentId);
  int GetNextInternshipCandidateId();
  void AddInternshipCandidate(InternshipCandidate internshipCandidate);
  void RemoveInternshipCandidates(int internshipId);
  void RemoveInternshipCandidacies(int studentId);

  // InternshipEvaluation methods
  InternshipEvaluation? GetInternshipEvaluationByInternship(int internshipId);
  int GetNextInternshipEvaluationId();
  void AddInternshipEvaluation(InternshipEvaluation internshipEvaluation);
  void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation);
}

