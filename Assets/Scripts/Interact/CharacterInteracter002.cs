using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour
{
    public LongLifeObjectManager longLifeObjectManager;
    
    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;

    private List<long> tickID_list;
    
    private void Start()
    {
        tickID_list = new List<long>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj))
        {
            //把一个接口实例放入玩家的接口结构中
            tickID_list.Add(longLifeObjectManager.tickRecorder.tickCount);
            MainCharacterDominantor.MytaskAssemble001 tAssemble = 
                new MainCharacterDominantor.MytaskAssemble001(longLifeObjectManager.tickRecorder.tickCount);
            
            tAssemble.Enqueue(new MyTasks.CameraMove_Zoom_001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                0.5f,
                tarCam.gameObject.transform.position + new Vector3(0f, -1f),
                10,
                0.7f
            ));

            //----------Connecter
            tAssemble.Enqueue(new ConnecterTask(1));

            tAssemble.Enqueue(new MyTasks.TextBoxAdjust_001(
                longLifeObjectManager.currentController.locker,
                longLifeObjectManager.textBox,
                20,
                0.1f));//Open

            tAssemble.Enqueue(new MyTasks.TextBoxTextWork_000(
                longLifeObjectManager.textBox
                ));
            //----------Connecter
            tAssemble.Enqueue(new ConnecterTask(1));

            tAssemble.Enqueue(new MyTasks.TextBoxTextWork_001(
                longLifeObjectManager.textBox,
                "1234567890123456789012345678901234567890123456789012345678901234567890",
                40,
                2f
                ));

            tAssemble.Enqueue(new MyTasks.TextBoxAdjust_002(
                longLifeObjectManager.currentController.locker,
                longLifeObjectManager.textBox,
                20,
                0.1f));//Close
            //----------Connecter
            tAssemble.Enqueue(new ConnecterTask(1));

            tAssemble.Enqueue(new MyTasks.CameraMove_Zoom_001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                1f,
                tarCam.gameObject.transform.position,
                10,
                0.7f,
                true
            ));
            targetDominantor.taskStack.Push(tAssemble);

            Debug.Log("in: "+gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj))
        {
            try
            {
                if (tickID_list.Contains(targetDominantor.taskStack.Tail().tickID))
                    targetDominantor.taskStack.Dequeue();
            }
            catch (System.Exception e)
            {
                
            }
            Debug.Log("leave: "+gameObject.name);
        }
    }
}
