using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guesser
{
  class Program
  {

    static void Main(string[] args)
    {
      StatsHelper.InitialiseFromFile();


      Console.WriteLine("Please pick one of the following animals: {0}", String.Concat(StatsHelper.Everything.Select(n => n.Name + ", ")));

      Console.WriteLine();

      while (true)
      {
        Question questionToAsk = StatsHelper.GetBestQuestionToAsk(0, StatsHelper.Knowledge);
        Console.WriteLine(questionToAsk.AsString);
        bool isYes = Console.ReadLine() == "y";
        StatsHelper.Knowledge.Add(questionToAsk, isYes);

        //StatsHelper.Everything = StatsHelper.Everything.OrderByDescending(n => StatsHelper.Likelyhood(n, StatsHelper.Knowledge)).ToList();
        //StatsHelper.Everything.DoToEach(n => Console.WriteLine("{0}: {1}, {2}", n.Name, StatsHelper.Likelyhood(n, StatsHelper.Knowledge), StatsHelper.Likelyhood(n, StatsHelper.Knowledge, false) * StatsHelper.Everything.Sum(t => t.TotalCounter)));
        Console.WriteLine();

        if (StatsHelper.Likelyhood(StatsHelper.Knowledge) > 0.90 || StatsHelper.Knowledge.Count == StatsHelper.Questions.Count)
        {
          Console.WriteLine(StatsHelper.Everything.GetMax(n => StatsHelper.Likelyhood(n, StatsHelper.Knowledge)).Name + "! I took " + StatsHelper.Knowledge.Count + " guesses");
          StatsHelper.Knowledge = new Dictionary<Question, bool>();
          Console.ReadLine();
        }
      }
    }
  }

  public static class StatsHelper
  {
    public static void InitialiseBasicData()
    {
      Thing dog = new Thing() { Name = "Dog", TotalCounter = 5833 };
      Thing fish = new Thing() { Name = "Fish", TotalCounter = 5833 };
      Thing cat = new Thing() { Name = "Cat", TotalCounter = 5833 };
      Thing mammoth = new Thing() { Name = "Wooly Mammoth", TotalCounter = 5833 };
      Thing human = new Thing() { Name = "Human", TotalCounter = 5833 };
      Thing mouse = new Thing() { Name = "Mouse", TotalCounter = 5833 };
      Thing crab = new Thing() { Name = "Crab", TotalCounter = 5833 };
      Thing snail = new Thing() { Name = "Snail", TotalCounter = 5833 };
      Thing parrot = new Thing() { Name = "Parrot", TotalCounter = 5833 };


      Question mammal = new Question() { AsString = "Is it a mammal?", TotalCounter = 300, ID = 0 };
      Question swim = new Question() { AsString = "Does it swim?", TotalCounter = 300, ID = 1 };
      Question nose = new Question() { AsString = "Does it have a nose?", TotalCounter = 300, ID = 2 };
      Question extinct = new Question() { AsString = "Is it extinct?", TotalCounter = 300, ID = 3 };
      Question talk = new Question() { AsString = "Can it talk like a human?", TotalCounter = 300, ID = 4 };
      Question speed = new Question() { AsString = "Can it go faster than a walking human?", TotalCounter = 300, ID = 5 };
      Question sideways = new Question() { AsString = "Does it walk sideways?", TotalCounter = 300, ID = 6 };
      Question fly = new Question() { AsString = "Can it fly?", TotalCounter = 300, ID = 7 };
      Question smaller = new Question() { AsString = "Is it smaller than your hand?", TotalCounter = 300, ID = 8 };
      Question shell = new Question() { AsString = "Does it have a hard shell?", TotalCounter = 300, ID = 9 };
      Question pet = new Question() { AsString = "Would someone have one as a pet?", TotalCounter = 300, ID = 10 };
      Question balance = new Question() { AsString = "Is it very good at balancing on things?", TotalCounter = 300, ID = 11 };

      dog.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(90, 100) },
        { mammal, new Tuple<int, int>(99, 100) }, 
        { nose, new Tuple<int, int>(99, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(3, 100) }, 
        { speed, new Tuple<int, int>(95, 100) }, 
        { sideways, new Tuple<int, int>(5, 100) }, 
        { fly, new Tuple<int, int>(1, 100) }, 
        { smaller, new Tuple<int, int>(1, 100) }, 
        { shell, new Tuple<int, int>(1, 100) }, 
        { pet, new Tuple<int, int>(93, 100) }, 
        { balance, new Tuple<int, int>(42, 100) } 
      };
      fish.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(99, 100) },
        { mammal, new Tuple<int, int>(20, 100) }, 
        { nose, new Tuple<int, int>(30, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(5, 100) }, 
        { speed, new Tuple<int, int>(70, 100) }, 
        { sideways, new Tuple<int, int>(1, 100) }, 
        { fly, new Tuple<int, int>(4, 100) }, 
        { smaller, new Tuple<int, int>(50, 100) }, 
        { shell, new Tuple<int, int>(10, 100) }, 
        { pet, new Tuple<int, int>(70, 100) }, 
        { balance, new Tuple<int, int>(1, 100) } 
      };
      cat.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(9, 100) },
        { mammal, new Tuple<int, int>(99, 100) }, 
        { nose, new Tuple<int, int>(99, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(2, 100) }, 
        { speed, new Tuple<int, int>(95, 100) }, 
        { sideways, new Tuple<int, int>(7, 100) }, 
        { fly, new Tuple<int, int>(1, 100) }, 
        { smaller, new Tuple<int, int>(3, 100) }, 
        { shell, new Tuple<int, int>(1, 100) }, 
        { pet, new Tuple<int, int>(98, 100) }, 
        { balance, new Tuple<int, int>(97, 100) } 
      };
      mammoth.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(50, 100) },
        { mammal, new Tuple<int, int>(95, 100) }, 
        { nose, new Tuple<int, int>(98, 100) },
        { extinct, new Tuple<int, int>(99, 100) }, 
        { talk, new Tuple<int, int>(2, 100) }, 
        { speed, new Tuple<int, int>(95, 100) }, 
        { sideways, new Tuple<int, int>(4, 100) }, 
        { fly, new Tuple<int, int>(2, 100) }, 
        { smaller, new Tuple<int, int>(1, 100) }, 
        { shell, new Tuple<int, int>(3, 100) }, 
        { pet, new Tuple<int, int>(1, 100) }, 
        { balance, new Tuple<int, int>(2, 100) } 
      };
      human.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(99, 100) },
        { mammal, new Tuple<int, int>(97, 100) }, 
        { nose, new Tuple<int, int>(99, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(99, 100) }, 
        { speed, new Tuple<int, int>(95, 100) }, 
        { sideways, new Tuple<int, int>(20, 100) }, 
        { fly, new Tuple<int, int>(10, 100) }, 
        { smaller, new Tuple<int, int>(1, 100) }, 
        { shell, new Tuple<int, int>(2, 100) }, 
        { pet, new Tuple<int, int>(3, 100) }, 
        { balance, new Tuple<int, int>(40, 100) } 
      };
      mouse.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(50, 100) },
        { mammal, new Tuple<int, int>(95, 100) }, 
        { nose, new Tuple<int, int>(99, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(1, 100) }, 
        { speed, new Tuple<int, int>(70, 100) }, 
        { sideways, new Tuple<int, int>(5, 100) }, 
        { fly, new Tuple<int, int>(2, 100) }, 
        { smaller, new Tuple<int, int>(93, 100) }, 
        { shell, new Tuple<int, int>(1, 100) }, 
        { pet, new Tuple<int, int>(94, 100) }, 
        { balance, new Tuple<int, int>(85, 100) } 
      };
      parrot.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(3, 100) },
        { mammal, new Tuple<int, int>(4, 100) }, 
        { nose, new Tuple<int, int>(30, 100) },
        { extinct, new Tuple<int, int>(2, 100) }, 
        { talk, new Tuple<int, int>(93, 100) }, 
        { speed, new Tuple<int, int>(97, 100) }, 
        { sideways, new Tuple<int, int>(3, 100) }, 
        { fly, new Tuple<int, int>(99, 100) }, 
        { smaller, new Tuple<int, int>(5, 100) }, 
        { shell, new Tuple<int, int>(3, 100) }, 
        { pet, new Tuple<int, int>(80, 100) }, 
        { balance, new Tuple<int, int>(85, 100) } 
      };
      crab.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(70, 100) },
        { mammal, new Tuple<int, int>(4, 100) }, 
        { nose, new Tuple<int, int>(4, 100) },
        { extinct, new Tuple<int, int>(2, 100) }, 
        { talk, new Tuple<int, int>(1, 100) }, 
        { speed, new Tuple<int, int>(60, 100) }, 
        { sideways, new Tuple<int, int>(99, 100) }, 
        { fly, new Tuple<int, int>(1, 100) }, 
        { smaller, new Tuple<int, int>(80, 100) }, 
        { shell, new Tuple<int, int>(99, 100) }, 
        { pet, new Tuple<int, int>(10, 100) }, 
        { balance, new Tuple<int, int>(10, 100) }
      };
      snail.YesCounters = new Dictionary<Question, Tuple<int, int>>()
      {
        { swim, new Tuple<int, int>(2, 100) },
        { mammal, new Tuple<int, int>(1, 100) }, 
        { nose, new Tuple<int, int>(4, 100) },
        { extinct, new Tuple<int, int>(1, 100) }, 
        { talk, new Tuple<int, int>(1, 100) }, 
        { speed, new Tuple<int, int>(1, 100) }, 
        { sideways, new Tuple<int, int>(3, 100) }, 
        { fly, new Tuple<int, int>(1, 100) }, 
        { smaller, new Tuple<int, int>(99, 100) }, 
        { shell, new Tuple<int, int>(97, 100) }, 
        { pet, new Tuple<int, int>(7, 100) }, 
        { balance, new Tuple<int, int>(70, 100) } 
      };

      Everything.Add(dog);
      Everything.Add(fish);
      Everything.Add(cat);
      Everything.Add(mammoth);
      Everything.Add(human);
      Everything.Add(snail);
      Everything.Add(crab);
      Everything.Add(parrot);
      Everything.Add(mouse);

      Questions.Add(swim);
      Questions.Add(mammal);
      Questions.Add(nose);
      Questions.Add(extinct);
      Questions.Add(talk);
      Questions.Add(speed);
      Questions.Add(sideways);
      Questions.Add(fly);
      Questions.Add(smaller);
      Questions.Add(shell);
      Questions.Add(pet);
      Questions.Add(balance);
    }

    public static void InitialiseFromFile()
    {
      Everything = new List<Thing>();
      EverythingIndexedByName = new Dictionary<string, Thing>();
      Questions = new List<Question>();
      QuestionsIndexedByID = new Dictionary<int, Question>();

      using (StreamReader reader = new StreamReader(@"C:\Users\George Powell\Documents\Visual Studio 2013\Projects\Guesser\Guesser\bin\Debug\data.txt"))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          string[] parts = line.Split(',');
          switch (parts[0])
          {
            case "T":
              Thing newThing = new Thing() { Name = parts[1], TotalCounter = Int32.Parse(parts[2]) };
              Everything.Add(newThing);
              EverythingIndexedByName.Add(newThing.Name, newThing);
              break;
            case "Q":
              Question newQuestion = new Question() { AsString = parts[1], ID = Int32.Parse(parts[2]) };
              Questions.Add(newQuestion);
              QuestionsIndexedByID.Add(newQuestion.ID, newQuestion);
              break;
            case "A":
              Thing thing = EverythingIndexedByName[parts[1]];
              Question question = QuestionsIndexedByID[Int32.Parse(parts[2])];
              thing.YesCounters.Add(question, new Tuple<int, int>(Int32.Parse(parts[3]), Int32.Parse(parts[4])));
              break;
          }
        }
      }
    }

    public static void SaveToFile()
    {
      StreamWriter writer = new StreamWriter(@"C:\Users\George Powell\Documents\Visual Studio 2013\Projects\Guesser\Guesser\bin\Debug\data.txt");
      foreach (Thing thing in Everything)
        writer.WriteLine("T,{0},{1}", thing.Name, thing.YesCounters.Sum(n => n.Value.Item2));
      foreach (Question question in Questions)
        writer.WriteLine("Q,{0},{1}", question.AsString, question.ID);
      foreach (Thing thing in Everything)
        foreach (Question question in thing.YesCounters.Keys)
          writer.WriteLine("A,{0},{1},{2},{3},{4}", thing.Name, question.ID, thing.YesCounters[question].Item1, thing.YesCounters[question].Item2, question.AsString);

      writer.Flush();
      writer.Close();
      writer.Dispose();
    }

    public static List<Thing> Everything = new List<Thing>();
    public static Dictionary<string, Thing> EverythingIndexedByName = new Dictionary<string, Thing>();

    public static List<Question> Questions = new List<Question>();
    public static Dictionary<int, Question> QuestionsIndexedByID = new Dictionary<int, Question>();

    public static Dictionary<Question, bool> Knowledge = new Dictionary<Question, bool>();

    public static Question GetBestQuestionToAsk(int depth, Dictionary<Question, bool> Knowledge)
    {
      //return Questions.GetConditionedMax(n => ExpectedProbabilityAfterAsking(n, Knowledge), n => !Knowledge.ContainsKey(n));
      double maxSizeSoFar = Int32.MinValue;
      Question maxValue = null;

      //Console.WriteLine(Likelyhood(Knowledge));

      foreach (Question question in Questions)
      {
        if (Knowledge.ContainsKey(question))
          continue;

        //double size = ExpectedProbabilityAfterAsking(question, Knowledge);
        double size = Math.Abs(0.5 - Everything.Sum(n => n.YesProbabilityGivenThis(question)));

        //Console.WriteLine("{0}:    {1}", size, question.AsString);
        if (size > maxSizeSoFar)
        {
          maxSizeSoFar = size;
          maxValue = question;
        }
      }

      return maxValue;
    }

    public static double ExpectedProbabilityAfterAsking(Question question, Dictionary<Question, bool> Knowledge, int depth = 0)
    {
      double probabilityNow = Likelyhood(Knowledge);

      double probabilityOfYes = Everything.Sum(thing => YesProb(thing, question, Knowledge));
      double probabilityOfNo = Everything.Sum(thing => NoProb(thing, question, Knowledge));

      double y = Likelyhood(CloneAdd(Knowledge, question, true));
      double n = Likelyhood(CloneAdd(Knowledge, question, false));

      double a = probabilityOfYes * y;
      double b = probabilityOfNo * n;

      double expectedOutcome = a + b;

      return expectedOutcome;
    }

    public static double RecursiveLikelyhood(int depth, Thing thing, Dictionary<Question, bool> Knowledge)
    {
      if (true)
        return Likelyhood(thing, Knowledge);
      else
      {
        foreach (Question q in Questions)
        {
          double likelyhoodAfterYes = Everything.Max(n => RecursiveLikelyhood(depth - 1, n, CloneAdd(Knowledge, q, true)));
          double likelyhoodAfterNo = Everything.Max(n => RecursiveLikelyhood(depth - 1, n, CloneAdd(Knowledge, q, false)));
        }
      }
    }

    public static double Likelyhood(Dictionary<Question, bool> Knowledge)
    {
      return Everything.Max(n => Likelyhood(n, Knowledge));
    }

    public static double Likelyhood(Thing thing, Dictionary<Question, bool> Knowledge, bool divide = true)
    {
      double probability = (1.0 * thing.TotalCounter) / Everything.Sum(n => n.TotalCounter);
      foreach (Question q in Knowledge.Keys)
      {
        probability *= Knowledge[q] ? thing.YesProbabilityGivenThis(q) : thing.NoProbabilityGivenThis(q);
      }

      double sum = 1.0;

      if (divide)
      {
        sum = 0.0;
        foreach (Thing innerThing in Everything)
          sum += Likelyhood(innerThing, Knowledge, false);
      }

      return probability / sum;
    }

    public static double YesProb(Thing thing, Question question, Dictionary<Question, bool> knowledge)
    {
      return Likelyhood(thing, Knowledge) * thing.YesProbabilityGivenThis(question);
    }

    public static double NoProb(Thing thing, Question question, Dictionary<Question, bool> knowledge)
    {
      return Likelyhood(thing, Knowledge) * thing.NoProbabilityGivenThis(question);
    }

    public static Dictionary<Question, bool> CloneAdd(Dictionary<Question, bool> dict, Question q, bool b)
    {
      Dictionary<Question, bool> rtn = new Dictionary<Question, bool>();
      foreach (Question question in dict.Keys)
        rtn.Add(question, dict[question]);
      rtn.Add(q, b);
      return rtn;
    }

    static Random r = new Random();

    public static void AddQuestion(string question)
    {
      int id = r.Next();
      Question newQuestion = new Question() { AsString = question, ID = id };
      Questions.Add(newQuestion);
      QuestionsIndexedByID.Add(id, newQuestion);

      SaveToFile();
    }

    public static void AddThing(string thing)
    {
      Thing newThing = new Thing() { Name = thing, TotalCounter = 0 };
      Everything.Add(newThing);
      EverythingIndexedByName.Add(thing, newThing);

      SaveToFile();
    }
  }

  public class Thing
  {
    public string Name { get; set; }
    public int TotalCounter { get; set; }
    public Dictionary<Question, Tuple<int, int>> YesCounters { get; set; }

    public Thing()
    {
      YesCounters = new Dictionary<Question, Tuple<int, int>>();
    }

    public int GetYesCount(Question question)
    {
      return (YesCounters.ContainsKey(question) ? YesCounters[question].Item1 : 0);
    }

    public int GetNoCount(Question question)
    {
      return (YesCounters.ContainsKey(question) ? YesCounters[question].Item2 - YesCounters[question].Item1 : 0);
    }

    public int GetTotalCount(Question question)
    {
      return (YesCounters.ContainsKey(question) ? YesCounters[question].Item2 : 0);
    }

    public double YesProbabilityGivenThis(Question q)
    {
      return YesCounters.ContainsKey(q) ? (1.0 * YesCounters[q].Item1) / YesCounters[q].Item2 : 0.5;
    }

    public double NoProbabilityGivenThis(Question q)
    {
      return YesCounters.ContainsKey(q) ? 1.0 - ((1.0 * YesCounters[q].Item1) / YesCounters[q].Item2) : 0.5;
    }
  }

  public class Question
  {
    public int ID { get; set; }
    public string AsString { get; set; }
    public int TotalCounter { get; set; }
  }

  public static class EnumerableExtensions
  {
    public static void DoToEach<T>(this IEnumerable<T> collection, Action<T> function)
    {
      foreach (T value in collection)
        function.Invoke(value);
    }

    public static T GetMax<T>(this IEnumerable<T> collection, Func<T, double> map)
    {
      double maxSizeSoFar = Int32.MinValue;
      T maxValue = default(T);

      foreach (T value in collection)
      {
        double size = map.Invoke(value);
        if (size > maxSizeSoFar)
        {
          maxSizeSoFar = size;
          maxValue = value;
        }
      }

      return maxValue;
    }

    public static T GetConditionedMax<T>(this IEnumerable<T> collection, Func<T, double> map, Func<T, bool> condition)
    {
      double maxSizeSoFar = Int32.MinValue;
      T maxValue = default(T);

      foreach (T value in collection)
      {
        if (!condition.Invoke(value))
          continue;

        double size = map.Invoke(value);
        if (size > maxSizeSoFar)
        {
          maxSizeSoFar = size;
          maxValue = value;
        }
      }

      return maxValue;
    }
  }
}
