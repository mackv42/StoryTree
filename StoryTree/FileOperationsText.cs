using System;
using System.Collections.Generic;
using System.Text;

namespace storyTree
{
    public class FileOperationsText : IFileOperator
    {
        public bool Save(string FileName)
        {
            return true;
        }

        public string GetFileContents(string FilePath)
        {
            return "";
        }

        public StoryTree Parse(string Contents)
        {
            return new StoryTree("");
        }
    }

}
