using Interface.TextParser.ReturnUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interface.Task;

namespace Translator
{
    public class ParserUnitToTaskTranslator : IParserUnitToTaskInterface
    {
        public ParserUnitToTaskTranslator(PersistentObjectManager pom)
        {
            this.pom = pom;
        }

        private PersistentObjectManager pom;
        
        public IBaseTask Translate(Unit_Mk004 unit)
        {
            switch(unit.Style)
            {
                case TextStyle.Plain:
                    return new MyTasks.TextBoxTextWork_002(pom.DefaultUIShowerSetting, _Refine(unit,0),true);
                case TextStyle.EventTrigger:
                    throw new NotImplementedException();
                    break;
                case TextStyle.Selective:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private string _Refine(Unit_Mk004 unit,int contentIndex)
        {
            return unit.Speaker + ": " + unit.Contents[contentIndex];
        }

    }
}
