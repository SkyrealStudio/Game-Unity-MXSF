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
        string Speaker;
        string[] TextContext;
        int TextCount;
        TextStyle textSet;

        List<IBaseTask> Events;
    }

    public interface ITextPhraser_Mk001
    {
        bool SetTarget(string scriptAssetPath);//return whether it success or not
        TextPhraser_ReturnUnit GetSingleUnit();
    }
}