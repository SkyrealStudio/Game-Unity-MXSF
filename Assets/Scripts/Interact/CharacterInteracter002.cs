using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using MyNamespace;
using Assets.MyStructures;

public class CharacterInteracter002 : MonoBehaviour, IInteractBase
{
    public PersistentObjectManager persistentObjectManager;

    public MainCharacterDominator targetDominantor;
    public ITaskStructCarrier targetTaskStructCarrier;
    public Camera tarCam;
    public TipCarrier001 tipCarrier;
    public ChoiceFormCarrier choiceFormCarrier;
    private MyTaskFactory myTaskFactory;
    private ITickRecorder tickRecorder;

    private List<long> tickID_list;

    public bool needFeed;

    public void InteractIsComplete()
    {
        tipCarrier.gameObject.SetActive(false);
        needFeed = false;
        //persistentObjectManager.tipDominator.Adjust();
    }

    private void Start()
    {
        tickID_list = new List<long>();
        needFeed = true;
        myTaskFactory = new MyTaskFactory(persistentObjectManager, tarCam, this, () => { return judge(2, _dict); });
        myTaskFactory.Load();

        targetTaskStructCarrier = targetDominantor;
    }

    private static string[] _dict = { "test_branch_1", "test_branch_2"};
    private bool[] judge(int branchLength, string[] dict)
    {
        bool[] rev = new bool[branchLength];
        for (int i = 0; i < branchLength; i++)
            if (choiceFormCarrier.choiceForm.Includes(dict[i]))
                rev[i] = false;//hide
            else
                rev[i] = true;//showing allowed

        return rev;
    }

    protected class MyTaskFactory
    {
        public MyTaskFactory(PersistentObjectManager persistentObjectManager,Camera tarCam, IInteractBase interactBase, MyTasks.JudgeAction judgeAction1)
        {
            this.judgeAction1 = judgeAction1;
            this.tarCam = tarCam;
            this.persistentObjectManager = persistentObjectManager;
            this._interactBase = interactBase;
            _taskArray = new IBaseTask[6,40];
        }
        
