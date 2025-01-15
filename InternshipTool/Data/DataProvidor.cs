using System;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Globals;
using Globals.Enums;
using DataInterface;

namespace Data;

public class DataProvidor : IDataProvidor
{
  private const string UsersFilePath = "users.json";
  private const string CompaniesFilePath = "companies.json";
  private const string InternshipsFilePath = "internships.json";

  private int _userCount;
  private int _companyCount;

  public DataProvidor()
  {
    _userCount = GetLastId(UsersFilePath);
    _companyCount = GetLastId(CompaniesFilePath);
  }

  // General methods
  private int GetLastId(string path)
  {
    if (!File.Exists(path))
      return 0;

    using (StreamReader file = File.OpenText(path))
    {
      JArray items = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var lastItem = items
         .Cast<JObject>()
         .LastOrDefault();

      if (lastItem == null) return 0;

      return lastItem.Value<int>("Id");
    }
  }

  // User methods
  public User? GetUser(int id)
  {
    if (!File.Exists(UsersFilePath))
      return null;

    using (StreamReader file = File.OpenText(UsersFilePath))
    {
      JArray users = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var userMatch = users
          .Cast<JObject>()
          .Where(user => (int)user["Id"] == id)
          .FirstOrDefault();

      if (userMatch == null)
      {
        return null;
      }

      switch (userMatch["UserType"].ToObject<UserTypes>())
      {
        case UserTypes.Admin:
          return userMatch.ToObject<Admin>();
        case UserTypes.Student:
          return userMatch.ToObject<Student>();
        case UserTypes.Teacher:
          return userMatch.ToObject<Teacher>();
        default:
          return userMatch.ToObject<CompanyEmployee>();
      }
    }
  }

  public User? GetUserByEmail(string email)
  {
    if (!File.Exists(UsersFilePath))
      return null;

    using (StreamReader file = File.OpenText(UsersFilePath))
    {
      JArray users = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var userMatch = users
            .Cast<JObject>()
            .Where(user => user["Email"].ToString() == email)
            .FirstOrDefault();

      if (userMatch == null)
      {
        return null;
      }

      switch (userMatch["UserType"].ToObject<UserTypes>())
      {
        case UserTypes.Admin:
          return userMatch.ToObject<Admin>();
        case UserTypes.Student:
          return userMatch.ToObject<Student>();
        case UserTypes.Teacher:
          return userMatch.ToObject<Teacher>();
        default:
          return userMatch.ToObject<CompanyEmployee>();
      }
    }
  }

  public int GetNextUserId()
  {
    return ++_userCount;
  }

  public void AddUser(User user)
  {
    JArray users;
    if (File.Exists(UsersFilePath))
    {
      using (StreamReader file = File.OpenText(UsersFilePath))
      {
        users = (JArray)JToken.ReadFrom(new JsonTextReader(file));
      }
    }
    else
    {
      users = new JArray();
    }

    JObject userObject = JObject.FromObject(user);

    users.Add(userObject);

    using (StreamWriter file = File.CreateText(UsersFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        users.WriteTo(writer);
      }
    }
  }


  public void UpdateUser(User user)
  {
    JArray users;
    using (StreamReader file = File.OpenText(UsersFilePath))
    {
      users = (JArray)JToken.ReadFrom(new JsonTextReader(file));
    }

    JObject userObject = JObject.FromObject(user);

    for (int i = 0; i < users.Count; i++)
    {
      JObject existingUser = (JObject)users[i];
      if (existingUser["Id"].ToString() == user.Id.ToString())
      {
        users[i] = userObject;
        break;
      }
    }

    using (StreamWriter file = File.CreateText(UsersFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        users.WriteTo(writer);
      }
    }
  }


  // Company methods
  public int GetNextCompanyId()
  {
    return ++_companyCount;
  }

  public void AddCompany(Company company)
  {
    JArray companies;
    if (File.Exists(CompaniesFilePath))
    {
      using (StreamReader file = File.OpenText(CompaniesFilePath))
      {
        companies = (JArray)JToken.ReadFrom(new JsonTextReader(file));
      }
    }
    else
    {
      companies = new JArray();
    }

    JObject companyObject = JObject.FromObject(company);
    companies.Add(companyObject);

    using (StreamWriter file = File.CreateText(CompaniesFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        companies.WriteTo(writer);
      }
    }
  }

  // Internship methods
  public Internship? GetInternship(int id)
  {
    if (!File.Exists(InternshipsFilePath))
      return null;

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatch = internships
          .Cast<JObject>()
          .Where(internship => (int)internship["Id"] == id)
          .FirstOrDefault();

      if (internshipMatch == null)
      {
        return null;
      }

      return internshipMatch.ToObject<Internship>();
    }
  }

  public void AddInternship(Internship internship)
  {
    JArray internships;
    if (File.Exists(InternshipsFilePath))
    {
      using (StreamReader file = File.OpenText(InternshipsFilePath))
      {
        internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));
      }
    }
    else
    {
      internships = new JArray();
    }

    JObject internshipObject = JObject.FromObject(internship);
    internships.Add(internshipObject);

    using (StreamWriter file = File.CreateText(InternshipsFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internships.WriteTo(writer);
      }
    }
  }
}

