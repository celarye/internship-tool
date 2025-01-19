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
  /// Interaction logic for InternshipDetailsWindow.xaml
  /// </summary>
  public partial class InternshipDetailsWindow : Window
  {
    private Internship _selectedInternship;

    public InternshipDetailsWindow(Internship selectedInternship)
    {
      InitializeComponent();

      _selectedInternship = selectedInternship;

      TitleLabel.Content += _selectedInternship.Title;
      DescriptionTextBox.Text += _selectedInternship.Description;
      CompanyLabel.Content += MainWindow._logicProvidor.GetCompany((int)_selectedInternship.CompanyId)!.Name;
      ApprovedLabel.Content += _selectedInternship.Approved.ToString();
      MentorLabel.Content += _selectedInternship.MentorId.HasValue ? MainWindow._logicProvidor.GetUser((int)_selectedInternship.MentorId)!.Email : "N/A";
      StudentLabel.Content += _selectedInternship.StudentId.HasValue ? MainWindow._logicProvidor.GetUser((int)_selectedInternship.StudentId)!.Email : "N/A";
      TeacherLabel.Content += _selectedInternship.TeacherId.HasValue ? MainWindow._logicProvidor.GetUser((int)_selectedInternship.TeacherId)!.Email : "N/A";

      if (!_selectedInternship.StudentId.HasValue)
      {
        CandidatesTextBox.Text +=  string.Join(", ", MainWindow._logicProvidor.GetInternshipCandidates(_selectedInternship.Id).Select(candidate => MainWindow._logicProvidor.GetUser(candidate.StudentId)!.Email));

        if (MainWindow._logicProvidor.GetLoggedInUser()!.UserType == UserTypes.Student)
        {
          CandidatesLabel.Visibility = Visibility.Hidden;
          CandidatesTextBox.Visibility = Visibility.Hidden;
          CandidateEmailLabel.Visibility = Visibility.Hidden;
          CandidateEmailTextBox.Visibility = Visibility.Hidden;
          DownloadCVButton.Visibility = Visibility.Hidden;
        }
      }
      else
      {

        CandidatesLabel.Visibility = Visibility.Hidden;
        CandidatesTextBox.Visibility = Visibility.Hidden;
        CandidateEmailLabel.Visibility = Visibility.Hidden;
        CandidateEmailTextBox.Visibility = Visibility.Hidden;
        DownloadCVButton.Visibility = Visibility.Hidden;
        OpenReviewButton.Visibility = Visibility.Visible;
      }
    }

    private void DownloadCV(object sender, RoutedEventArgs e)
    {
      try
      {
        string candidateEmail = CandidateEmailTextBox.Text;

        if (string.IsNullOrWhiteSpace(candidateEmail))
        {
          FeedbackLabel.Content = "Candidate email can't be empty";
          return;
        }

        User? user = MainWindow._logicProvidor.GetUserByEmail(candidateEmail);

        if (user == null)
        {
          FeedbackLabel.Content = "Candidate not found";
          return;
        }

        Student student = (Student)user;

        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
        {
          FileName = System.IO.Path.GetFileName(student.CVPath),
          Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
          Title = "Save Candidate's CV"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
          System.IO.File.Copy(student.CVPath!, saveFileDialog.FileName!, overwrite: true);
          FeedbackLabel.Content = "CV downloaded successfully";
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        FeedbackLabel.Content = "Access denied";
      }
    }

    private void OpenReview(object sender, RoutedEventArgs e)
    {
      InternshipEvaluationWindow reviewWindow = new InternshipEvaluationWindow(_selectedInternship);
      reviewWindow.Show();
    }
  }
}
