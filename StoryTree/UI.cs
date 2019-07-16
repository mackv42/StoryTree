using System;
using System.Collections.Generic;
using System.Text;

namespace ChooseYourOwnAdventure
{
    public class UI
    {
        private static readonly object ConsoleLock = new object();

        public static void loopUntilTrue(Func<bool> f)
        {
            while (!f()) { }
        }

        public static void printAndWait(string s, int timeout)
        {
            Console.WriteLine(s);
            System.Threading.Thread.Sleep(timeout);
        }

        public static void printTitle(string s)
        {
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine($"=======================================================================");
            Console.WriteLine($"                           {s} \n\n");
            Console.WriteLine("-----------------------------------------------------------------------");
        }

        public static int promptForOptions(string question, List<string> options)
        {
            int choice = 0;

            string s = "";

            Console.WriteLine(question);
            s = Console.ReadLine();

            Func<int> find = () =>
            {
                for (int i = 0; i < options.Count; i++)
                {
                    if (options[i].Equals(s, StringComparison.OrdinalIgnoreCase)) { return i; }
                }

                return -1;
            };

            choice = find();

            while (choice == -1)
            {
                Console.WriteLine($"I don't know what a {s} is try something else");
                Console.WriteLine(question);
                s = Console.ReadLine();
                choice = find();
            }

            return choice;
        }

        public static int promptForInteger(String prompt)
        {
            int _return = 0;
            loopUntilTrue(() =>
            {
                try
                {
                    Console.Write(prompt);
                    _return = Int32.Parse(Console.ReadLine());
                    if (_return < 0)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException E)
                {
                    Console.WriteLine("You need To enter a number greater than 0!");
                    return false;
                }

                return true;
            }
            );

            return _return;
        }

        public static double promptForDouble(String prompt)
        {
            double _return = 0;
            loopUntilTrue(() =>
            {
                try
                {
                    Console.Write(prompt);
                    _return = Double.Parse(Console.ReadLine());
                    if (_return < 0)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException E)
                {
                    Console.WriteLine("You need To enter a decimal greater than 0!");
                    return false;
                }

                return true;
            }
            );

            return _return;
        }

        public static bool yesNo(String question)
        {
            string answer = ".";
            while (true)
            {
                Console.Write("{0} Y/N ", question);
                answer = Console.ReadLine();

                if (answer == "Y" || answer == "y")
                {
                    return true;
                }
                if (answer == "N" || answer == "n")
                {
                    return false;
                }
                Console.WriteLine("Invalid it's a yes or no question");
            }
        }



        public static double[] promptForMoney(String item, double price)
        {
            Console.WriteLine($"It's ${price} per {item}");
            Console.WriteLine($"How Many {item}s would you like to Buy?");

            try
            {
                Console.Write("Buy: ");
                int quantity = Int32.Parse(Console.ReadLine());
                if (quantity < 0)
                {
                    throw new FormatException();
                }
                if (quantity < 0)
                {
                    return new double[2] { (double)quantity * -1, quantity * -1 * price };
                }
                return new double[2] { (double)quantity, quantity * price };
            }
            catch (FormatException E)
            {
                Console.WriteLine("Invalid you need to enter a number greater than 0");
                return promptForMoney(item, price);
            }
        }

        public static void clearAndDisplay(String s, int x)
        {
            lock (UI.ConsoleLock)
            {
                Console.Clear();
                UI.writeAt(s, x);
                System.Threading.Thread.Sleep(1500);
                Console.Clear();
            }
        }

        public static void writeAt(String s, int x)
        {
            lock (UI.ConsoleLock)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write(s);
            }
        }

        public static int cy;
        public static void writeLineAt(String s, int x)
        {
            cy++;
            lock (UI.ConsoleLock)
            {
                Console.SetCursorPosition(x, cy);
                Console.WriteLine(s);
                Console.SetCursorPosition(0, cy);
            }
        }
    }
}
