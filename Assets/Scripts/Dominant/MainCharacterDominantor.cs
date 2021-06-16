using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterDominantor : MonoBehaviour
{
    //public Stack<IGeneralTask> generalTaskStack = new Stack<IGeneralTask>();
    public Stack<MytaskAssemble001> taskStack = new Stack<MytaskAssemble001>();
    
    public class MytaskAssemble001 : IBaseTaskAssemble
    {
        public MytaskAssemble001()
        {
            _data = new Queue<IBaseTask>();
            isExecuting = false;
        }
        
        public void Enqueue(IBaseTask task)
        {
            _data.Enqueue(task);
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public void Execute()
        {
            if (isExecuting) return;
            if (_data.Count == 0) return;
            else
            {
                _data.Dequeue().Execute_P(this);
                isExecuting = true;
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

        private Queue<IBaseTask> _data;
        public bool isExecuting;
    }
}
