using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour, IInteractBase
{
    public LongLifeObjectManager longLifeObjectManager;

    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;
    public TipCarrier001 tipCarrier;
    public ChoiceFormCarrier choiceFormCarrier;
    private MyTaskFactory myTaskFactory;

    private List<long> tickID_list;

    public bool needFeed;

    public void InteractIsComplete()
    {
        tipCarrier.gameObject.SetActive(false);
        needFeed = false;
        longLifeObjectManager.tipDominator.Adjust();
    }

    private void Start()
    {
        tickID_list = new List<long>();
        needFeed = true;
        myTaskFactory = new MyTaskFactory(longLifeObjectManager, tarCam, this, () => { return judge(2, _dict); });
        myTaskFactory.Load();
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
        public MyTaskFactory(LongLifeObjectManager longLifeObjectManager,Camera tarCam, IInteractBase interactBase, MyTasks.JudgeAction judgeAction1)
        {
            this.judgeAction1 = judgeAction1;
            this.tarCam = tarCam;
            this.longLifeObjectManager = longLifeObjectManager;
            this._interactBase = interactBase;
            _taskArray = new IBaseTask[6,40];
        }
        
        public void Load()
        {
            _indexX = 0;
            _indexY = 0;

            //_indexX == 0
            _taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker, tarCam, 0.5f, tarCam.gameObject.transform.position + new Vector3(0f, -1f), 10, 0.7f);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_001(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.1f);
            _taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker, tarCam, 1f, tarCam.gameObject.transform.position, 10, 0.7f, true);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.1f);

            //StartBranch -- MC 
            //_indexX == 1
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[3] { "����˭", "������", "��" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "����ռ䣿" }, 20, 0.5f);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "�Ҵ���ġ���" }, 20, 0.5f);//2
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "��" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "ʲô������?" }, 20, 0.5f);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "����˵���" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "��" }, 20, 0.5f);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_001(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[2] { "�Ҹ���ʲô", "Ϊʲô��������" }, 20, 0.5f, judgeAction1);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "��ΪʲôҪ����" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "ûʲô��" }, 20, 0.5f);//9


            //EndBranch -- MC
            //_indexX == 2
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f);

            //Start -- Fox
            //_indexX == 3
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "...", 40, 0.1f, true);//0

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������������˭���㻹�ǵ�֮ǰ������ʲô��", 40, 0.2f, true);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������������ʲô�����ǵ����ء����������˵��һ��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�������Ѿ��µ�˵���������������Ҵ��˵��һ�����", 40, 0.2f, true);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�������ȣ���������һ�����ҽ��ܣ�����Խ��Һ���Ȼ�������ڴ��ڵĿռ䡭��������Ϊ��ľ���ռ䡣", 40, 0.2f, true);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������������ʵ�Ǹ��Ӹ��ӵĶ��������������Ҳ���ӷ���԰ɣ�˳�㣬����ռ���������ġ�", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������һ�£�����˵����ʱ��������������˵�ꡣ", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�������������ռ������кܶ࣬����һ�㶼����ľ���״̬�йأ�����ȷ��һ�£��㲢û���κμ���԰ɡ���", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "��:��̾�����Ǿ����ˣ���Ϊ�˱����Լ����Է�ס�����Լ��ļ��䣬Ҳ����˵�����㴦�������ⲿ�ֵ���в����Ƭ�ռ�ͻ���ʧ��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�����ü򵥵ķ�ʽ�����;������һ����Լ��ļ���֮����ܴ������ȥ��Ȼ���һḺ�������㡣", 40, 0.2f, true);//9
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�����Ȱѱ��˵Ļ������ǻ������ǣ��벻Ҫ�ٴδ����˵���ˡ�", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�����������㴴���Ŀռ䣬������Щ������������ļ����й����ġ�����������ҵ�����Կ�״������뿪��˵��������ʲô������", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������̾����лл�����Ѿ�˵���ˡ��������Ѿ����͵�ʮ������˰ɣ������ʲô����������ʣ��������ԵĻ��Ҳ����ٽ������Ѿ����͹������⡣", 40, 0.2f, true);//12

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������֮ǰ˵���˰ɡ��ҵ���ļ���Ȼ���뿪�����Ȼ�������Ը�����������Ҳû�����", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������", 40, 0.2f, true);//14
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "��������������Ϊ���������뿪������ҡ�����Ϊ��������Ҿ�û�������������ˣ����������Ҳ�������ȷ��ѡ��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������������Ϊ����һ�£���֮���Ҳ��Ầ�㣬���һ�������뿪����ط���", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "������Ȼ����Ϊ�Ǻ�������Ҳϣ�����ܹ�������ϡ�����˫�������ٺܶ��鷳��", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "��������ʲôҪ�ʵ���", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "����������ô��ѯ���ھ���ô�����ˡ�", 40, 0.2f, true);//19
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "�����ã���ô�������ڽ���֮��ĵط��߶�һ�����������ɣ�����������㡣", 40, 0.2f, true);//20
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "���������ˣ���������ڵ�״���˾͸Ͻ�ȥ����������˵����������㣬����ؿ�һЩ��", 40, 0.2f, true);

            //AllEnd
            //_indexX == 4
            _indexX++; _indexY = 0;
            _taskArray[_indexX, _indexY++] = new MyTasks.Acknowledge_TaskIsComplete(_interactBase);
        }
        
        public IBaseTask GetTask(int indexX, int indexY)
        {
            return _taskArray[indexX, indexY];
        }

        public static MyTasks.TextBoxGroupTask Pack_GPTsk(IBaseTask a,IBaseTask b)
        {
            return new MyTasks.TextBoxGroupTask(new IBaseTask[2] { a, b });
        }
        public static MyTasks.TextBoxGroupTask Pack_GPTsk(IBaseTask[] tasks)
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

        public static MyTasks.TaskStructCuterTask001 Pack_CuterTsk001(UnityAction<IBaseTask[]> ua, IBaseTask[] refTsks)
        {
            return new MyTasks.TaskStructCuterTask001(ua, refTsks);
        }

        public Camera tarCam;
        public LongLifeObjectManager longLifeObjectManager;
        private IInteractBase _interactBase;
        private IBaseTask[,] _taskArray;

        private int _indexX;
        private int _indexY;
        private MyTasks.JudgeAction judgeAction1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj)
            && needFeed)
        {
            tickID_list.Add(longLifeObjectManager.tickRecorder.tickCount);
            //��һ���ӿ�ʵ��������ҵĽӿڽṹ��
            MainCharacterDominantor.MytaskAssemble001 tAssemble = new MainCharacterDominantor.MytaskAssemble001(
                longLifeObjectManager.tickRecorder.tickCount,
                tipCarrier);

            // ua : tAssemble.ChangeQueueWith
            // tAssemble.Enqueue();

            tAssemble.Enqueue(MyTaskFactory.Pack_GPTsk(new IBaseTask[]{
                myTaskFactory.GetTask(0, 0), //zoom
                myTaskFactory.GetTask(0, 1), //show Box
                myTaskFactory.GetTask(3, 0)  //show TextInit
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 0));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                myTaskFactory.GetTask(3, 1),
                myTaskFactory.GetTask(3, 2),
                myTaskFactory.GetTask(3, 3)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(3, 4));
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 1));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                myTaskFactory.GetTask(3, 5)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 2));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,6)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(3, 7));
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 3));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,8)
            }));
            
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 4));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,9)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 5));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,10)
            }));
            tAssemble.Enqueue(myTaskFactory.GetTask(3, 11));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 6));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,12)
            }));

            targetDominantor.taskStack.Push(tAssemble);
            Debug.Log("in: "+gameObject.name);
            longLifeObjectManager.tipDominator.Adjust();



            
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj) && needFeed)
        {
            try
            {
                if (tickID_list.Contains(targetDominantor.taskStack.Tail().tickID))
                {
                    targetDominantor.taskStack.Dequeue();
                }
                else if (tickID_list.Contains(targetDominantor.taskStack.Top().tickID))
                {
                    targetDominantor.taskStack.PopTop();
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("seems this task have been executed");
            }

            longLifeObjectManager.tipDominator.Adjust();

            Debug.Log("leave: "+gameObject.name);
        }
    }
}
