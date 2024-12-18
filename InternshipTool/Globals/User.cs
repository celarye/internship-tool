namespace Globals;

public abstract class User(Int32 id, string username, string password, UserTypes userType)
{
  private readonly Int32 _id = id;
  private string _username = username;
  private string _password = password;
  private UserTypes _userType = userType;

  public Int32 Id { get; }
  public string Username { get { return Username; } set { Username = value; } }
  public string Password { get { return Password; } set { Password = value; } }
  public UserTypes UserType { get { return UserType; } set { UserType = value; } }
}
