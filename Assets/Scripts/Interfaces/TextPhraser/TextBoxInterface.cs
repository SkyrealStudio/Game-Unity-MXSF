using System.Collections.Generic;

using Interface.Task;

namespace Interface.TextPhraser
{
    public enum TextStyle
    {
        plain,
        selective,
        eventTrigger,
        //special,
    }

    public struct TextPhraser_ReturnUnit
    {
        public string Speaker;
        public string[] Content;
        public ulong Size;
        public TextStyle Style;
        public List<IBaseTask> Events;
    }

    public interface ITextPhraser_Mk001
    {
        bool SetTarget(string scriptAssetPath);//return whether it success or not
        TextPhraser_ReturnUnit GetSingleUnit();
    }
}