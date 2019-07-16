using System;

using System.Collections.Generic;
using static ChooseYourOwnAdventure.StorySwitches;

namespace ChooseYourOwnAdventure
{
    class Program
    {
        static void Main(string[] args)
        {
            StoryTree myStory = new StoryTree("The Journey");

            myStory.AddLevel();

            myStory.AddNode(() =>
            {
                return UI.promptForOptions("Would You Like To go Left or Right",
                new List<string>() { "Left", "Right" });
            });

            myStory.AddLevel();

            myStory.AddNode(() =>
            {
                Console.WriteLine("Wrong Choice You Were Eaten By a Bear!");
                return (int)END;
            });


            myStory.AddNode(() =>
            {
                Console.WriteLine("You continue on the right path");
                Console.WriteLine("The path Splits 3 ways Each has a sign leading to Camelot, Babylon and Constantinople");
                Console.WriteLine("You are growing slightly fatigued and feel like taking a nap also...");
                return UI.promptForOptions("Which Way do you choose to go?",
                    new List<string>() { "Camelot", "Babylon", "Constantinople", "Nap" });
            });

            myStory.AddLevel();

            myStory.AddNode(() =>
            {
                UI.printAndWait("You walk to Camelot...", 1000);
                Console.WriteLine("They send you back on a Sick Horse.");
                UI.printAndWait("You make it back and the horse dies", 600);
                return (int)GO_UP - 1;
            });

            myStory.AddNode(() =>
            {
                UI.printAndWait("You Pace 2000 steps to Babylon...", 1000);
                Console.WriteLine("They Murder you for Trespassing!");
                return (int)END;
            });


            myStory.AddNode(() =>
            {
                UI.printAndWait("You Start Your merry Way to Constantinople", 1000);
                myStory.changeChapter("Skipping along to Constantinople");
                return 0;
            });

            

            StoryTree constantinople = myStory.branch("Skipping along to Constantinople");
            constantinople.AddLevel();

            constantinople.AddNode(() =>
            {
                Console.WriteLine("The king of Constantinople greets you");
                Console.WriteLine("What do you do?");
                return UI.promptForOptions("Shake Hand / Bow",
                    new List<string>() { "Shake", "Bow" });
            });

            constantinople.AddLevel();

            constantinople.AddNode(() =>
            {
                UI.printAndWait("King: How dare thee!", 400);
                Console.WriteLine("You are Beheaded");
                return (int)END;
            });

            constantinople.AddNode(() =>
            {
                Console.Write("King: Welcome Traveler.");
                return 0;
            });

            myStory.AddNode(() =>
            {
                Console.WriteLine("You Walk into the Woods to take a nap");
                myStory.changeChapter("Into the Woods");
                return 0;
            });

            StoryTree woods = myStory.branch("Into the Woods");

            woods.AddLevel();
            woods.AddNode(() =>
            {
                Console.WriteLine("You wake up feeling Refreshed in the morning and don't feel like leaving");
                return (int)GO_UP;
            });

            myStory.Start();
        }
    }
}
