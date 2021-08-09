﻿using System.Collections.Generic;
using Interface.TextParser.ReturnUnit;

namespace Interface.TextParser
{
    namespace ReturnUnit
    {
        public enum TextStyle
        {
            plain,
            selective,
            eventTrigger,
            //special,
        }
        
        public class Unit_Mk004
        {
            public ulong SelfIndex;
            
            public string speaker;
            public string[] content;
            public TextStyle style;

            public ulong[] PossibleNextIndexs;
            //public Unit_Mk004[] nexts;
        }
    }

    public interface ITextParser_Mk004
    {
        bool SetTargetFile(string scriptAssetPath);//return whether it success or not
        Unit_Mk004 GetUnit(ulong index);
    }
}
