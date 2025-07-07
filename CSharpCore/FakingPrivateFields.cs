#region Copyright (c) 2004-2021, Typemock     http://www.typemock.com
/************************************************************************************
'
' Copyright © 2004-2021 Typemock Ltd
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely.
'
'***********************************************************************************/
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Typemock.Examples.CSharp.FakingPrivateFields
{
    /// <summary>
    /// This test class demonstrates way to control private fields for live and next instance of the class
    /// </summary>
    [TestClass, Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after all tests in class
    public class FakingPrivateFields
    {
        [TestInitialize]
        public void Setup()
        {
            ClassUnderTestWithStatics.Init();
        }

        [TestMethod]
        public void GetPrivateField()
        {   
            //Arrange  
            var instance = new ClassUnderTest();

            //Act
            var result = Isolate.NonPublic.InstanceField(instance, "PrivateField").Value;

            //Assert
            Assert.AreEqual("Typemock", result);
        }

        [TestMethod]
        public void SetPrivateField()
        {
            //Arrange  
            var instance = new ClassUnderTest();
            Isolate.NonPublic.InstanceField(instance, "PrivateField").Value = "Typemock Rocks";

            //Act
            var result = Isolate.NonPublic.InstanceField(instance, "PrivateField").Value;

            //Assert
            Assert.AreEqual("Typemock Rocks", result);
        }

        [TestMethod]
        public void GetPrivateStaticField()
        {
            //Act
            var result = Isolate.NonPublic.StaticField<ClassUnderTestWithStatics>("StaticField").Value;

            //Assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void SetPrivateStaticField()
        {
            //Arrange
            Isolate.NonPublic.StaticField<ClassUnderTestWithStatics>("StaticField").Value = 0;

            //Act
            var result = Isolate.NonPublic.StaticField<ClassUnderTestWithStatics>("StaticField").Value;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void PrivateFieldForNextInstance()
        {
            //Arrange
            var handle = Isolate.Fake.NextInstance<ClassUnderTest>();
            var instance = new ClassUnderTest();
            Isolate.NonPublic.InstanceField(handle, "PrivateField").Value = "I'm already mocked";

            //Act           
            var result = Isolate.NonPublic.InstanceField(instance, "PrivateField").Value;

            //Assert
            Assert.AreEqual("I'm already mocked", result);
        }

        [TestMethod]
        public void PrivateFieldSetRecursiveFake()
        {
            //Arrange
            var instance = new ClassUnderTest();
            var fake = Isolate.NonPublic.InstanceField(instance, "toFake").SetRecursiveFake<ToFake>();
            Isolate.WhenCalled(() => fake.Foo()).WillReturn(1);

            //Act  
            var result = instance.CallFoo();

            //Assert
            Assert.AreEqual(1, result);
        }
    }

    //------------------
    // Classes under test
    // - ClassUnderTest: Class with private field
    // - ClassUnderTestWithStatics: Class with private static field
    //------------------

    public class ClassUnderTest
    {
        private string PrivateField;
        private ToFake toFake = null;

        public ClassUnderTest()
        {
            PrivateField = "Typemock";
        }

        public int CallFoo()
        {
            return toFake.Foo();
        }
    }

    public class ToFake
    {
        public int Foo()
        {
            throw new NotImplementedException();
        }
    }

    public class ClassUnderTestWithStatics
    {
        private static int StaticField;

        public static void Init()
        {
            StaticField = 100;
        }
    }
}
