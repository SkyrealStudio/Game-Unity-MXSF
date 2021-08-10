using System;

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using MethodClasses;
using Scripts;
using MyStructures;

using Interface;
using Interface.Task;
using Interface.Tick;
using Interface.TextParser;
using Interface.TextParser.ReturnUnit;

using MyTasks;

namespace Scripts
{
    class CharacterInteracter003 : MonoBehaviour
    {
        [SerializeField]
        private ulong[] MyIndexs;

        private List<int> tickID_list = new List<int>();

        public PersistentObjectManager persistentObjectManager;
        public MainCharacterDominator targetDominantor;
        public ITaskEntranceStruct taskEntranceStruct;
        public ITextParser_Mk004 TextParser;

        private class FeedCheck
        {
            public FeedCheck()
            {
                status = Status.neverInteracted;
            }
            public enum Status
            {
                neverInteracted,
                Talked001,
            }
            public Status status;
        }
        private FeedCheck feedCheck;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.parent.gameObject == persistentObjectManager.MainCharacterGObj)
            {
                Debug.Log("Entering: " + gameObject.name);
                int tick = tickRecorder.GetTickCount();
                tickID_list.Add(tick);
                MyStruct1<DataWithTickCount<IBaseTask>> dest = taskEntranceStruct.GetTaskEntranceStruct();

                if (feedCheck.status == FeedCheck.Status.neverInteracted)
                {
                    TaskChainEntrance tce = new TaskChainEntrance(persistentObjectManager.currentController.locker,
                                                                  TextParser.GetUnit(MyIndexs[0]),
                                                                  targetDominantor);
                    dest.Push(new DataWithTickCount<IBaseTask>(tick,tce));
                }
                else if (feedCheck.status == FeedCheck.Status.Talked001)
                {
                    TaskChainEntrance tce = new TaskChainEntrance(persistentObjectManager.currentController.locker,
                                                                  TextParser.GetUnit(MyIndexs[1]),
                                                                  targetDominantor);
                    dest.Push(new DataWithTickCount<IBaseTask>(tick, tce));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.parent.gameObject == persistentObjectManager.MainCharacterGObj &&
                taskEntranceStruct.GetTaskEntranceStruct().Count>0)
            {
                MyStruct1<DataWithTickCount<IBaseTask>> from = taskEntranceStruct.GetTaskEntranceStruct();

                if (tickID_list.Contains(from.Top().tickCount))
                {
                    from.PopTop();
                }
                else if(tickID_list.Contains(from.Tail().tickCount))
                {
                    from.Dequeue();
                }
                else
                {
                    return;
                }
            }
        }

        private ITickRecorder tickRecorder;
        private void Awake()
        {
            tickRecorder = persistentObjectManager;
            taskEntranceStruct = targetDominantor;
        }
    }
}
