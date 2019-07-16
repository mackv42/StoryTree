using System;
using System.Collections.Generic;
using System.Text;

namespace ChooseYourOwnAdventure
{
    public enum StorySwitches
    {
        GO_UP = -1,
        END = -42
    };

    public interface IStoryNode
    {
        int Run();
        int getResult();
    }

    public class StoryNodeDefault : IStoryNode
    {
        private Func<int> function;
        private int result;

        public StoryNodeDefault(Func<int> callback)
        {
            function = callback;
        }

        public int Run()
        {
            this.result = function();
            return result;
        }

        public int getResult()
        {
            return result;
        }
    }

    public static class Container
    {
        public static Dictionary<String, StoryTree> chapterList;
    }

    public class StoryTree
    {
        private List<List<IStoryNode>> story;
        public string Title;
        private List<int> choicesList;

        private bool chapterChange;

        private Dictionary<string, StoryTree> branches;

        public StoryTree getChapter(string nameId)
        {
            return branches[nameId];
        }

        public void changeChapter(string nameId)
        {
            try
            {
                story = branches[nameId].story;
                chapterChange = true;
            } catch( Exception E)
            {
                story = Container.chapterList[nameId].story;
            }
        }
        
        public StoryTree branch(string name)
        {
            branches.Add(name, new StoryTree(name));


            return branches[name];
        }

        public StoryTree(string storyName)
        {
            this.chapterChange = false;
            this.branches = new Dictionary<string, StoryTree>();

            this.story = new List<List<IStoryNode>>();

            this.Title = storyName;

            this.AddLevel();

            story[0].Add(new StoryNodeDefault(() => {
                UI.printTitle(storyName);
                return 0;
            }));
        
        }

        public void AddLevel()
        {
            story.Add(new List<IStoryNode>());
        }

        public void AddLevel(List<IStoryNode> nodes)
        {
            story.Add(nodes);
        }

        public void AddNode( IStoryNode n )
        {
            this.story[story.Count - 1].Add(n);
        }

        public void AddNode(Func<int> function)
        {
            this.story[story.Count - 1].Add(new StoryNodeDefault(function));
        }

        public void Start()
        {
            int choice = 0;
            int tmp = 0;

            choicesList = new List<int>();

            for (int i = 0; i < story.Count; i++)
            {
                tmp = story[i][choice].Run();

                if (tmp == (int)StorySwitches.END)
                {
                    break;
                }

                if (tmp == (int)StorySwitches.GO_UP)
                {
                    
                    i -= 2;
                    choice = choicesList[i];
                    continue;
                }

                if (tmp < (int)StorySwitches.GO_UP)
                {
                    i -= tmp - 1;
                    choice = choicesList[i];

                    continue;
                }

                choicesList.Add(tmp);
                choice = tmp;
                
                if (chapterChange)
                {
                    chapterChange = false;
                    Start();
                    break;
                }
            }
        }
    }
}