        public void Load()
        {
            #region
            _indexX = 0;
            _indexY = 0;

            //_indexX == 0
            _taskArray[_indexX, _indexY++] = new MyTasks.Camera_Messenger_Task001(persistentObjectManager.cameraExecuter, 0.5f, new Vector3(-1.26f, 0f,-10f), 10, 0.7f);
            //_taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(persistentObjectManager.currentController.locker, tarCam, 0.5f, tarCam.gameObject.transform.position + new Vector3(0f, -1f), 10, 0.7f);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_001(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, 20, 0.1f);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_002(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, 20, 0.1f);
            _taskArray[_indexX, _indexY++] = new MyTasks.Camera_Messenger_Task001(persistentObjectManager.cameraExecuter, 2.0f, new Vector3(persistentObjectManager.MainCharacterGObj.transform.position.x,
                                                                                                                                          persistentObjectManager.MainCharacterGObj.transform.position.y,
                                                                                                                                          -10f), 10, 0.7f,true);
            //_taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(persistentObjectManager.currentController.locker, tarCam, 1f, tarCam.gameObject.transform.position, 10, 0.7f, true);//2

            //StartBranch -- MC 
            //_indexX == 1
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[3] { "����˭", "������", "��" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "����ռ䣿" }, 20, 0.5f);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "�Ҵ���ġ���" }, 20, 0.5f);//2
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "��" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "ʲô������?" }, 20, 0.5f);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "����˵���" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "��" }, 20, 0.5f);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_001(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[2] { "�Ҹ���ʲô", "Ϊʲô��������" }, 20, 0.5f, judgeAction1);//7
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "��ΪʲôҪ����" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, new string[1] { "ûʲô��" }, 20, 0.5f);//9


            //EndBranch -- MC
            //_indexX == 2
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_002(persistentObjectManager.currentController.locker, persistentObjectManager.textBox, 20, 0.5f);

            //Start -- Fox
            //_indexX == 3
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "...", 40, 0.1f, true);//0

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������������˭���㻹�ǵ�֮ǰ������ʲô��", 40, 0.2f, true);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������������ʲô�����ǵ����ء����������˵��һ��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�������Ѿ��µ�˵���������������Ҵ��˵��һ�����", 40, 0.2f, true);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�������ȣ���������һ�����ҽ��ܣ�����Խ��Һ���Ȼ�������ڴ��ڵĿռ䡭��������Ϊ��ľ���ռ䡣", 40, 0.2f, true);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������������ʵ�Ǹ��Ӹ��ӵĶ��������������Ҳ���ӷ���԰ɣ�˳�㣬����ռ���������ġ�", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������һ�£�����˵����ʱ��������������˵�ꡣ", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�������������ռ������кܶ࣬����һ�㶼����ľ���״̬�йأ�����ȷ��һ�£��㲢û���κμ���԰ɡ���", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "��:��̾�����Ǿ����ˣ���Ϊ�˱����Լ����Է�ס�����Լ��ļ��䣬Ҳ����˵�����㴦�������ⲿ�ֵ���в����Ƭ�ռ�ͻ���ʧ��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�����ü򵥵ķ�ʽ�����;������һ����Լ��ļ���֮����ܴ������ȥ��Ȼ���һḺ�������㡣", 40, 0.2f, true);//9
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�����Ȱѱ��˵Ļ������ǻ������ǣ��벻Ҫ�ٴδ����˵���ˡ�", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�����������㴴���Ŀռ䣬������Щ������������ļ����й����ġ�����������ҵ�����Կ�״������뿪��˵��������ʲô������", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������̾����лл�����Ѿ�˵���ˡ��������Ѿ����͵�ʮ������˰ɣ������ʲô����������ʣ��������ԵĻ��Ҳ����ٽ������Ѿ����͹������⡣", 40, 0.2f, true);//12

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������֮ǰ˵���˰ɡ��ҵ���ļ���Ȼ���뿪�����Ȼ�������Ը�����������Ҳû�����", 40, 0.2f, true);//13

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������", 40, 0.2f, true);//14
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "��������������Ϊ���������뿪������ҡ�����Ϊ��������Ҿ�û�������������ˣ����������Ҳ�������ȷ��ѡ��", 40, 0.2f, true);//15

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������������Ϊ����һ�£���֮���Ҳ��Ầ�㣬���һ�������뿪����ط���", 40, 0.2f, true);//16
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "������Ȼ����Ϊ�Ǻ�������Ҳϣ�����ܹ�������ϡ�����˫�������ٺܶ��鷳��", 40, 0.2f, true);//17

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "��������ʲôҪ�ʵ���", 40, 0.2f, true);//18

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "����������ô��ѯ���ھ���ô�����ˡ�", 40, 0.2f, true);//19
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "�����ã���ô�������ڽ���֮��ĵط��߶�һ�����������ɣ�����������㡣", 40, 0.2f, true);//20

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(persistentObjectManager.textBox, "���������ˣ���������ڵ�״���˾͸Ͻ�ȥ����������˵����������㣬����ؿ�һЩ��", 40, 0.2f, true);//21

            //AllEnd
            //_indexX == 4
            _indexX++; _indexY = 0;
            _taskArray[_indexX, _indexY++] = new MyTasks.Acknowledge_TaskIsComplete(_interactBase);
            #endregion
        }

        public IBaseTask GetTask(int indexX, int indexY)
        {
            return _taskArray[indexX, indexY];
        }

        public static MyTasks.TextBoxGroupTask Pack_GPTsk(IBaseTask a,IBaseTask b)
        {
            return new MyTasks.TextBoxGroupTask(new IBaseTask[2] { a, b });
        }
        public static MyTasks.TextBoxGroupTask Pack_GroupTsk(IBaseTask[] tasks)
        {
            return new MyTasks.TextBoxGroupTask(tasks);
        }
        
        public static MyTasks.TextBoxVariableTask001 Pack_TexVarTsk001(IBaseTask a,MyTasks.JudgeAction j,IBaseTask[] b)
        {
            return new MyTasks.TextBoxVariableTask001(a, j, b);
        }

        public static MyTasks.TextBoxVariableTask002 Pack_TexVarTsk002(IBaseTask a, IBaseTask[] b)
        {
            return new MyTasks.TextBoxVariableTask002(a, b);
        }
        
        public Camera tarCam;
        public PersistentObjectManager persistentObjectManager;
        private IInteractBase _interactBase;
        private IBaseTask[,] _taskArray;

        private int _indexX;
        private int _indexY;
        private MyTasks.JudgeAction judgeAction1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(persistentObjectManager.MainCharacterGObj))
        {
            //targetTaskStructCarrier = collision.gameObject.GetComponent<MainCharacterDominator>();

            if (needFeed)
            {

#if DEBUG
                tickID_list.Add(tickRecorder.GetTickCount());

                TaskQueueWithTickCount<IBaseTask> messengerTaskQueue = new TaskQueueWithTickCount<IBaseTask>(tickRecorder.GetTickCount());

                //MainCharacterDominantor.MytaskAssemble001 transferMyStruct1 = new MainCharacterDominantor.MytaskAssemble001(
                //    tickRecorder.GetTickCount(),
                //    tipCarrier);

                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                    myTaskFactory.GetTask(0, 0), //zoom
                    myTaskFactory.GetTask(0, 1), //show Box
                    myTaskFactory.GetTask(3, 0)  //show TextInit
                }));

                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_GroupTsk(
                    new IBaseTask[]
                    {
                    myTaskFactory.GetTask(0, 2),
                    myTaskFactory.GetTask(0, 3),
                    new MyTasks.Acknowledge_TaskIsComplete(this),
                    new MyTasks.ControllerLockerAction001(persistentObjectManager.currentController.locker,ControllerLocker.ControllerLockerState.Unlocked)
                    }
                ));

                targetTaskStructCarrier.GetTaskStruct().Push(messengerTaskQueue);
                Debug.Log("in: " + gameObject.name);
                //persistentObjectManager.tipDominator.Adjust();
