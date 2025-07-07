#region Copyright (c) 2004-2021, Typemock     http://www.typemock.com
/************************************************************************************
                 
 Copyright © 2004-2021 Typemock Ltd

 This code is protected by international laws

 ***********************************************************************************/
#endregion
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdPartyProject;
using TypeMock.ArrangeActAssert;

namespace Typemock.Examples.CSharp
{
    [TestClass, Isolated]
    public class FakingNotVisibleInstances
    {
        [TestMethod, Isolated]
        public void FakingInstanceOfNotVisibleType()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.NotVisibleTypeToFake");
            var dependancy = Isolate.NonPublic.Fake.Instance(internalType);
            var instance = new VisibleType();

            //Act
            var result = (double)Isolate.Invoke.Method(instance, "Calculate", 0.1, 0.2, dependancy);

            //Assert
            Assert.AreEqual(0.3, result, 0.1);
        }

        [TestMethod, Isolated]
        public void FakingFutureInstanceOfNotVisibleType()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.NotVisibleTypeToFake");
            var handle = Isolate.NonPublic.Fake.NextInstance(internalType);

            //Act
            var result = new VisibleType().Calculate(5);

            //Assert
            Assert.AreEqual(25, result);
        }

        [TestMethod, Isolated]
        public void FakingAllInstanceOfNotVisibleType()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.AnotherNotVisibleTypeToFake");
            var handle = Isolate.NonPublic.Fake.AllInstances(internalType);

            //Act
            var result = new VisibleType().Calculate(0.5);

            //Assert
            Assert.AreEqual(0.25, result);
        }

        [TestMethod, Isolated]
        public void FakingAllInstanceOfNotVisibleType_Singletons()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.InternalSingleton");
            var handle = Isolate.NonPublic.Fake.AllInstances(internalType);
            Isolate.NonPublic.WhenCalled(handle, "ReturnZero").WillReturn(10);

            //Act
            var result = new VisibleType().CalculateWithSingleton(1);

            //Assert
            Assert.AreEqual(11, result);
        }

        [TestMethod, Isolated]
        public void ReturningRecursiveFakeOfNotVisibleType()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.NotVisibleTypeToFake");
            var instance = new VisibleType();
            Isolate.NonPublic.WhenCalled(instance, "GetInternalInstance").ReturnRecursiveFake(internalType);
            
            //Act
            var result = instance.Calculate(1, 2);

            //Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod, Isolated]
        public void CreateInstanceOfNotVisibleType()
        {
            //Arrange
            var internalType = typeof(VisibleType).Assembly.GetType("ThirdPartyProject.NotVisibleTypeToFake");
            var dependancy = Isolate.NonPublic.CreateInstance(internalType);
            var instance = new VisibleType();

            //Act
            var result = Isolate.Invoke.Method(instance, "Calculate", 1, 2, dependancy );

            //Assert
            Assert.AreEqual(6, result);
        }
    }
}
