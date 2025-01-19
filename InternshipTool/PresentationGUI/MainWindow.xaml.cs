using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Globals;
using Globals.Enums;
using LogicInterface;

namespace PresentationGUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public static readonly ILogicProvidor _logicProvidor = new Logic.LogicProvidor();
    public MainWindow()
    {
      InitializeComponent();
      try
      {
        // Admin
        _logicProvidor.RegisterUser("admin1@example.com", "admin1", "Eduard", "Smet", UserTypes.Admin);

        // Teachers
        _logicProvidor.RegisterUser("teacher1@example.com", "teacher1", "John", "Doe", UserTypes.Teacher);
        _logicProvidor.RegisterUser("teacher2@example.com", "teacher2", "Alice", "Brown", UserTypes.Teacher);
        _logicProvidor.RegisterUser("teacher3@example.com", "teacher3", "Sarah", "Johnson", UserTypes.Teacher);

        // Students
        _logicProvidor.RegisterUser("student1@example.com", "student1", "Mark", "Smith", UserTypes.Student);
        _logicProvidor.RegisterUser("student2@example.com", "student2", "James", "Williams", UserTypes.Student);
        _logicProvidor.RegisterUser("student3@example.com", "student3", "Olivia", "Davis", UserTypes.Student);
        _logicProvidor.RegisterUser("student4@example.com", "student4", "Liam", "Miller", UserTypes.Student);
        _logicProvidor.RegisterUser("student5@example.com", "student5", "Sophia", "Wilson", UserTypes.Student);
        _logicProvidor.RegisterUser("student6@example.com", "student6", "Jackson", "Moore", UserTypes.Student);
        _logicProvidor.RegisterUser("student7@example.com", "student7", "Emily", "Taylor", UserTypes.Student);
        _logicProvidor.RegisterUser("student8@example.com", "student8", "Daniel", "Anderson", UserTypes.Student);
        _logicProvidor.RegisterUser("student9@example.com", "student9", "Ava", "Thomas", UserTypes.Student);
        _logicProvidor.RegisterUser("student10@example.com", "student10", "Mason", "Jackson", UserTypes.Student);

        // Register Companies
        _logicProvidor.RegisterCompany("manager1@example.com", "manager1", "Eduard", "Smet", "Tech Innovations");
        _logicProvidor.RegisterCompany("manager2@example.com", "manager2", "John", "Doe", "Creative Solutions");
        _logicProvidor.RegisterCompany("manager3@example.com", "manager3", "Alice", "Brown", "HealthCare Systems");
      }
      catch { }
    }

    private void Login(object sender, RoutedEventArgs e)
    {
      string email = LoginEmailTextBox.Text;
      string password = LoginPasswordBox.Password;

      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
      {
        ErrorLabel.Content = "Email, password can't be empty";
        return;
      }
      try
      {
        _logicProvidor.LoginUser(email, password);

        switch (_logicProvidor.GetLoggedInUser()!.UserType)
        {
          case UserTypes.Admin:
            DashBoardWindow adminWindow = new DashBoardWindow();
            adminWindow.Show();
            break;
          case UserTypes.Student:
            DashBoardWindow studentWindow = new DashBoardWindow();
            studentWindow.Show();
            break;
          case UserTypes.Teacher:
            DashBoardWindow teacherWindow = new DashBoardWindow();
            teacherWindow.Show();
            break;
          case UserTypes.CompanyEmployee:
            _logicProvidor.LoginCompany();
            DashBoardWindow companyEmployeeWindow = new DashBoardWindow();
            companyEmployeeWindow.Show();
            break;
        }

        this.Hide();
      }
      catch (UnauthorizedAccessException err)
      {
        ErrorLabel.Content = err.Message;
      }
    }

    private void Register(object sender, RoutedEventArgs e)
    {
      string email = RegisterEmailTextBox.Text;
      string password = RegisterPasswordBox.Password;
      string firstName = RegisterFirstNameTextBox.Text;
      string secondName = RegisterSecondNameTextBox.Text;
      string companyName = RegisterCompanyNameTextBox.Text;

      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName) || string.IsNullOrEmpty(companyName))
      {
        ErrorLabel.Content = "Email, password, company name, first name, second name can't be empty";
        return;
      }

      try
      {
        _logicProvidor.RegisterCompany(email, password, firstName, secondName, companyName);
        _logicProvidor.LoginUser(email, password);
      }
      catch (ArgumentException err)
      {
        ErrorLabel.Content = err.Message;
      }
      catch (UnauthorizedAccessException err)
      {
        ErrorLabel.Content = err.Message;
      }

      DashBoardWindow companyEmployeeWindow = new DashBoardWindow();
      companyEmployeeWindow.Show();

      this.Hide();
    }
  }
}
