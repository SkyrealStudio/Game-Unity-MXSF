using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MyStructures
{
    public class MyStruct1<T>
    {
        public MyStruct1()
        {
            _data = new T[10];
            _innerSize = 10;

            _count = 0;

            _pushPointer = 0;
            _popPointer = -1;
            _dequeuePointer = 0;
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < _count)
                    return _data[_dequeuePointer + index];
                else
                    throw new System.Exception("Index Out Of Range | MyStruct1 | get");
            }
            set
            {
                if (index >= 0 && index < _count)
                    _data[_dequeuePointer + index] = value;
                else
                    throw new System.Exception("Index Out Of Range | MyStruct1 | set");
            }
        }

        public void Enqueue(T inVal) //equals to Push()
        {
            Push(inVal);
        }

        public void Push(T inVal) //equals to Enqueue()
        {
            if (_pushPointer == _innerSize)
            {
                Expand();
            }

            _data[_pushPointer] = inVal;

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
            return _data[_dequeuePointer++];
        }

        public T PopTop()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | PopTop");
            }

            _count--;
            _pushPointer--;
            return _data[_popPointer--];
        }

        public T Top()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | Top");
            }
            return _data[_popPointer];
        }

        public T Tail()
        {
            if (_count == 0)
            {
                throw new System.Exception("yc : Struct Index Out Of Range | Tail");
            }
            return _data[_dequeuePointer];
        }


        private void Expand()
        {
            T[] transfer = new T[_innerSize * 2];

            for (int i = _dequeuePointer, j = 0; j < _count; j++, i++)
            {
                transfer[j] = _data[i];
            }
            _data = transfer;
            _innerSize *= 2;

            _popPointer -= _dequeuePointer;
            _pushPointer -= _dequeuePointer;
            _dequeuePointer = 0;
        }

        
        private int _innerSize;
        public int Count { get => _count; }
        private int _count;

        public T[] _data;

        

        private int _popPointer;
        private int _pushPointer;
        private int _dequeuePointer;

        public int GetStartIndex { get => _dequeuePointer; }
    }
}
