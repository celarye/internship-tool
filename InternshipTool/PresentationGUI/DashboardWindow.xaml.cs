using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Globals;
using Globals.Enums;
using Microsoft.Win32;

namespace PresentationGUI
{
  /// <summary>
  /// Interaction logic for CompanyEmployee.xaml
  /// </summary>
  public partial class DashBoardWindow : Window
  {
    private InternshipsWindow _internshipsWindow;
    public DashBoardWindow()
    {
      InitializeComponent();

      User loggedInUser = MainWindow._logicProvidor.GetLoggedInUser()!;

      switch (loggedInUser.UserType)
      {
        case UserTypes.Admin:
          break;
        case UserTypes.Student:
          AddCVButton.Visibility = Visibility.Visible;
          break;
        case UserTypes.Teacher:
          break;
        case UserTypes.CompanyEmployee:
          if (loggedInUser.Id != MainWindow._logicProvidor.GetLoggedInCompany()!.CompanyManager)
            break;

          TitleLabel.Visibility = Visibility.Visible;
          TitleTextBox.Visibility = Visibility.Visible;
          DescriptionLabel.Visibility = Visibility.Visible;
          DescriptionTextBox.Visibility = Visibility.Visible; 
          AddInternshipButton.Visibility = Visibility.Visible;
          EmailLabel.Visibility = Visibility.Visible;
          EmailTextBox.Visibility = Visibility.Visible;
          PasswordLabel.Visibility = Visibility.Visible;
          PasswordTextBox.Visibility = Visibility.Visible;
          FirstNameLabel.Visibility = Visibility.Visible;
          FirstNameTextBox.Visibility = Visibility.Visible;
          SecondNameLabel.Visibility = Visibility.Visible;
          SecondNameTextBox.Visibility = Visibility.Visible;
          AddCompanyEmployeeButton.Visibility = Visibility.Visible;
          break;
      }

      this.Closed += (s, e) => Application.Current.Shutdown();
    }

    private void Internships(object sender, RoutedEventArgs e)
    {
      if (_internshipsWindow != null)
        if (_internshipsWindow.IsVisible)
          return;

      _internshipsWindow = new InternshipsWindow();
      _internshipsWindow.Show();
    }

    private void AddCV(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();

      openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

      if (openFileDialog.ShowDialog() == true)
      {
        string selectedFilePath = openFileDialog.FileName;
        try
        {
          MainWindow._logicProvidor.UpdateUser(MainWindow._logicProvidor.GetLoggedInUser()!, null, null, null, null, null, selectedFilePath);
          FeedbackLabel.Content = "Succesfully saved the CV";
        }
        catch (IOException err)
        {
          System.Diagnostics.Debug.WriteLine(err);
        }
        catch (UnauthorizedAccessException err)
        {
          System.Diagnostics.Debug.WriteLine(err);
        }
      }
    }


    private void AddInternship(object sender, RoutedEventArgs e)
    {
      string title = TitleTextBox.Text;
      string description = DescriptionTextBox.Text;

      if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
      {
        FeedbackLabel.Content = "Title, description can't be empty";
        return;
      }

      MainWindow._logicProvidor.AddInternship(title, description);
      FeedbackLabel.Content = "Internship added successfully";
    }

    private void AddCompanyEmployee(object sender, RoutedEventArgs e)
    {
      string email = EmailTextBox.Text;
      string password = PasswordTextBox.Password;
      string firstName = FirstNameTextBox.Text;
      string secondName = SecondNameTextBox.Text;

      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName))
      {
        FeedbackLabel.Content = "Email, password, first name, second name can't be empty";
        return;
      }

      try
      {
        MainWindow._logicProvidor.RegisterUser(email, password, firstName, secondName, UserTypes.CompanyEmployee);
        FeedbackLabel.Content = "Company employee added succesfully";
      }
      catch (ArgumentException err)
      {
        FeedbackLabel.Content = err.Message;
      }
      catch (UnauthorizedAccessException err)
      {
        FeedbackLabel.Content = err.Message;
      }
    }
  }
}
