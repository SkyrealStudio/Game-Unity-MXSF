using Interface.Task.Chain;
using Interface.Task;
using Interface.TextParser.ReturnUnit;

namespace Interface.Task
{
    public interface ParserUnitToTaskInterface
    {
        IBaseTask Translate(Unit_Mk004 unit);
    }
}
