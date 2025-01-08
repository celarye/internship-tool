using System;
using Globals.Interfaces;
using Logic;

namespace Presentation;

public class MainMenu()
{
  public void Display(ref ILogic _logic, ref Pages? currentPage)
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("Welcome to the Internship Tool");
      Console.WriteLine("1. Login");
      Console.WriteLine("2. Register");
      Console.WriteLine("3. Exit");
      Console.Write("Choose an option: ");

      string? choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          if (Login(ref _logic))
          {
            currentPage = Pages.StudentList;
            return;
          }
          break;
        case "2":
          Register(ref _logic);
          break;
        case "3":
          currentPage = null;
          return;
        default:
          break;
      }
    }
  }

  private bool Login(ref ILogic _logic)
  {
    Console.Clear();
    Console.WriteLine("Login:");
    Console.Write("Enter your username: ");
    string? username = Console.ReadLine();

    Console.Write("Enter your password: ");
    string? password = Console.ReadLine();

    bool invalid_input;

    if (username != null && password != null)
    {
      invalid_input = false;
    }
    else
    {
      invalid_input = true;
    }

    if (!invalid_input && _logic.ValidateLogin(username!, password!))
    {
      Console.Clear();
      Console.WriteLine("Login successful! Press Enter to return to the main menu.");
      Console.ReadLine();
      return true;
    }
    else
    {
      Console.Clear();
      Console.WriteLine("Invalid username or password. Press Enter to try again.");
      Console.ReadLine();
      return false;
    }
  }

  private void Register(ref ILogic _logic)
  {
    Console.Clear();
    Console.WriteLine("Register:");
    Console.Write("Enter a username: ");
    string? username = Console.ReadLine();

    Console.Write("Enter a password: ");
    string? password = Console.ReadLine();

    Console.Write("Confirm your password: ");
    string? confirmPassword = Console.ReadLine();

    if (password != confirmPassword && username != null && password != null)
    {
      Console.Clear();
      Console.WriteLine("Passwords do not match. Press Enter to try again.");
      Console.ReadLine();
      return;
    }

    if (_logic.RegisterUser(username!, password!))
    {
      Console.Clear();
      Console.WriteLine("Registration successful! Press Enter to return to the main menu.");
      Console.ReadLine();
    }
    else
    {
      Console.WriteLine("Registration failed. Username may already be taken. Press Enter to try again.");
      Console.ReadLine();
    }
  }
}

