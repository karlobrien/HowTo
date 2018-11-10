using System;

namespace HowTo.GarbageCollection
{
    public class UnderstandingValueType
    {
        public UnderstandingValueType()
        {

        }

        public void One(int value)
        {
            value = 10;
        }

        public void Two(ref int value)
        {
            value = 10;
        }

        public void ModifyArray(int[] items)
        {
            items[1] = 20;
        }

        public void CreateNewArray(int[] items)
        {
            items = new int[5];
            items[0] = 8;
        }

        public void WithRefArray(ref int[] items)
        {
            items = new int[5];
            for(int i = 0; i < 5; i++)
            {
                items[i] = i*10;
            }
        }

        //Produces an error as you cannot expose a ref struct to the heap
        public void LookAtRefStruct()
        {
            var obj = new MyRefStruct();

            //Console.WriteLine(obj);
        }

    }

    public class UnderstandingReferences
    {
        //read only references
        /// <summary>
        /// Prevents the method from making changes
        /// Caller has to initialise the value
        /// You can change the internals of the array but not point at another reference
        /// </summary>
        /// <param name="items"></param>
        public void RefLocalsIn(in int[] items)
        {
            items[0] = 100; //will work
            //items = new int[10]; - will not compile
        }

        public void TestIn(in long t)
        {
            //t = 10;  will not compile
        }

        /// <summary>
        /// Equivalent in the other direction
        /// Called can't modify the code
        /// </summary>
        public void RefReadonly()
        {

        }
    }

    public struct ReadObnlyArray<T>
    {
        private T[] _array;
        public ref readonly T ItemRef(int i)
        {
            return ref _array[i];
        }
    }

    public class SimpleObject
    {
        public string Name {get;set;}
        public int Age {get;set;}
    }

    public ref struct MyRefStruct
    {
        public string Name {get;set;}
        public int Age {get;set;}
    }


    public struct S
    {
        public void InstanceM() { this = new S(); }
    }

    public class C
    {
        readonly S s;

        void M() {
            F1(s); // copy
            F2(s); // no copy
        }

        void F1(S x) {
            x.InstanceM(); // no copy
            x.InstanceM(); // no copy
        }

        void F2(in S x) {
            x.InstanceM(); // copy!
            x.InstanceM(); // copy!
        }
    }
}
