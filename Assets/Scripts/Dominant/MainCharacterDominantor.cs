using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MyStructures;
using System.Threading.Tasks;

public class MainCharacterDominantor : MonoBehaviour
{
    //public Stack<IGeneralTask> generalTaskStack = new Stack<IGeneralTask>();
    internal MyStruct1<MytaskAssemble001> taskStack = new MyStruct1<MytaskAssemble001>();
    
    public class MytaskAssemble001 : IBaseTaskAssemble
    {
        public MytaskAssemble001(long tickID)
        {
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
                bool invest = await Execute(1); // includes *.Dequeue();
                if (_data.Count>0 && invest && _data.Peek().GetType() == new ConnecterTask().GetType())
                {
                    ConnecterTask tConnecterTask = (_data.Peek() as ConnecterTask);
                    _data.Dequeue();//Dump The Connecter;
                    await Execute(tConnecterTask.ConnectCount);
                }
            }
            Debug.Log("All Done");
        }
        
        public async Task<bool> Execute(int reqTaskCount)
        {
            if (isExecuting) return false;
            if (_data.Count < reqTaskCount) throw new System.Exception("Too less Tasks remaining for Execute | Execute(int) : value= "+ reqTaskCount);
            else
            {
                isExecuting = true;
                for(int i=0;i<reqTaskCount;i++)
                {
                    Debug.Log("E:" + i + "Ex_Start");
                    await _data.Dequeue().Execute_P(this);
                    Debug.Log("E:" + i + "Ex_End");
                }
                return true;
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
