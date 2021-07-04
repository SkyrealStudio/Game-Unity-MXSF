using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour,IInteractBase
{
    public LongLifeObjectManager longLifeObjectManager;
    
    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;
    public TipCarrier001 tipCarrier;
    public ChoiceFormCarrier choiceFormCarrier;

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
    }

    private static string[] _dict = { "test_branch_1", "test_branch_2", "test_branch_3" };
    private bool[] judge(int branchLength,string[] dict)
    {
        bool[] rev = new bool[branchLength];
        for (int i = 0; i < branchLength; i++)
            if (choiceFormCarrier.choiceForm.Includes(dict[i]))
                rev[i] = false;//hide
            else
                rev[i] = true;//showing allowed

        return rev;
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

            tAssemble.Enqueue(new MyTasks.TextBoxGroupTask(
                new IBaseTask[] {
                    new MyTasks.CameraMove_Zoom_001(
                        longLifeObjectManager.currentController.locker,
                        tarCam,
                        0.5f,
                        tarCam.gameObject.transform.position + new Vector3(0f, -1f),
                        10,
                        0.7f),

                    new MyTasks.TextBoxAdjust_001(
                        longLifeObjectManager.currentController.locker,
                        longLifeObjectManager.textBox,
                        20,
                        0.1f),
                    new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"...",40,0.1f,true)
                })
            );

            tAssemble.Enqueue(
                new MyTasks.TextBoxBranchAdjust_001(
                    longLifeObjectManager.currentController.locker,
                    longLifeObjectManager.textBox,
                    new string[3] { "����˭", "������", "��" },
                    20,
                    0.5f,
                    () => { return new bool[3] { true, true, true }; }
                    )
                );
            tAssemble.Enqueue(
                new MyTasks.TextBoxVariableTask001(
                    new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f),
                    () => { return new bool[3] { true, true, true }; },
                    new IBaseTask[3]
                    {
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"������������˭���㻹�ǵ�֮ǰ������ʲô��",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"������������ʲô�����ǵ����ء����������˵��һ��",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"�������Ѿ��µ�˵���������������Ҵ��˵��һ�����",40,0.2f,true)
                    }
                ));
            
            tAssemble.Enqueue(
                new MyTasks.TextBoxVariableTask001(
                    new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f),
                    () => { return judge(3, _dict); },
                    new IBaseTask[3]
                    {
                        new MyTasks.TextBoxGroupTask(new IBaseTask[]{
                            new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"���ǵ�һ��ѡ��",40,0.2f,true),
                            new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_1")
                        }),
                        new MyTasks.TextBoxGroupTask(new IBaseTask[]{
                            new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"this is the second choice",40,0.2f,true),
                            new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_2")
                        }),
                        new MyTasks.TextBoxGroupTask(new IBaseTask[]{
                            new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"�� ��ݧ��ѧ�, �ӧ� �ӧ�ҧ�ѧݧ� ���� �ӧѧ�ڧѧߧ��",40,0.2f,true),
                            new MyTasks.ChoiceMarkerTask(choiceFormCarrier.choiceForm,"test_branch_3"),
                            new MyTasks.TaskStructCuterTask001(tAssemble.ChangeQueueWith,new IBaseTask[]
                            {
                                new MyTasks.TextBoxGroupTask( new IBaseTask[]{
                                new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"-�¼�����гɹ�-",40,0.2f,true),
                                new MyTasks.Acknowledge_TaskIsComplete(this),
                                new MyTasks.TextBoxAdjust_002(longLifeObjectManager.currentController.locker,longLifeObjectManager.textBox,20,0.1f),
                                new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker,tarCam,1f,tarCam.gameObject.transform.position,10,0.7f,true)
                                })
                            })
                        })
                    }
                ));

            tAssemble.Enqueue(new MyTasks.TextBoxGroupTask(
                new IBaseTask[] {
                    new MyTasks.Acknowledge_TaskIsComplete(this),
                    new MyTasks.TextBoxAdjust_002(longLifeObjectManager.currentController.locker,longLifeObjectManager.textBox,20,0.1f),
                    new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker,tarCam,1f,tarCam.gameObject.transform.position,10,0.7f,true)
                }
                ));
            
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
