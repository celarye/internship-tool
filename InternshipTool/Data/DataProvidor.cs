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
  private const string InternshipsCandidateFilePath = "internships_candidate.json";
  private const string InternshipsEvaluationFilePath = "internships_evaluation.json";

  private int _userCount;
  private int _companyCount;
  private int _internshipCount;
  private int _internshipCandidateCount;
  private int _internshipEvaluationCount;

  public DataProvidor()
  {
    _userCount = GetLastId(UsersFilePath);
    _companyCount = GetLastId(CompaniesFilePath);
    _internshipCount = GetLastId(InternshipsFilePath);
    _internshipCandidateCount = GetLastId(InternshipsCandidateFilePath);
    _internshipEvaluationCount = GetLastId(InternshipsEvaluationFilePath);
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
  public User GetUser(int id)
  {
    using (StreamReader file = File.OpenText(UsersFilePath))
    {
      JArray users = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var userMatch = users
          .Cast<JObject>()
          .Where(user => (int)user["Id"] == id)
          .FirstOrDefault();

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

      if ((int)existingUser["Id"] == user.Id)
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
  public Company GetCompany(int id)
  {
    using (StreamReader file = File.OpenText(CompaniesFilePath))
    {
      JArray companies = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var companyMatch = companies
          .Cast<JObject>()
          .Where(company => (int)company["Id"] == id)
          .FirstOrDefault();

      return companyMatch.ToObject<Company>();
    }
  }

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
  public List<Internship> GetInternships(bool approved)
  {
    if (!File.Exists(InternshipsFilePath))
      return new List<Internship>();

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatches = internships
          .Cast<JObject>()
          .Where(internship => (bool)internship["Approved"] == approved);

      return internshipMatches.Select(jObject => jObject.ToObject<Internship>()).ToList();
    }
  }

  public List<Internship> GetInternshipsByStudent(int? studentId)
  {
    if (!File.Exists(InternshipsFilePath))
      return new List<Internship>();

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatches = internships
          .Cast<JObject>()
          .Where(internship => (bool)internship["Approved"] == true && (int)internship["StudentId"] == studentId);

      return internshipMatches.Select(jObject => jObject.ToObject<Internship>()).ToList();
    }
  }

  public List<Internship> GetInternshipsByTeacher(int teacherId)
  {
    if (!File.Exists(InternshipsFilePath))
      return new List<Internship>();

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatches = internships
          .Cast<JObject>()
          .Where(internship => (int)internship["TeacherId"] == teacherId);

      return internshipMatches.Select(jObject => jObject.ToObject<Internship>()).ToList();
    }
  }

  public List<Internship> GetInternshipsByCompany(bool approved, int companyId)
  {
    if (!File.Exists(InternshipsFilePath))
      return new List<Internship>();

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatches = internships
          .Cast<JObject>()
          .Where(internship => (bool)internship["Approved"] == approved && (int)internship["CompanyId"] == companyId);

      return internshipMatches.Select(jObject => jObject.ToObject<Internship>()).ToList();
    }
  }

  public List<Internship> GetInternshipsByMentor(int mentorId)
  {
    if (!File.Exists(InternshipsFilePath))
      return new List<Internship>();

    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      JArray internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipMatches = internships
          .Cast<JObject>()
          .Where(internship => (int)internship["MentorId"] == mentorId);

      return internshipMatches.Select(jObject => jObject.ToObject<Internship>()).ToList();
    }
  }

  public int GetNextInternshipId()
  {
    return ++_internshipCount;
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

  public void UpdateInternship(Internship internship)
  {
    JArray internships;
    using (StreamReader file = File.OpenText(InternshipsFilePath))
    {
      internships = (JArray)JToken.ReadFrom(new JsonTextReader(file));
    }

    JObject internshipObject = JObject.FromObject(internship);

    for (int i = 0; i < internships.Count; i++)
    {
      JObject existingInternship = (JObject)internships[i];

      if ((int)existingInternship["Id"] == internship.Id)
      {
        internships[i] = internshipObject;
        break;
      }
    }

    using (StreamWriter file = File.CreateText(InternshipsFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internships.WriteTo(writer);
      }
    }
  }

  // InternshipCandidate methods
  public List<InternshipCandidate> GetInternshipCandidatesByInternship(int internshipId)
  {
    if (!File.Exists(InternshipsCandidateFilePath))
      return new List<InternshipCandidate>();

    using (StreamReader file = File.OpenText(InternshipsCandidateFilePath))
    {
      JArray internshipCandidates = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipCandidateMatches = internshipCandidates
          .Cast<JObject>()
          .Where(internshipCandidate => (int)internshipCandidate["InternshipId"] == internshipId);

      return internshipCandidateMatches.Select(jObject => jObject.ToObject<InternshipCandidate>()).ToList();
    }
  }

  public List<InternshipCandidate> GetInternshipCandidatesByStudent(int studentId)
  {
    if (!File.Exists(InternshipsCandidateFilePath))
      return new List<InternshipCandidate>();

    using (StreamReader file = File.OpenText(InternshipsCandidateFilePath))
    {
      JArray internshipCandidates = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipCandidateMatches = internshipCandidates
          .Cast<JObject>()
          .Where(internshipCandidate => (int)internshipCandidate["StudentId"] == studentId);

      return internshipCandidateMatches.Select(jObject => jObject.ToObject<InternshipCandidate>()).ToList();
    }
  }

  public int GetNextInternshipCandidateId()
  {
    return ++_internshipCandidateCount;
  }

  public void AddInternshipCandidate(InternshipCandidate internshipCandidate)
  {
    JArray internshipCandidates;
    if (File.Exists(InternshipsCandidateFilePath))
    {
      using (StreamReader file = File.OpenText(InternshipsCandidateFilePath))
      {
        internshipCandidates = (JArray)JToken.ReadFrom(new JsonTextReader(file));
      }
    }
    else
    {
      internshipCandidates = new JArray();
    }

    JObject internshipCandidateObject = JObject.FromObject(internshipCandidate);
    internshipCandidates.Add(internshipCandidateObject);

    using (StreamWriter file = File.CreateText(InternshipsCandidateFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internshipCandidates.WriteTo(writer);
      }
    }
  }

  public void RemoveInternshipCandidates(int internshipId)
  {
    JArray internshipCandidates;
    using (StreamReader file = File.OpenText(InternshipsCandidateFilePath))
    {
      internshipCandidates = (JArray)JToken.ReadFrom(new JsonTextReader(file));
    }

    for (int i = 0; i < internshipCandidates.Count; i++)
    {
      JObject existingInternshipCandidate = (JObject)internshipCandidates[i];

      if ((int)existingInternshipCandidate["Id"] == internshipId)
      {
        internshipCandidates.Remove(existingInternshipCandidate);
      }
    }

    using (StreamWriter file = File.CreateText(InternshipsCandidateFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internshipCandidates.WriteTo(writer);
      }
    }
  }

  public void RemoveInternshipCandidacies(int studentId)
  {
    JArray internshipCandidates;
    using (StreamReader file = File.OpenText(InternshipsCandidateFilePath))
    {
      internshipCandidates = (JArray)JToken.ReadFrom(new JsonTextReader(file));
    }

    for (int i = 0; i < internshipCandidates.Count; i++)
    {
      JObject existingInternshipCandidate = (JObject)internshipCandidates[i];

      if ((int)existingInternshipCandidate["StudentId"] == studentId)
      {
        internshipCandidates.Remove(existingInternshipCandidate);
      }
    }

    using (StreamWriter file = File.CreateText(InternshipsCandidateFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internshipCandidates.WriteTo(writer);
      }
    }
  }

  // InternshipEvaluation methods
  public InternshipEvaluation GetInternshipEvaluationByInternship(int internshipId)
  {
    using (StreamReader file = File.OpenText(InternshipsEvaluationFilePath))
    {
      JArray internshipEvaluations = (JArray)JToken.ReadFrom(new JsonTextReader(file));

      var internshipEvaluationMatches = internshipEvaluations
          .Cast<JObject>()
          .Where(internshipEvaluation => (int)internshipEvaluation["InternshipId"] == internshipId)
          .First();

      return internshipEvaluationMatches.ToObject<InternshipEvaluation>();
    }
  }

  public int GetNextInternshipEvaluationId()
  {
    return ++_internshipEvaluationCount;
  }

  public void AddInternshipEvaluation(InternshipEvaluation internshipEvaluation)
  {
    JArray internshipEvaluations;
    if (File.Exists(InternshipsEvaluationFilePath))
    {
      using (StreamReader file = File.OpenText(InternshipsEvaluationFilePath))
      {
        internshipEvaluations = (JArray)JToken.ReadFrom(new JsonTextReader(file));
      }
    }
    else
    {
      internshipEvaluations = new JArray();
    }

    JObject internshipEvaluationObject = JObject.FromObject(internshipEvaluation);
    internshipEvaluations.Add(internshipEvaluationObject);

    using (StreamWriter file = File.CreateText(InternshipsEvaluationFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internshipEvaluations.WriteTo(writer);
      }
    }
  }

  public void UpdateInternshipEvaluation(InternshipEvaluation internshipEvaluation)
  {
    JArray internshipEvaluations;
    using (StreamReader file = File.OpenText(InternshipsEvaluationFilePath))
    {
      internshipEvaluations = (JArray)JToken.ReadFrom(new JsonTextReader(file));
    }

    JObject internshipEvaluationObject = JObject.FromObject(internshipEvaluation);

    for (int i = 0; i < internshipEvaluations.Count; i++)
    {
      JObject existingInternshipEvaluations = (JObject)internshipEvaluations[i];

      if ((int)existingInternshipEvaluations["Id"] == internshipEvaluation.Id)
      {
        internshipEvaluations[i] = internshipEvaluationObject;
        break;
      }
    }

    using (StreamWriter file = File.CreateText(InternshipsEvaluationFilePath))
    {
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        writer.Formatting = Formatting.Indented;
        internshipEvaluations.WriteTo(writer);
      }
    }
  }
}

