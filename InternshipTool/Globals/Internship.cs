using Globals.Enums;
namespace Globals;

public class Internship(int id, int companyId, string title, string description, int mentorId)
{
  public int Id { get; } = id;
  public int CompanyId { get; } = companyId;
  public string Title { get; set; } = title;
  public string Description { get; set; } = description;
  public int MentorId { get; set; } = mentorId;
  public bool Approved { get; set; } = false;
  public int? StudentId { get; set; }
  public int? TeacherId { get; set; }

  public override string ToString()
  {
    return "Internship: " + Id + ", " + CompanyId + ", " + Title + ", " + Description + ", " + MentorId + ", " + Approved + ", " + StudentId + ", " + TeacherId;
  }
}

public class InternshipCandidate(int internshipId, int studentId)
{
  public int InternshipId { get; } = internshipId;
  public int StudentId { get; } = studentId;

  public override string ToString()
  {
    return "Internship Candidate: " + InternshipId + ", " + StudentId;
  }

}

public class InternshipEvaluation(int id, int internshipId)
{
  public int Id { get; } = id;
  public int InternShipId { get; } = internshipId;
  public InternshipEvaluations? MentorEvaluation1 { get; set; }
  public InternshipEvaluations? TeacherEvaluation1 { get; set; }
  public InternshipEvaluations? MentorEvaluation2 { get; set; }
  public InternshipEvaluations? TeacherEvaluation2 { get; set; }
  public int? OveralScore { get; set; }
  public string? Note { get; set; }

  public override string ToString()
  {
    return "Internship Evaluation: " + Id + ", " + InternShipId + ", " + MentorEvaluation1 + ", " + TeacherEvaluation1 + ", " + MentorEvaluation2 + ", " + TeacherEvaluation2 + ", " + OveralScore + ", " + Note;
  }
}
