using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scripts.Task;
using Scripts.Task.Chain;

using MyStructures;

using Interface.Task;
using Interface.TextParser.ReturnUnit;

namespace Interface.TextPhraser
{
    interface IParserUnitCarrier
    {
        MyStruct1<DataWithTickCount<Unit_Mk004>> GetTaskEntranceStruct();
    }
}
