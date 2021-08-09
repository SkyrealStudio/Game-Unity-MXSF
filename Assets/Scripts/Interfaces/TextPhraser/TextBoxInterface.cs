namespace Interface.TextParser
{
    public enum TextStyle
    {
        plain,
        selective,
        eventTrigger,
        //special,
    }
    
    public class TextParser_ReturnUnit_Mk002
    {
        public string Speaker;
        public string[] Content;
        public ulong Size;
        public TextStyle Style;

        public TextParser_ReturnUnit_Mk002 next;
    }
    
    public interface ITextParser_Mk002
    {
        bool SetTarget(string scriptAssetPath);//return whether it success or not
        TextParser_ReturnUnit_Mk002 GetSingleUnit();
    }
}