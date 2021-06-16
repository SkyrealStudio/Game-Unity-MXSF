namespace Assets.MyStructures
{
    class MyStruct1<T>
    {
        public MyStruct1()
        {
            data = new T[10];
            _innerSize = 10;

            _count = 0;

            pushPointer = 0;
            popPointer = 0;
            dequeuePointer = 0;
        }

        public void Push()
        {

        }
        public T Dequeue()
        {
            
        }

        public T PopTop()
        {

        }

        private void Expand()
        {
            T[] transfer = new T[_innerSize*2];

            for(int i=0;i<_count;i++)
            {
                transfer[i] = data[i];
            }
            data = transfer;
            _innerSize *= 2;
        }

        private int _innerSize;
        public int Count { get => _count;  }
        public T[] data;
        private int _count;

        public int popPointer; //equals to pushPointer
        private int pushPointer;
        public int dequeuePointer;
    }
}
