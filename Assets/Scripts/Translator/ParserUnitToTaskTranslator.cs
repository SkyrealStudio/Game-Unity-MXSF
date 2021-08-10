using Interface.TextParser.ReturnUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interface.Task;

namespace Translator
{
    public class ParserUnitToTaskTranslator : ParserUnitToTaskInterface
    {
        public IBaseTask Translate(Unit_Mk004 unit)
        {
            switch(unit.Style)
            {
                case TextStyle.plain:

                    break;
                case TextStyle.eventTrigger:
                    break;
                case TextStyle.selective:
                    break;
                default:
                    break;
            }
        }
    }
}
