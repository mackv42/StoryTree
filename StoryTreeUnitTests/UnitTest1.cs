using Microsoft.VisualStudio.TestTools.UnitTesting;
using storyTree;
using System.Collections.Generic;


namespace StoryTreeUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestStoryNodeDefault_ExpectedBehaviour()
        {
            int expected = 1;
            StoryNodeDefault test = new StoryNodeDefault(() =>
            {
                return 1;
            });

            test.Run();
            Assert.IsTrue(test.getResult() == expected);
        }

        [TestMethod]
        public void TestStoryTree_ExpectedBehaviour_FollowsPath()
        {
            // 0 
            StoryTree myStory = new StoryTree("A");
            myStory.AddLevel();

            // 1
            myStory.AddNode(() => { return 1; });

            
            //0
            myStory.AddLevel();
            myStory.AddNode(new StoryNodeDefault(() =>
            {
                return 1;
            }));
            
            myStory.AddNode(new StoryNodeDefault(() =>
            {
                return 0;
            }));
            /////////////////////////////////////////

            //1
            myStory.AddLevel();
            myStory.AddNode(() => { return 1; });
            myStory.AddNode(() => { return 0; });

            //7
            myStory.AddLevel();
            myStory.AddNode(() => { return 6; });
            myStory.AddNode(() => { return 7; });
            myStory.AddNode(() => { return 8; });

            myStory.Start();

            List<IStoryNode> test = myStory.TraversePath();

            List<int> expected = new List<int> { 0, 1, 0, 1, 7 };

            for(int i=0; i<test.Count; i++)
            {
                Assert.IsTrue(test[i].getResult() == expected[i]);
            }
        }

        
    }
}
