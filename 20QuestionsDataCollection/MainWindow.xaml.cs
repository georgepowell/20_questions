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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Guesser;

namespace _20QuestionsDataCollection
{
  public partial class MainWindow : Window
  {
    public Thing currentThing;
    public Question currentQuestion;

    public MainWindow()
    {
      InitializeComponent();

      StatsHelper.InitialiseFromFile();

      NextQuestion();
    }

    void NextQuestion()
    {
      currentThing = null;
      currentQuestion = null;

      foreach (Thing thing in StatsHelper.Everything)
        foreach (Question question in StatsHelper.Questions)
          if (!thing.YesCounters.ContainsKey(question))
          {
            currentThing = thing;
            currentQuestion = question;
          }

      if (currentQuestion != null && currentThing != null)
      {
        txtThing.Text = currentThing.Name;
        txtQuestion.Text = currentQuestion.AsString;
      }
      else
      {
        txtThing.Text = "Finished!";
        txtQuestion.Text = "Congratulations, all Questions have been answered for all Things!";
      }
    }

    void AddQuestion(string question)
    {
      StatsHelper.AddQuestion(question);
    }

    void AddThing(string thing)
    {
      StatsHelper.AddThing(thing);
    }

    private void Percentage_Click(object sender, RoutedEventArgs e)
    {
      int percentage = Int32.Parse((string)((Button)sender).Tag);
      if (currentThing != null && !currentThing.YesCounters.ContainsKey(currentQuestion))
        currentThing.YesCounters.Add(currentQuestion, new Tuple<int, int>(percentage, 100));
      StatsHelper.SaveToFile();
      NextQuestion();
    }

    private void btnNewThing_Click(object sender, RoutedEventArgs e)
    {
      AddThing(txtNewThing.Text);
      txtNewThing.Text = "";
      txtNewThing.Focus();

      if (currentThing == null || currentQuestion == null)
        NextQuestion();
    }

    private void btnNewQuestion_Click(object sender, RoutedEventArgs e)
    {
      AddQuestion(txtNewQuestion.Text);
      txtNewQuestion.Text = "";
      txtNewQuestion.Focus();

      if (currentThing == null || currentQuestion == null)
        NextQuestion();
    }

    private void txtNewThing_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
        btnNewThing_Click(null, null);
    }

    private void txtNewQuestion_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
        btnNewQuestion_Click(null, null);
    }
  }
}
