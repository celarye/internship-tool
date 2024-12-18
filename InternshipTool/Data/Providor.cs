using Globals.Interfaces;

namespace Data;

class Providor
{
  public List<IUser> listUsers()
  {
    List<IUser> userlist = new List<IUser>();


    FileStream fileStream = File.OpenRead($"data/users.csv");
    StreamReader streamReader = new StreamReader(fileStream);

    string? line;
    while ((line = streamReader.ReadLine()) != null)
    {
      string[] line_values = line.Split(",");

      if (line.Length != 6)
      {
        continue;
      }

      IUser user = new

    }

    return userlist;
  }
}
