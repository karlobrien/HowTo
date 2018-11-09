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

        /// <summary>
        /// Prevents the method from making changes
        /// Caller has to initialise the value
        /// You can change the internals of the array but not point at another reference
        /// </summary>
        /// <param name="items"></param>
        public void RefLocalsIn(in int[] items)
        {
            items[0] = 100; //will not work
            //items = new int[10]; - will not compile
        }

        public void TestIn(in long t)
        {
            //t = 10;  will not compile
        }

    }

    public class SimpleObject
    {
        public string Name {get;set;}
        public int Age {get;set;}
    }
}
