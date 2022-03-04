using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands;
using Build1.PostMVC.Utils.Pooling;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands
{
    internal sealed class CommandPoolTests
    {
        private Pool<CommandBase> _pool;

        [SetUp]
        public void SetUp()
        {
            _pool = new Pool<CommandBase>();
        }
        
        [Test]
        public void NullTypeTest()
        {
            Assert.That(() => _pool.GetAvailableInstancesCount(null), Throws.Exception);
            Assert.That(() => _pool.GetUsedInstancesCount(null), Throws.Exception);
        }
        
        [Test]
        public void EmptyCountersTest()
        {
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }
        
        [Test]
        public void TakeNullTest()
        {
            Assert.That(() => _pool.Take(null), Throws.Exception);
        }
        
        [Test]
        public void ReturnNullTest()
        {
            Assert.DoesNotThrow(() => _pool.Return(null));
        }

        [Test]
        public void NewInstanceTest()
        {
            Assert.IsNotNull(_pool.Take<Command01>(out var isNewInstance));
            Assert.IsTrue(isNewInstance);
        }

        [Test]
        public void ReturnedInstanceTest()
        {
            var command = _pool.Take<Command01>();
            _pool.Return(command);
            
            command = _pool.Take<Command01>(out var isNewInstance);
            Assert.IsNotNull(command);
            Assert.IsFalse(isNewInstance);
        }

        [Test]
        public void CountersTest()
        {
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            var command = _pool.Take<Command01>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            
            _pool.Return(command);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            command = _pool.Take<Command01>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void InstanceIdentityTest()
        {
            var command01 = _pool.Take<Command01>();
            _pool.Return(command01);
            var command02 = _pool.Take<Command01>();
            Assert.AreEqual(command01, command02);
        }
        
        [Test]
        public void MultipleReturnsTest()
        {
            var command = _pool.Take<Command01>();
            _pool.Return(command);
            _pool.Return(command);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void OutsideInstanceReturnTest()
        {
            _pool.Return(new Command01());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }
        
        [Test]
        public void SameTypeOutsideInstanceReturnTest()
        {
            var instance02 = new Command01();
            
            _pool.Return(instance02);
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            var instance01 = _pool.Take<Command01>();

            _pool.Return(instance02);
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            
            _pool.Return(instance01);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void MultipleTypesTest()
        {
            var instance01 = _pool.Take<Command01>();
            var instance02 = _pool.Take<Command02>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command02>());
            
            _pool.Return(instance01);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command02>());
            
            _pool.Return(instance02);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command02>());
        }
    }
}