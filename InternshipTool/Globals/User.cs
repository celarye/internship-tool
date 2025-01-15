using Globals.Enums;

namespace Globals;

public abstract class User(int id, string email, string firstName, string secondName, string password, UserTypes userType)
{
  public int Id { get; } = id;
  public string Email { get; set; } = email;
  public string FirstName { get; set; } = firstName;
  public string SecondName { get; set; } = secondName;
  public string Password { get; set; } = password;
  public UserTypes UserType { get; set; } = userType;

  public override string ToString()
  {
    return "User: " + Id + ", " + Email + ", " + FirstName + ", " + SecondName + ", " + Password + ", " + UserType;
  }
}

public class Admin : User
{
  public Admin(int id, string email, string firstName, string secondName, string password, UserTypes userType) : base(id, email, firstName, secondName, password, userType)
  { }
}

public class Student : User
{
  public string? CVPath { get; set; }

  public Student(int id, string email, string firstName, string secondName, string password, UserTypes userType, string? cvPath) : base(id, email, firstName, secondName, password, userType)
  {
    CVPath = cvPath;
  }
}

public class Teacher : User
{
  public Teacher(int id, string email, string firstName, string secondName, string password, UserTypes userType) : base(id, email, firstName, secondName, password, userType)
  { }
}

public class CompanyEmployee : User
{
  public int? CompanyId { get; set; }

  public CompanyEmployee(int id, string email, string firstName, string secondName, string password, UserTypes userType, int? companyId) : base(id, email, firstName, secondName, password, userType)
  {
    CompanyId = companyId;
  }

  public override string ToString()
  {
    return "User: " + Id + ", " + Email + ", " + FirstName + ", " + SecondName + ", " + Password + ", " + UserType + ", " + CompanyId;
  }
}
