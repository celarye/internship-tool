using System;
using System.Collections.Generic;
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

namespace PresentationGUI
{
  /// <summary>
  /// Interaction logic for Internships.xaml
  /// </summary>
  public partial class InternshipsWindow : Window
  {
    public InternshipsWindow()
    {
      InitializeComponent();

      User loggedInUser = MainWindow._logicProvidor.GetLoggedInUser()!;

      switch (loggedInUser.UserType)
      {
        case UserTypes.Admin:
          RightListBox.ItemsSource = MainWindow._logicProvidor.GetAdminInternships(true);
          LeftListBox.ItemsSource = MainWindow._logicProvidor.GetAdminInternships(false);

          ApproveInternshipButton.Visibility = Visibility.Visible;
          AssignInternshipButton.Visibility = Visibility.Visible;
          MentorEmailLabel.Visibility = Visibility.Visible;
          MentorEmailTextBox.Visibility = Visibility.Visible;
          StudentEmailLabel.Visibility = Visibility.Visible;
          StudentEmailTextBox.Visibility = Visibility.Visible;
          TeacherEmailLabel.Visibility = Visibility.Visible;
          TeacherEmailTextBox.Visibility = Visibility.Visible;
          break;
        case UserTypes.Student:
          LeftListLabel.Text = "Unassigned Internships";
          RightListLabel.Text = "Assigned Internships";

          RightListBox.ItemsSource = MainWindow._logicProvidor.GetStudentInternship();
          LeftListBox.ItemsSource = MainWindow._logicProvidor.GetStudentAvailableInternships();

          CandidateButton.Visibility = Visibility.Visible;
          break;
        case UserTypes.Teacher:
          RightListBox.ItemsSource = MainWindow._logicProvidor.GetAdminInternships(true);
          LeftListBox.ItemsSource = MainWindow._logicProvidor.GetAdminInternships(false);
          break;
        case UserTypes.CompanyEmployee:
          RightListBox.ItemsSource = MainWindow._logicProvidor.GetCompanyInternships(true);
          LeftListBox.ItemsSource = MainWindow._logicProvidor.GetCompanyInternships(false);

          if (loggedInUser.Id == MainWindow._logicProvidor.GetLoggedInCompany()!.CompanyManager)
          {
            AssignInternshipButton.Visibility = Visibility.Visible;
            MentorEmailLabel.Visibility = Visibility.Visible;
            MentorEmailTextBox.Visibility = Visibility.Visible;
            StudentEmailLabel.Visibility = Visibility.Visible;
            StudentEmailTextBox.Visibility = Visibility.Visible;
            TeacherEmailLabel.Visibility = Visibility.Visible;
            TeacherEmailTextBox.Visibility = Visibility.Visible;
          }

          break;
      }
    }

    private void InternshipDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var listBox = sender as ListBox;
      var selectedInternship = listBox?.SelectedItem as Internship;

      if (selectedInternship != null)
      {
        var internshipDetailsWindow = new InternshipDetailsWindow(selectedInternship);
        internshipDetailsWindow.Show();
      }
    }

    private void ApproveInternship(object sender, RoutedEventArgs e)
    {
      Internship? selectedInternship = LeftListBox.SelectedItem as Internship;

      if (selectedInternship == null)
        return;


      MainWindow._logicProvidor.ApproveInternship(selectedInternship);

      var approvedInternships = MainWindow._logicProvidor.GetAdminInternships(true);
      var unapprovedInternships = MainWindow._logicProvidor.GetAdminInternships(false);

      RightListBox.ItemsSource = approvedInternships;
      LeftListBox.ItemsSource = unapprovedInternships;
    }

    private void CandidateForInternship(object sender, RoutedEventArgs e)
    {
      Internship? selectedInternship = LeftListBox.SelectedItem as Internship;

      if (selectedInternship == null)
        return;

      try
      {
        List<InternshipCandidate> internshipCandidates = MainWindow._logicProvidor.GetInternshipCandidates(selectedInternship.Id);

        if (!internshipCandidates.Any(candidate => candidate.StudentId == MainWindow._logicProvidor.GetLoggedInUser()?.Id))
        {
          MainWindow._logicProvidor.AddInternshipCandidate(selectedInternship.Id);
          FeedbackLabel.Content = "Succesfully placed as candidate";
        }
        else
        {
          FeedbackLabel.Content = "You already are a candidate for this internship";
        }
      }
      catch (UnauthorizedAccessException err)
      {
        FeedbackLabel.Content = err.Message;
      }
    }

    private void AssignInternship(object sender, RoutedEventArgs e)
    {
      string mentorEmail = MentorEmailTextBox.Text;
      string studentEmail = StudentEmailTextBox.Text;
      string teacherEmail = TeacherEmailTextBox.Text;

      Internship? selectedInternship = RightListBox.SelectedItem as Internship;

      if (selectedInternship == null)
      {
        FeedbackLabel.Content = "Select an approved internship";
        return;
      }

      try
      {
        MainWindow._logicProvidor.AssignInternship(selectedInternship, mentorEmail, studentEmail, teacherEmail);
        FeedbackLabel.Content = "Internship assigned successfully";
    }
      catch (UnauthorizedAccessException err)
      {
        FeedbackLabel.Content = err.Message;
      }
      catch (ArgumentException err)
      {
        FeedbackLabel.Content = err.Message;
      }
    }
  }
}
