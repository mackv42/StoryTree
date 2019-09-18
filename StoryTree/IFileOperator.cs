using System;
using System.Collections.Generic;
using System.Text;

namespace storyTree
{
    public interface IFileOperator
    {
        bool Save(string FileName);
        string GetFileContents(string FilePath);
        StoryTree Parse(string Contents);
    }
}
