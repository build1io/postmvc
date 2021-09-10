using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands
{
    internal sealed class CommandPoolTests
    {
        private ICommandPool _pool;

        [SetUp]
        public void SetUp()
        {
            _pool = new CommandPool();
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
            Assert.That(() => _pool.TakeCommand(null), Throws.Exception);
        }
        
        [Test]
        public void ReturnNullTest()
        {
            Assert.DoesNotThrow(() => _pool.ReturnCommand(null));
        }

        [Test]
        public void NewInstanceTest()
        {
            Assert.IsNotNull(_pool.TakeCommand<Command01>(out var isNewInstance));
            Assert.IsTrue(isNewInstance);
        }

        [Test]
        public void ReturnedInstanceTest()
        {
            var command = _pool.TakeCommand<Command01>();
            _pool.ReturnCommand(command);
            
            command = _pool.TakeCommand<Command01>(out var isNewInstance);
            Assert.IsNotNull(command);
            Assert.IsFalse(isNewInstance);
        }

        [Test]
        public void CountersTest()
        {
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            var command = _pool.TakeCommand<Command01>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            
            _pool.ReturnCommand(command);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            command = _pool.TakeCommand<Command01>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void InstanceIdentityTest()
        {
            var command01 = _pool.TakeCommand<Command01>();
            _pool.ReturnCommand(command01);
            var command02 = _pool.TakeCommand<Command01>();
            Assert.AreEqual(command01, command02);
        }
        
        [Test]
        public void MultipleReturnsTest()
        {
            var command = _pool.TakeCommand<Command01>();
            _pool.ReturnCommand(command);
            _pool.ReturnCommand(command);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void OutsideInstanceReturnTest()
        {
            _pool.ReturnCommand(new Command01());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }
        
        [Test]
        public void SameTypeOutsideInstanceReturnTest()
        {
            var instance02 = new Command01();
            
            _pool.ReturnCommand(instance02);
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            
            var instance01 = _pool.TakeCommand<Command01>();

            _pool.ReturnCommand(instance02);
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            
            _pool.ReturnCommand(instance01);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
        }

        [Test]
        public void MultipleTypesTest()
        {
            var instance01 = _pool.TakeCommand<Command01>();
            var instance02 = _pool.TakeCommand<Command02>();
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command02>());
            
            _pool.ReturnCommand(instance01);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(1, _pool.GetUsedInstancesCount<Command02>());
            
            _pool.ReturnCommand(instance02);
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command01>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command01>());
            Assert.AreEqual(1, _pool.GetAvailableInstancesCount<Command02>());
            Assert.AreEqual(0, _pool.GetUsedInstancesCount<Command02>());
        }
    }
}