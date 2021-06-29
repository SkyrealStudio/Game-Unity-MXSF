using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MyStructures;
using System.Threading.Tasks;

public class MainCharacterDominantor : MonoBehaviour
{
    //public Stack<IGeneralTask> generalTaskStack = new Stack<IGeneralTask>();
    public MyStruct1<MytaskAssemble001> taskStack = new MyStruct1<MytaskAssemble001>();
    
    public class MytaskAssemble001 : IBaseTaskAssemble
    {
        public MyNamespace.ITipBase tipCarrier;

        public MytaskAssemble001(long tickID, MyNamespace.ITipBase tipCarrier)
        {
            this.tipCarrier = tipCarrier;
            _data = new Queue<IBaseTask>();
            isExecuting = false;

            this.tickID = tickID;
        }
        
        public void Enqueue(IBaseTask task)
        {
            _data.Enqueue(task);
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public async void Execute()
        {
            if (isExecuting) return;
            if (_data.Count == 0) throw new System.Exception("Too less Tasks remaining for Execute | Execute(void)");
            else
            {
                isExecuting = true;
                await _data.Dequeue().Execute_P(this);
            }
            Debug.Log("All Done");
        }
        
        public async void ExecuteVariableTask_Path001(int para)
        {
            if (isExecuting) return;
            if (_data.Count == 0) throw new System.Exception("Too less Tasks remaining for Execute | Execute(void)");
            else
            {
                isExecuting = true;
                IVariableTask current = _data.Dequeue() as IVariableTask;
                current.Select(para);
                await current.Execute_P(this);
            }
        }

        int IBaseTaskAssemble.Execute_GetCount()
        {
            Execute();
            return _data.Count;
        }

        public void ReleaseExecutingStatus()
        {
            isExecuting = false;
        }
        
        public long tickID;
        private Queue<IBaseTask> _data;
        public bool isExecuting;
    }
}
