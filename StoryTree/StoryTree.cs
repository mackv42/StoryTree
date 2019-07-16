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

    public interface StoryNode
    {
        int Run();
        int getResult();
    }

    public class StoryNodeDefault : StoryNode
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

    public class StoryTree
    {
        private List<List<StoryNode>> story;
        public string Title;
        private List<int> choicesList;
        private List<StoryTree> chapters;
        private bool chapterChange;

        private Dictionary<string, StoryTree> branches;

        public StoryTree getChapter(string nameId)
        {
            return branches[nameId];
        }

        public void changeChapter(string nameId)
        {
            chapters.Add(this);
            story = branches[nameId].story;
            chapterChange = true;
        }

        public StoryTree branch(string name)
        {
            branches.Add(name, new StoryTree(name));
            return branches[name];
        }

        public StoryTree(string storyName)
        {
            this.chapterChange = false;
            this.chapters = new List<StoryTree>();
            this.branches = new Dictionary<string, StoryTree>();
            this.story = new List<List<StoryNode>>();

            this.Title = storyName;

            this.AddLevel();
            story[0].Add(new StoryNodeDefault(() => {
                UI.printTitle(storyName);
                return 0;
            }));
        }

        public void AddLevel()
        {
            story.Add(new List<StoryNode>());
        }

        public void AddLevel(List<StoryNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                this.story[story.Count - 1].Add(nodes[i]);
            }
        }

        public void AddNode( StoryNode n )
        {
            this.story[story.Count - 1].Add(n);
        }

        public void AddNode(Func<int> function)
        {
            this.story[story.Count - 1].Add(new StoryNodeDefault(function));
        }

        public void Start()
        {
            int[] currentNode = new int[2] { 0, 0 };
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
            }

            if (chapterChange)
            {
                chapterChange = false;
                this.Start();
            }
        }
    }
}
