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
  /// Interaction logic for InternshipReview.xaml
  /// </summary>
  public partial class InternshipEvaluationWindow : Window
  {
    private readonly InternshipEvaluation _internshipEvaluation;
    public InternshipEvaluationWindow(Internship internship)
    {
      InitializeComponent();

      User loggedInUser = MainWindow._logicProvidor.GetLoggedInUser()!;

      switch (loggedInUser.UserType)
      {
        case UserTypes.Admin:
          break;
        case UserTypes.Student:
          MentorEvaluation1TextBox.IsReadOnly = true;
          MentorEvaluation2TextBox.IsReadOnly = true;
          TeacherEvaluationTextBox.IsReadOnly = true;
          NotesTextBox.IsReadOnly = true;

          UpdateInernshipEvaluation.Visibility = Visibility.Hidden;
          break;
        case UserTypes.Teacher:
          if (loggedInUser.Id != internship.TeacherId)
          {
            UpdateInernshipEvaluation.Visibility = Visibility.Hidden;
            TeacherEvaluationTextBox.IsReadOnly = true;
            NotesTextBox.IsReadOnly = true;
          }

          MentorEvaluation1TextBox.IsReadOnly = true;
          MentorEvaluation2TextBox.IsReadOnly = true;
          break;
        case UserTypes.CompanyEmployee:
          if (loggedInUser.Id != internship.MentorId)
          {
            UpdateInernshipEvaluation.Visibility = Visibility.Hidden;
            MentorEvaluation1TextBox.IsReadOnly = true;
            MentorEvaluation2TextBox.IsReadOnly = true;
            NotesTextBox.IsReadOnly = true;
          }

          TeacherEvaluationTextBox.IsReadOnly = true;
          break;
      }

      _internshipEvaluation = MainWindow._logicProvidor.GetInternshipEvaluation(internship.Id)!;

      MentorEvaluation1TextBox.Text = _internshipEvaluation.MentorEvaluation1.HasValue ? _internshipEvaluation.MentorEvaluation1.ToString() : "";
      MentorEvaluation2TextBox.Text = _internshipEvaluation.MentorEvaluation2.HasValue ? _internshipEvaluation.MentorEvaluation2.ToString() : "";
      TeacherEvaluationTextBox.Text = _internshipEvaluation.TeacherEvaluation.HasValue ? _internshipEvaluation.TeacherEvaluation.ToString() : "";
      OverallScoreTextBox.Text = _internshipEvaluation.OveralScore.HasValue ? _internshipEvaluation.OveralScore.ToString() : "N/A";
      NotesTextBox.Text = _internshipEvaluation.Note;
    }

    private void UpdateInternshipEvaluation(object sender, RoutedEventArgs e)
    {
      InternshipEvaluations? mentorEvaluation1 = null;
      InternshipEvaluations? mentorEvaluation2 = null;
      bool isValid = true;

      if (!string.IsNullOrEmpty(MentorEvaluation2TextBox.Text))
      {
        if (string.IsNullOrEmpty(MentorEvaluation1TextBox.Text))
        {
          FeedbackLabel.Content = "Mentor Evaluation 1 is required when Mentor Evaluation 2 is provided.";
          isValid = false;
        }
        else if (!Enum.TryParse(MentorEvaluation1TextBox.Text, out InternshipEvaluations parsedMentorEvaluation1))
        {
          FeedbackLabel.Content = "Invalid value for Mentor Evaluation 1 (A-E).";
          isValid = false;
        }
        else
        {
          mentorEvaluation1 = parsedMentorEvaluation1;
        }
      }

      if (!string.IsNullOrEmpty(MentorEvaluation2TextBox.Text))
      {
        if (!Enum.TryParse(MentorEvaluation2TextBox.Text, out InternshipEvaluations parsedMentorEvaluation2))
        {
          FeedbackLabel.Content = "Invalid value for Mentor Evaluation 2 (A-E).";
          isValid = false;
        }
        else
        {
          mentorEvaluation2 = parsedMentorEvaluation2;
        }
      }

      InternshipEvaluations? teacherEvaluation = null;
      if (!string.IsNullOrEmpty(TeacherEvaluationTextBox.Text))
      {
        if (!Enum.TryParse(TeacherEvaluationTextBox.Text, out InternshipEvaluations parsedTeacherEvaluation))
        {
          FeedbackLabel.Content = "Invalid value for Teacher Evaluation (A-E).";
          isValid = false;
        }
        else
        {
          teacherEvaluation = parsedTeacherEvaluation;
        }
      }

      if (!isValid)
      {
        return;
      }

      string note = NotesTextBox.Text;

      // Update internship evaluation only if all values are valid
      MainWindow._logicProvidor.UpdateInternshipEvaluation(
          _internshipEvaluation, mentorEvaluation1, mentorEvaluation2, teacherEvaluation, note
      );

      // Refetch the updated evaluation (handling possible null value)
      var updatedEvaluation = MainWindow._logicProvidor.GetInternshipEvaluation(_internshipEvaluation.InternshipId);

      if (updatedEvaluation == null)
      {
        FeedbackLabel.Content = "Error retrieving updated evaluation.";
        return;
      }

      // Update the overall score text box
      OverallScoreTextBox.Text = updatedEvaluation!.OveralScore.ToString();

      FeedbackLabel.Content = "Successfully updated the internship evaluation.";
    }
  }
}
