using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CharacterInteracter001 : MonoBehaviour
{
    public LongLifeObjectManager longLifeObjectManager;

    public MainCharacterDominantor targetDominantor;

    //private class MyTextTask : IBaseTask
    //{
    //    public void Execute()
    //    {
    //        if (_outTextUI)
    //            _outTextUI.text = _data;
    //        else
    //            throw new System.Exception("No upstream TextUI");
    //    }
        
    //    public MyTextTask(string s, Text text)
    //    {
    //        _outTextUI = text;
    //        _data = s;
    //    }

    //    private string _data;
    //    public Text _outTextUI;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
    //    {
    //        //��һ���ӿ�ʵ��������ҵĽӿڽṹ��
    //        //targetDominantor.taskStack.Push(new MyTextTask("���Ͱ���",longLifeObjectManager.outTextUI));
    //        Debug.Log("in!");
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
    //    {
    //        longLifeObjectManager.outTextUI.text = "--";
    //        targetDominantor.taskStack.Pop();
    //        Debug.Log("leave!");
    //    }
    //}
}
