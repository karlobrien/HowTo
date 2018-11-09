using System;
using Xunit;
using HowTo.GarbageCollection;

namespace HowTo.Gc.Tests
{
    public class ValueTypesTests
    {
        [Fact]
        public void ValueIsUnchanged()
        {
            var uvt = new UnderstandingValueType();
            int val = 1;
            uvt.One(val);
            Assert.True(val == 1);

            uvt.Two(ref val);
            Assert.True(val == 10);

            int[] items = {1, 2, 3, 4, 5};
            uvt.ModifyArray(items);
            Assert.True(items[1] == 20);

            uvt.CreateNewArray(items);
            Assert.True(items[0] == 1);

            uvt.WithRefArray(ref items);
            Assert.True(items[1] == 10);
        }
    }

    public class RefTypesTests
    {
        [Fact]
        public void RefLocalTest()
        {
            var item = new SimpleObject{ Name = "Test", Age=30};
            ref SimpleObject copied = ref item;

            Assert.True(item.Name == copied.Name);

            copied = new SimpleObject { Name = "Change", Age=80};
            Assert.True(item.Name == copied.Name);
        }
    }
}
