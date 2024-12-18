namespace Globals;

public class Admin : User
{
  public Admin(Int32 id, string username, string password, UserTypes userType) : base(id, username, password, userType) { }
}
