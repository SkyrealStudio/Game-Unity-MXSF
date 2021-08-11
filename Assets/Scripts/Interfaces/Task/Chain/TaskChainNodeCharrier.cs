using System.Threading.Tasks;

namespace Interface.Task.Chain
{
    public interface ITaskChainNodeCarrier
    {
        ref TextParser.ReturnUnit.Unit_Mk004 GetTaskChainNode();
        void SetTaskChainNode(TextParser.ReturnUnit.Unit_Mk004 unit);
    }
}
