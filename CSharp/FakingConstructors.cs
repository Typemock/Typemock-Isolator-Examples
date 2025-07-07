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
using TypeMock.ArrangeActAssert;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Typemock.Examples.CSharp.Core.FakingConstructors
{
    /// <summary>
    /// This test class demonstrates controlling arguments passed to constructor of a fake 
    /// and controlling the constructors that are called
    /// </summary>

    [TestClass]

    [Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after the test
    public class FakingConstructors
    {
        [TestMethod]
        public void CallConstructorAndPassArguments_FakeAllMethods()
        {
            // The constructor is not faked here.      
            var fake = Isolate.Fake.Instance<Dependency>
                (Members.ReturnRecursiveFakes, ConstructorWillBe.Called, 5, "Typemock");

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.GetString(fake);

            Assert.AreEqual("Typemock5", result);

        }


        [TestMethod]
        public void IgnoringOnlyConstrutor_RestOfMethodsCalled()
        {
            var fake = Isolate.Fake.Instance<Dependency>
                (Members.CallOriginal, ConstructorWillBe.Ignored);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.GetString(fake);

            Assert.AreEqual("0", result);

        }

        [TestMethod]
        public void FutureInstance_VerifyThrowingExceptionOnCreation()
        {
            // We want a memory handling exception to be thrown the next time a Dependency is instantiated
            Isolate.Swap.NextInstance<Dependency>()
                .ConstructorWillThrow(new OutOfMemoryException());

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Create(); // exception is caught and we return null

            Assert.IsNull(result);

        }

        [TestMethod]
        public void CallConstructor_FakeBaseClassConstructor()
        {
            // create an instance of Derived, but avoid calling the base class constructor
            var dependency = Isolate.Fake.Instance<Derived>(Members.CallOriginal, ConstructorWillBe.Called,
                                                            BaseConstructorWillBe.Ignored);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.GetSize(dependency);

            Assert.AreEqual(100, result);

        }

        [TestMethod, Isolated]
        public void CallConstructor_IgnoreLastBaseConstructors()
        {
            // create an instance of Derived, but avoid calling the base class constructors starting from SecondBase's constructor
            var fake = Isolate.Fake.Instance<VeryDerived>(Members.CallOriginal, ConstructorWillBe.Called, BaseConstructorWillBe.Ignored, typeof(SecondBase));

            Assert.AreEqual(0, fake.val);
            Assert.AreEqual(1, fake.firstVal);
            Assert.AreEqual(0, fake.secondVal);
            Assert.AreEqual(0, fake.mainVal);

        }


        [TestMethod]
        public void CallConstructor_DoCustomCode()
        {
            var handle = Isolate.Fake.NextInstance<Dependency>(Members.ReturnRecursiveFakes, context =>
            {
                if ((string)context.Parameters[1] == "typemock")
                {
                    // contructor is faked
                    return;
                }
                context.WillCallOriginal(); // constructor will be called
            });

            var classUnderTest = new ClassUnderTest();
            var dependency = classUnderTest.Create("typemock");

            var result = classUnderTest.GetString(dependency);

            Assert.AreEqual("0", result);

        }
    }

    //------------------
    // Classes under test
    // - Dependency: Class with Methods that need to be faked out
    // - ClassUnderTest: Class that creates and uses Dependency
    // - Base and Derived: Class Hierarchy but Base still needs to implement its constructor
    //------------------

    public class Dependency
    {
        public int Age;
        public string Name;
        public Dependency(int age, string name)
        {
            Age = age;
            Name = name;
        }

        public virtual void DoSomething()
        {
            throw new NotImplementedException();
        }
    }

    public class Base
    {
        public Base()
        {
            throw new NotImplementedException();
        }

        public virtual int Size { get; set; }
    }

    public class Derived : Base
    {
        public Derived()
        {
            Size = 100;
        }
    }

    public class ClassUnderTest
    {
        public string GetString(Dependency user)
        {
            return user.Name + user.Age.ToString();

        }

        public Dependency Create()
        {
            try
            {
                return new Dependency(0, "");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dependency Create(string name)
        {
            return new Dependency(0, name);
        }

        public int GetSize(Derived derived)
        {
            return derived.Size;
        }
    }

    public class MainBase
    {
        public int mainVal;
        public MainBase(int val)
        {
            mainVal = val;
        }
    }

    public class SecondBase : MainBase
    {
        public int secondVal;

        public SecondBase(int val) : base(val + 1)
        {
            secondVal = val;
        }
    }

    public class FirstBase : SecondBase
    {
        public int firstVal;

        public FirstBase(int val) : base(val + 1)
        {
            firstVal = val;
        }
    }

    public class VeryDerived : FirstBase
    {
        public int val;
        VeryDerived(int val) : base(val + 1)
        {
            this.val = val;
        }
    }
}