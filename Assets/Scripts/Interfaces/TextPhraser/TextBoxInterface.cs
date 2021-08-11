using Interface.TextParser.ReturnUnit;

namespace Interface.TextParser
{
    namespace ReturnUnit
    {
        public enum TextStyle
        {
            Plain,
            Selective,
            EventTrigger,
            //Special,
        }
        
        public class Unit_Mk004
        {
            public int SelfIndex;
            public string Speaker;
            public string[] Contents;
            public TextStyle Style;
            public int[] PossibleNextIndexs;
            //public Unit_Mk004[] nexts;
        }
    }

    public interface ITextParser_Mk004
    {
        bool SetTargetFile(string scriptAssetPath);//return whether it success or not
        Unit_Mk004 GetUnit(int index);
    }
}
