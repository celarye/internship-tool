using System;

using Globals.Enums;
using Logic;
using LogicInterface;

namespace Presentation;

public class Program
{
  public static ILogicProvidor _logicProvidor = new Logic.LogicProvidor();

  public static void Main(string[] args)
  {
    try
    {
      _logicProvidor.RegisterUser("test1", "a", "b", "test1", UserTypes.CompanyEmployee);

      Console.WriteLine("Succesfully registered user");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.LoginUser("test1", "test1");

      Console.WriteLine("Succesfully logged in user");
    }
    catch (UnauthorizedAccessException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.RegisterCompany("test_company");

      Console.WriteLine("Succesfully registered company");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.UpdateUser(null, null, null, null, null);

      Console.WriteLine("Succesfully updated user");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.RegisterUser("test2", "c", "d", "test2", UserTypes.Student);

      Console.WriteLine("Succesfully registered user");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.RegisterUser("test3", "e", "f", "test3", UserTypes.Teacher);

      Console.WriteLine("Succesfully registered user");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.RegisterUser("test4", "g", "h", "test4", UserTypes.Admin);

      Console.WriteLine("Succesfully registered user");
    }
    catch (ArgumentException e)
    {
      Console.WriteLine(e.Message);
    }

    try
    {
      _logicProvidor.LoginUser("test2", "test2");

      Console.WriteLine("Succesfully logged in user");
    }
    catch (UnauthorizedAccessException e)
    {
      Console.WriteLine(e.Message);
    }

    Console.WriteLine("Exiting");
  }
}

