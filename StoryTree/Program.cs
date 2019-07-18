using System;
using System.Diagnostics;
using System.Collections.Generic;
using storyTree;
using static storyTree.StorySwitches;

namespace ChooseYourOwnAdventure
{
    class Program
    {
        static void Main(string[] args)
        {
            Process proc = Process.GetCurrentProcess();
            StoryTree myStory = new StoryTree("A");
            Stopwatch stopWatch = new Stopwatch();
            Random rnd = new Random();
            
            stopWatch.Start();
            
            //74 mb 9 x 50,000 = (450,000) - 9
            //74/4.5 = 
            for(int i=0; i<50000; i++)
            {
                myStory.AddLevel();
                for(int j=0; j<9; j++)
                {
                    myStory.AddNode(() =>
                    {
                        return new StorySwitch(rnd.Next(0, j));
                    });
                }
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine(String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10));

            System.Threading.Thread.Sleep(5000);

            stopWatch = new Stopwatch();
            stopWatch.Start();
            myStory.Start();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine(String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10));

            
            List<IStoryNode> test = myStory.TraversePath();
            for (int i = 0; i < test.Count; i++)
            {
                Console.Write(test[i].getResult().option);
            }
        }
    }
}

