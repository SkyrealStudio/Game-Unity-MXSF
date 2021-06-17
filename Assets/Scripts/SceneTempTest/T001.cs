using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.MyStructures;

namespace Tests.t001
{
    public class T001 : MonoBehaviour
    {
        private class MyClass
        {
            int value = 0;
        }

        private MyStruct1<MyClass> stru = new MyStruct1<MyClass>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                stru.Push(new MyClass());
                Debug.Log("A");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                stru.PopTop();
                Debug.Log("B");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                stru.Dequeue();
                Debug.Log("C");
            }
        }
    }
}
