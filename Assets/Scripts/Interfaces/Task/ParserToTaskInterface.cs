using Interface.Task.Chain;
using Interface.Task;
using Interface.TextParser.ReturnUnit;

using System.Collections.Generic;

namespace Interface.Task
{
    public interface IParserUnitToTaskInterface
    {
        IBaseTask Translate(Unit_Mk004 unit);
    }
    namespace Chain
    {
        public interface IParserUnitModifier
        {
            Unit_Mk004 Modify(Unit_Mk004 unit);
        }
    }
}