#else
                tickID_list.Add(tickRecorder.GetTickCount());

                TaskQueueWithTickCount<IBaseTask> messengerTaskQueue = new TaskQueueWithTickCount<IBaseTask>(tickRecorder.GetTickCount());
                
                //MainCharacterDominantor.MytaskAssemble001 transferMyStruct1 = new MainCharacterDominantor.MytaskAssemble001(
                //    tickRecorder.GetTickCount(),
                //    tipCarrier);

                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                    myTaskFactory.GetTask(0, 0), //zoom
                    myTaskFactory.GetTask(0, 1), //show Box
                    myTaskFactory.GetTask(3, 0)  //show TextInit
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 0));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                    myTaskFactory.GetTask(3, 1),
                    myTaskFactory.GetTask(3, 2),
                    myTaskFactory.GetTask(3, 3)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 4));
                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 1));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                    myTaskFactory.GetTask(3, 5)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 2));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                    myTaskFactory.GetTask(3,6)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 7));
                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 3));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                    myTaskFactory.GetTask(3,8)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 4));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                    myTaskFactory.GetTask(3,9)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 5));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                    myTaskFactory.GetTask(3,10)
                }));
                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 11));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 6));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                    myTaskFactory.GetTask(3,12)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 7));

                //messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]
                //{
                //    MyTaskFactory.Pack_GPTsk(
                //        myTaskFactory.GetTask(3,13),
                //        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_1")
                //    ),
                //    MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                //        myTaskFactory.GetTask(3,14),
                //        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_2"),
                //        new MyTasks.TaskStructModifierTask001(TaskQueueMethods.InsertQueueWith,new IBaseTask[]{
                //            myTaskFactory.GetTask(3,15),

                //            myTaskFactory.GetTask(1,8),
                //            MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                //                myTaskFactory.GetTask(3,16)
                //            }),
                //            myTaskFactory.GetTask(3,17),
                //        },
                //        )
                //    })
                //}));

                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]
                {
                    MyTaskFactory.Pack_GPTsk(
                        myTaskFactory.GetTask(3,13),
                        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_1")
                    ),
                    MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                        myTaskFactory.GetTask(3,14),
                        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_2"),
                        new MyTasks.TaskStructModifierTask001(TaskQueueMethods.instance.InsertQueueWith,new IBaseTask[]{
                            myTaskFactory.GetTask(3,15),

                            myTaskFactory.GetTask(1,8),
                            MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                                myTaskFactory.GetTask(3,16)
                            }),
                            myTaskFactory.GetTask(3,17),
                        },
                        persistentObjectManager.MainCharacterGObj.GetComponent<MainCharacterDominator>().GetTaskStruct())
                    })
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 18));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 7));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk001(myTaskFactory.GetTask(2, 0), () => { return judge(2, _dict); }, new IBaseTask[]
                {
                    MyTaskFactory.Pack_GPTsk(
                        myTaskFactory.GetTask(3,13),
                        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_1")
                    ),
                    MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                        myTaskFactory.GetTask(3,14),
                        new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_2"),
                        new MyTasks.TaskStructModifierTask001(TaskQueueMethods.instance.InsertQueueWith,new IBaseTask[]{
                            myTaskFactory.GetTask(3,15),

                            myTaskFactory.GetTask(1,8),
                            MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                                myTaskFactory.GetTask(3,16)
                            }),
                            myTaskFactory.GetTask(3,17),
                        },
                        persistentObjectManager.MainCharacterGObj.GetComponent<MainCharacterDominator>().GetTaskStruct())
                    })
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 18));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(1, 9));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                    myTaskFactory.GetTask(3, 19)
                }));

                messengerTaskQueue.Enqueue(myTaskFactory.GetTask(3, 20));
                messengerTaskQueue.Enqueue(MyTaskFactory.Pack_GroupTsk(
                    new IBaseTask[]
                    {
                    myTaskFactory.GetTask(0, 2),
                    myTaskFactory.GetTask(0, 3),
                    new MyTasks.Acknowledge_TaskIsComplete(this),
                    new MyTasks.ControllerLockerAction001(persistentObjectManager.currentController.locker,ControllerLocker.ControllerLockerState.Unlocked)
                    }
                ));

                targetTaskStructCarrier.GetTaskStruct().Push(messengerTaskQueue);
                Debug.Log("in: " + gameObject.name);
                //persistentObjectManager.tipDominator.Adjust();
