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

    }
}
