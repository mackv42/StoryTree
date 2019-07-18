using System;
using System.Collections.Generic;
using System.Text;

namespace storyTree
{
    public enum StorySwitches
    {
        NULL = -int.MaxValue
    };

    public struct GO_UP
    {
        public int levels { get { return levels; } set { levels = value; } }
        public GO_UP(int l)
        {
            levels = l;
        }
    }

    public struct StorySwitch
    {
        public bool UP;
        public bool END;
        public int option;
        public int levels;

        
        public StorySwitch(int opt, bool e, bool up)
        {
            END = e;
            option = opt;
            UP = up;

            if (UP)
            {
                levels = 1;
            }
            else
            {
                levels = 0;
            }
        }

        public StorySwitch(int opt, int levels)
        {
            END = false;
            UP = true;
            option = opt;
            this.levels = levels;
        }

        public StorySwitch(int opt)
        {
            END = false;
            UP = false;
            levels = 0;
            option = opt;
        }

        public StorySwitch(bool die)
        {
            END = true;
            UP = false;
            levels = 0;
            option = 0;
        }
    }

    public interface IStoryNode
    {
        StorySwitch Run();
        StorySwitch getResult();
    }

    public class StoryNodeDefault : IStoryNode
    {
        private Func<StorySwitch> function;
        private StorySwitch result;

        public StoryNodeDefault(Func<StorySwitch> callback)
        {
            result = new StorySwitch();
            result.option = (int)StorySwitches.NULL;
            function = callback;
        }

        public StorySwitch Run()
        {
            this.result = function();
            return result;
        }

        public StorySwitch getResult()
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
                return new StorySwitch();
            }));
        }

        public List<IStoryNode> TraversePath()
        {
            List<IStoryNode> ret = new List<IStoryNode>();
            for(int i=0; i<story.Count; i++)
            {
                for(int j=0; j<story[i].Count; j++)
                {
                    if (story[i][j].getResult().option != (int)StorySwitches.NULL)
                    {
                        ret.Add(story[i][j]);
                        //Adds twice for some reason
                        break;
                    }
                }
            }

            return ret;
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

        public void AddNode(Func<StorySwitch> function)
        {
            this.story[story.Count - 1].Add(new StoryNodeDefault(function));
        }

        public void Start()
        {
            int choice = 0;
            int tmp = 0;
            StorySwitch choiceTmp = new StorySwitch(0);
            choicesList = new List<int>();

            for (int i = 0; i < story.Count; i++)
            {
                choiceTmp = story[i][choice].Run();
                tmp = choiceTmp.option;

                if (choiceTmp.END)
                {
                    break;
                }

                if (choiceTmp.levels != 0)
                {
                    i -= choiceTmp.levels+1;
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
