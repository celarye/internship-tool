namespace Globals;

public class Internship(int id, int companyId, string title, string description, int internshipMentor, int? internshipTeacher, int? internshipStudent)
{
  public int Id { get; } = id;
  public int CompanyId { get; set; } = companyId;
  public string Title { get; set; } = title;
  public string Description { get; set; } = description;
  public int InternshipMentor { get; set; } = internshipMentor;
  public int? InternshipTeacher { get; set; } = internshipTeacher;
  public int? InternshipStudent { get; set; } = internshipStudent;

  public override string ToString()
  {
    return "User: " + Id + ", " + CompanyId + ", " + Title + ", " + Description + ", " + InternshipMentor + ", " + InternshipTeacher + ", " + InternshipStudent;
  }
}

