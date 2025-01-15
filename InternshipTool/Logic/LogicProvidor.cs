using System;
using System.Security.Cryptography;

using Globals;
using Globals.Enums;
using LogicInterface;
using Data;
using DataInterface;

namespace Logic;

public class LogicProvidor : ILogicProvidor
{
  private readonly IDataProvidor _dataProvidor;
  private User? _loggedInUser { get; set; }
  private Company? _loggedInCompany { get; set; }

  public LogicProvidor()
  {
    _dataProvidor = new Data.DataProvidor();
    _loggedInUser = null;
    _loggedInCompany = null;
  }

  public void LoginUser(string email, string password)
  {
    User? currentUser = _dataProvidor.GetUserByEmail(email);

    if (currentUser == null)
    {
      throw new UnauthorizedAccessException("No user with this email exists.");
    }

    if (VerifyPasswords(currentUser.Password, password))
    {
      _loggedInUser = currentUser;
    }

    if (_loggedInUser == null)
    {
      throw new UnauthorizedAccessException("Invalid email or password.");
    }
  }

  public void RegisterUser(string email, string firstName, string secondName, string password, UserTypes userType)
  {
    if (email.Length < 3)
    {
      throw new ArgumentException("Email is too short. Should be longer than 3 characters.");
    }

    if (password.Length < 3)
    {
      throw new ArgumentException("Password is too short. Should be longer than 3 characters.");
    }

    if (_dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("User with the same email already exists.");
    }

    User newUser;

    switch (userType)
    {
      case UserTypes.Admin:
        newUser = new Admin(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType);
        break;
      case UserTypes.Student:
        newUser = new Student(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType, null);
        break;
      case UserTypes.Teacher:
        newUser = new Teacher(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType);
        break;
      default:
        newUser = new CompanyEmployee(_dataProvidor.GetNextUserId(), email, firstName, secondName, HashPassword(null, password), userType, null);
        break;
    }

    _dataProvidor.AddUser(newUser);
  }

  public void UpdateUser(string? email, string? firstName, string? secondName, string? password, UserTypes? userType)
  {
    if (email != null && email.Length < 3)
    {
      throw new ArgumentException("Email is too short. Should be longer than 3 characters.");
    }
    else if (email != null && _dataProvidor.GetUserByEmail(email) != null)
    {
      throw new ArgumentException("User with the same email already exists.");
    }

    if (password != null && password.Length < 3)
    {
      throw new ArgumentException("Password is too short. Should be longer than 3 characters.");
    }

    if (userType != null)
    {
      _loggedInUser.UserType = (UserTypes)userType;
    }

    if (_loggedInUser.UserType == UserTypes.CompanyEmployee)
    {
      _loggedInUser = new CompanyEmployee(_loggedInUser.Id, _loggedInUser.Email, _loggedInUser.FirstName, _loggedInUser.SecondName, _loggedInUser.Password, _loggedInUser.UserType, _loggedInCompany.Id);
    }

    if (email != null)
    {
      _loggedInUser.Email = email;
    }

    if (firstName != null)
    {
      _loggedInUser.FirstName = firstName;
    }

    if (secondName != null)
    {
      _loggedInUser.SecondName = secondName;
    }

    if (password != null)
    {
      _loggedInUser.Password = password;
    }

    _dataProvidor.UpdateUser(_loggedInUser);
  }

  public byte[] GenerateSalt()
  {
    byte[] salt = new byte[16];
    using (var rng = new RNGCryptoServiceProvider())
    {
      rng.GetBytes(salt);
    }
    return salt;
  }

  public string HashPassword(byte[] salt, string password)
  {
    if (salt == null)
    {
      salt = GenerateSalt();
    }

    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
    byte[] hash = pbkdf2.GetBytes(20);

    byte[] hashBytes = new byte[36];
    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 20);

    return Convert.ToBase64String(hashBytes);
  }

  public bool VerifyPasswords(string savedPasswordHash, string inputPassword)
  {
    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

    byte[] salt = new byte[16];
    Array.Copy(hashBytes, 0, salt, 0, 16);

    // // Use the salt to hash the input password
    // var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, 100000);
    // byte[] hash = pbkdf2.GetBytes(20);

    return HashPassword(salt, inputPassword) == savedPasswordHash;

    // // Compare the stored hash (last 20 bytes) with the newly derived hash
    // for (int i = 0; i < 20; i++)
    // {
    //   if (hashBytes[i + 16] != hash[i])
    //   {
    //     return false; // Mismatch found
    //   }
    // }

    // return true; // All bytes match
  }

  public void RegisterCompany(string companyName)
  {
    if (companyName.Length < 3)
    {
      throw new ArgumentException("Company name is too short. Should be longer than 3 characters.");
    }

    var newCompany = new Company(_dataProvidor.GetNextCompanyId(), companyName, _loggedInUser.Id);

    // Unlike with users, registering companies sets them as the _loggedInCompany
    _loggedInCompany = newCompany;

    _dataProvidor.AddCompany(newCompany);
  }
}

