using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interface.Task;
using Interface.Task.Chain;
using Interface.TextParser.ReturnUnit;

namespace Scripts.Task.Chain
{
    class ChainUnitModifier : IParserUnitModifier
    {
        public Unit_Mk004 Modify(Unit_Mk004 unit)
        {
            Unit_Mk004 ret = new Unit_Mk004();
            ret.Contents = new string[unit.Contents.Length - 1];
            //向前方移动一格
            for(int i=1;i<unit.Contents.Length;i++)
            {
                ret.Contents[i - 1] = unit.Contents[i];
            }
            ret.Speaker = unit.Speaker;
            ret.Style = unit.Style;
            ret.SelfIndex = unit.SelfIndex;
            ret.PossibleNextIndexs = unit.PossibleNextIndexs;
            return ret;
        }
    }
}