#endif
            }

            else
            {
                tickID_list.Add(tickRecorder.GetTickCount());

                TaskQueueWithTickCount<IBaseTask> messengerTaskQueue_noneFeed = new TaskQueueWithTickCount<IBaseTask>(tickRecorder.GetTickCount());
                //MainCharacterDominantor.MytaskAssemble001 tAssemble = new MainCharacterDominantor.MytaskAssemble001(
                //    tickRecorder.GetTickCount(),
                //    tipCarrier);

                messengerTaskQueue_noneFeed.Enqueue(MyTaskFactory.Pack_GroupTsk(new IBaseTask[]{
                    myTaskFactory.GetTask(0, 0), //zoom
                    myTaskFactory.GetTask(0, 1), //show Box
                    myTaskFactory.GetTask(3, 21)
                }));
                messengerTaskQueue_noneFeed.Enqueue(MyTaskFactory.Pack_GroupTsk(
                    new IBaseTask[]
                    {
                    myTaskFactory.GetTask(0, 2),//close Box
                    myTaskFactory.GetTask(0, 3),//zoom
                    new MyTasks.Acknowledge_TaskIsComplete(this),
                    new MyTasks.ControllerLockerAction001(persistentObjectManager.currentController.locker,ControllerLocker.ControllerLockerState.Unlocked)
                    }
                ));


                //targetDominantor.taskStack.Push(tAssemble);
                targetTaskStructCarrier.GetTaskStruct().Push(messengerTaskQueue_noneFeed);
                Debug.Log("in: " + gameObject.name);
                //persistentObjectManager.tipDominator.Adjust();
            }
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(persistentObjectManager.MainCharacterGObj))
        {
            //targetTaskStructCarrier = collision.gameObject.GetComponent<MainCharacterDominator>();
            try
            {
                if (targetTaskStructCarrier.GetTaskStruct().Count > 0)
                {
                    if (tickID_list.Contains(targetTaskStructCarrier.GetTaskStruct().Tail().tickCount))
                    {
                        targetTaskStructCarrier.GetTaskStruct().Dequeue();
                    }
                    else if (tickID_list.Contains(targetTaskStructCarrier.GetTaskStruct().Top().tickCount))
                    {
                        targetTaskStructCarrier.GetTaskStruct().Dequeue();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("seems this task have been executed");
            }

            //persistentObjectManager.tipDominator.Adjust();

            Debug.Log("leave: " + gameObject.name);
        }
    }


    private void Awake()
    {
        tickRecorder = persistentObjectManager;
    }
}
