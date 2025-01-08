using System;

using Globals.Interfaces;

namespace Presentation;

public class Program
{
  private static ILogic _logic = new Logic.Logic();
  private static Pages? currentPage = Pages.MainMenu;

  public static void Main(string[] args)
  {
    while (currentPage != null)
    {
      switch (currentPage)
      {
        case Pages.MainMenu:
          MainMenu mainMenu = new MainMenu();
          mainMenu.Display(ref _logic, ref currentPage);
          break;
        // case Pages.StudentList:
        //   StudentList studentList = new StudentList();
        //   studentList.Display();
        //   break;
        // case Pages.InternshipList:
        //   InternshipList internshipList = new InternshipList();
        //   internshipList.Display();
        //   break;
        default:
          currentPage = Pages.MainMenu;
          break;
      }
    }

    Console.WriteLine("Exiting the application. Goodbye!");
  }
}

