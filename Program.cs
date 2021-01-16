using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Unreal;

namespace Unreal
{
    public interface IPlayer
    {
        void GetPoint();
        void Hited(int value);
    }
    public static class Unreal
    {
        public class Player : IPlayer
        {
            public string name;
            public List<int> numbers;
            public List<int> hit;
            public int points;

            public void GetPoint()
            {
                points++;
            }

            public void Hited(int value)
            {
                hit.Add(value);
            }
        }

        public static void Declare(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].numbers = new List<int>();
                players[i].hit = new List<int>();
                players[i].points = 0;
            }
        }

        public static List<int> Gen(int min_value, int max_value)
        {
            Random random = new Random();
            List<int> vs = new List<int>();

            for (int i = 0; i < Program.NUMS; i++)
            {
                vs.Add(random.Next(min_value, (max_value + 1)));
            }

            return vs;
        }

        public static bool IsNumberCorrect(List<int> correct, int number)
        {
            if (correct.Contains(number)) return true;
            else return false;
        }

        public static void PrintResults(List<Player> players, List<int> nums)
        {
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine("{0}:", players[i].name);
                for (int j = 0; j < Program.NUMS; j++)
                {
                    Thread.Sleep(100);
                    if (IsNumberCorrect(players[i].hit, players[i].numbers[j]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-> {0} (trafiony)", players[i].numbers[j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-> {0}", players[i].numbers[j]);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Wylosowany zestaw:");
            for (int i = 0; i < nums.Count; i++)
            {
                Thread.Sleep(100);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("-> {0} (prawidłowy numer)", nums[i]);
                Console.ResetColor();
            }
        }
    }

    class Program
    {
        public const int MIN_VALUE = 1;
        public const int MAX_VALUE = 49;
        public const int NUMS = 6;
        static void Main(string[] args)
        {
            Console.Title = "UnrealGen";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Unreal");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Gen\n");
            Console.ResetColor();
            List<Unreal.Player> players = new List<Unreal.Player>();
            int temp;
            string temp_s = "";

            Console.WriteLine("Podawaj imiona graczy, ciąg \"\\0\" kończy: ");
            temp = 0;
            while (temp_s != "\\0")
            {
                Console.ResetColor();
                Console.Write("Podaj imię gracza /->/ ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                temp_s = Console.ReadLine();
                if (temp_s != "\\0")
                {
                    players.Add(new Unreal.Player());
                    players[temp].name = temp_s;
                }
                temp++;
            }
            Console.ResetColor();

            Unreal.Declare(players);
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine("OK! {0}, pora na Ciebie!", players[i].name);
                for (int j = 0; j < NUMS; j++)
                {
                    Console.Write("Podaj {0} liczbę /->/ ", (j + 1));
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        temp = int.Parse(Console.ReadLine());
                        if (temp > MAX_VALUE)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Liczba {0} jest większa od maksymalnej wartości {1}", temp, MAX_VALUE);
                            j--;
                            Console.ResetColor();
                            continue;
                        }

                        if (temp < MIN_VALUE)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Liczba {0} jest mniejsza od minimalnej wartości {1}", temp, MIN_VALUE);
                            j--;
                            Console.ResetColor();
                            continue;
                        }

                        if (players[i].numbers.Contains(temp))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Liczba {0} występuje już na liście!", temp);
                            j--;
                            Console.ResetColor();
                            continue;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("{0} -> Prawdopodobnie podałeś ciąg znaków bądź wartość zmiennoprzecinkową zamiast liczby całkowitej!", ex);
                        j--;
                        Console.ResetColor();
                        continue;
                    }
                    catch (OverflowException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("{0} -> Podana liczba jest zbyt wielka dla typu Int32!", ex);
                        j--;
                        Console.ResetColor();
                        continue;
                    }

                    players[i].numbers.Add(temp);
                    Console.ResetColor();
                }
            }

            List<int> correct = Unreal.Gen(MIN_VALUE, MAX_VALUE);
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < NUMS; j++)
                {
                    if (Unreal.IsNumberCorrect(correct, players[i].numbers[j]))
                    {
                        players[i].GetPoint();
                        players[i].Hited(players[i].numbers[j]);
                    }
                }
            }

            Console.WriteLine("\n");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(".");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(".");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(".");
            Thread.Sleep(1000);
            Console.ResetColor();

            Console.WriteLine();
            Unreal.PrintResults(players, correct);
            Console.WriteLine("Do zobaczenia");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\n\n\n\nABY ZAKOŃCZYĆ, WCIŚNIJ DOWOLNY KLAWISZ ... ");
            Console.ReadKey();
        }
    }
}