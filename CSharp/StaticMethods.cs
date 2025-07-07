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
using System.Threading;
using TypeMock.ArrangeActAssert;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Typemock.Examples.CSharp.Desktop.StaticMethods
{
    /// <summary>
    /// This test class shows how to fake static methods. 
    /// </summary>

    [TestClass]

    [Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after the test
    public class StaticMethodsAndConstructors
    {
        [TestMethod, Isolated]
        public void FakeAllStaticMethods()
        {
            Isolate.Fake.StaticMethods<Dependency>();

            var result = new ClassUnderTest().Calculate(1, 2);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void FakeOneStaticMethod()
        {
            Isolate.WhenCalled(() => Dependency.CheckSecurity(null, null)).IgnoreCall();

            var result = new ClassUnderTest().Calculate(1, 2);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void VerifyStaticMethodWasCalled()
        {
            Isolate.Fake.StaticMethods<Dependency>();

            var result = new ClassUnderTest().Calculate(1, 2);
            Isolate.Verify.WasCalledWithAnyArguments(() => Dependency.CheckSecurity(null, null));
        }

        /// <summary>
        /// This test shows to to fake calls to static constructors using Isolate.Fake.StaticConstructor().
        /// By default static constructors are called to fake them use Fake.StaticConstructor()
        /// </summary>
        [TestMethod, Isolated]
        public void FakingStaticConstructor()
        {
            StaticConstructorExample.TrueOnStaticConstructor = false;
            Isolate.Fake.StaticConstructor<StaticConstructorExample>();

            // calling a static method on the class forces the static constructor to be called
            StaticConstructorExample.Foo();

            // this verifies the static constructor was faked and not called

            Assert.IsFalse(StaticConstructorExample.TrueOnStaticConstructor);

        }

        /// <summary>
        /// As static constructors for a type is only executed once, if we fake it we need a way to invoke it in a test that 
        /// requires normal execution.
        /// 
        /// Typemock Isolator does this automatically, but here is a way to force a static-constructor call
        /// </summary>
        [TestMethod, Isolated]
        public void CallingStaticConstructorTest()
        {
            StaticConstructorExample.TrueOnStaticConstructor = false;

            // force static constructor to be called
            Isolate.Invoke.StaticConstructor(typeof(StaticConstructorExample));

            Assert.IsTrue(StaticConstructorExample.TrueOnStaticConstructor);

        }

        [TestMethod, Isolated]
        public void CallingStaticConstructorTest2()
        {
            var x = Isolate.Fake.AllInstances<ClassUnderTest>();

            x.Calculate(1, 1);
        }


        [TestMethod, Isolated]
        public void StaticLocalFunction_Return()
        {
            var real = new ClassWithStaticLocal();
            Isolate.NonPublic.WhenCalledLocal<ClassWithStaticLocal>("UseLocalFunc", "GetLocal").WillReturn(5);

            var call = ClassWithStaticLocal.UseLocalFunc();

            Assert.AreEqual(5, call);
        }

        [TestMethod, Isolated]
        public void StaticLocalFunction_CallOriginal()
        {
            var real = new ClassWithStaticLocal();

            Isolate.NonPublic.WhenCalledLocal<ClassWithStaticLocal>("UseLocalFunc", "GetLocal").CallOriginal();

            var privateResult = ClassWithStaticLocal.UseLocalFunc();

            Assert.AreEqual(100, privateResult);
        }

        [TestMethod, Isolated]
        public void StaticLocalFunction_IgnoreCall()
        {
            var real = new ClassWithStaticLocal();
            Isolate.NonPublic.WhenCalledLocal<ClassWithStaticLocal>("UseVoidLocal", "VoidLocal").IgnoreCall();

            ClassWithStaticLocal.UseVoidLocal();

            Assert.AreEqual(0, ClassWithStaticLocal.count);
        }

    }



    //------------------
    // Classes under test
    // - Dependency: Methods are not implemented - these need to be faked out
    // - ClassUnderTest: Class that uses Dependency
    // - StaticConstructorExample: a class with a static constructor and a flag that indicates it was called.
    // - ClassWithStaticLocal: a Class that holds a local function inside static method.
    //------------------

    public class StaticConstructorExample
    {
        private static bool trueOnStaticConstructor = false;
        public static bool TrueOnStaticConstructor
        {
            get { return trueOnStaticConstructor; }
            set { trueOnStaticConstructor = value; }
        }

        static StaticConstructorExample()
        {
            trueOnStaticConstructor = true;
        }

        public static void Foo() { }
    }

    public class Dependency
    {
        public static void CheckSecurity(string name, string password)
        {
            throw new NotImplementedException();
        }

        public static void CallGuard()
        {
            throw new NotImplementedException();
        }
    }

    public class ClassUnderTest
    {
        public int Calculate(int a, int b)
        {
            Dependency.CheckSecurity("typemock", "rules");

            return a + b;
        }
    }
    public class ClassWithStaticLocal
    {
        public static int count = 0;
        public static int UseLocalFunc()
        {
            count = 0;
            return GetLocal();
            int GetLocal()
            {
                count =1;
                return 100;
            }
        }
        public static void UseVoidLocal()
        {
            count = 0;
            VoidLocal();
            void VoidLocal()
            {
                count = 1;
            }
        }
    }
}
