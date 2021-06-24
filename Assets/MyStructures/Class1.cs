using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MyStructures
{
    public class MyStruct1<T>
    {
        public MyStruct1()
        {
            data = new T[10];
            _innerSize = 10;

            _count = 0;

            _pushPointer = 0;
            _popPointer = -1;
            _dequeuePointer = 0;
        }

        public void Push(T inVal)
        {
            if (_pushPointer == _innerSize)
            {
                Expand();
            }

            data[_pushPointer] = inVal;

            _pushPointer++;
            _popPointer++;
            _count++;
        }

        public T Dequeue()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | Dequeue");
            }

            _count--;
            return data[_dequeuePointer++];
        }

        public T PopTop()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | PopTop");
            }

            _count--;
            _pushPointer--;
            return data[_popPointer--];
        }

        public T Top()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | Top");
            }
            return data[_popPointer];
        }

        public T Tail()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | Tail");
            }
            return data[_dequeuePointer];
        }


        private void Expand()
        {
            T[] transfer = new T[_innerSize * 2];

            for (int i = _dequeuePointer, j = 0; j < _count; j++, i++)
            {
                transfer[j] = data[i];
            }
            data = transfer;
            _innerSize *= 2;

            _popPointer -= _dequeuePointer;
            _pushPointer -= _dequeuePointer;
            _dequeuePointer = 0;
        }

        private int _innerSize;
        public int Count { get => _count; }
        private int _count;

        public T[] data;

        private int _popPointer;
        private int _pushPointer;
        private int _dequeuePointer;

        public int GetStartIndex { get => _dequeuePointer; }
    }
}
