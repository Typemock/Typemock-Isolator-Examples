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
using System.Reflection;
using TypeMock.ArrangeActAssert;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Typemock.Examples.CSharp.Core.InvokingMethods
{
    /// <summary>
    /// This class demonstrates the ability of firing events and invoking private methods using Isolator.
    /// </summary>

    [TestClass]

    [Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after the test
    public class InvokingMethods
    {
        [TestMethod]
        [Isolated]
        public void FireEvent_RunEvent()
        {
            var underTest = new ClassUnderTest();
            var counter = new Counter(underTest);

            // Note how adding a dummy event is the way to fire it
            Isolate.Invoke.Event(() => underTest.RunEvent += null, 0);

            Assert.AreEqual(1, counter.Times);

        }

        [TestMethod, Isolated]
        public void InvokePrivateMethod()
        {
            var underTest = new ClassUnderTest();

            var result = Isolate.Invoke.Method(underTest, "Sum", 2, 5);

            Assert.AreEqual(7, result);

        }

        [TestMethod, Isolated]
        public void InvokeBaseMethod()
        {
            var underTest = new DerivedClass();

            var result = Isolate.Invoke.MethodFromBase<ClassUnderTest>(underTest, "Subtract", 5, 2);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void InvokePrivateStaticMethod()
        {
            var result = Isolate.Invoke.Method<ClassUnderTest>("Multiply", 2, 5);

            Assert.AreEqual(10, result);

        }

        [TestMethod, Isolated]
        public void InvokePrivateMethodWithRef()
        {
            var byRef = Args.Ref(5);
            Isolate.Invoke.Method<ClassUnderTest>("MultiplyByRef", byRef, 2);

            var result = byRef.Value;

            Assert.AreEqual(10, result);

        }

        [TestMethod, Isolated]
        public void InvokePrivateMethodWithNull()
        {
            var result = Isolate.Invoke.Method<ClassUnderTest>("MultiplyNullable", 2, Args.Null<int?>());

            Assert.AreEqual(0, result);

        }

        [TestMethod, Isolated]
        public void InvokeStaticLocalFunction()
        {
            var result = Isolate.Invoke.LocalFunction<ClassWithLocal>("UseStaticLocalFunc", "GetLocal", 5);

            Assert.AreEqual(5, result);

        }

        [TestMethod, Isolated]
        public void InvokeLocalFunction()
        {
            var fake = Isolate.Fake.Instance<ClassWithLocal>(Members.CallOriginal);
            var result = Isolate.Invoke.LocalFunction(fake, "UseLocalFunc", "GetLocal", 5);

            Assert.AreEqual(5, result);

        }
    }

    //------------------
    // Classes under test
    // - Dependency: Class with Methods that need to be faked out
    // - ClassUnderTest: Class that creates and uses Dependency
    // - Counter: A Class that registers to our ClassUnderTest events
    // - ClassWithLocal: A class that uses local functions.
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
    }


    public class Counter
    {
        public int Times { get; set; }
        public Counter(ClassUnderTest underTest)
        {
            underTest.RunEvent += new Action<int>(underTest_RunEvent);
        }

        public void underTest_RunEvent(int obj)
        {
            Times++;
        }
    }
	#pragma warning disable CS0067


    public class ClassUnderTest
    {
        public event Action<int> RunEvent;

        public ClassUnderTest() { }

        private int Sum(int a, int b)
        {
            return a + b;
        }

        private static int Multiply(int a, int b)
        {
            return a * b;
        }

        private static void MultiplyByRef(ref int a, int b)
        {
            a = a * b;
        }

        private static int MultiplyNullable(int a, int? b)
        {
            if (!b.HasValue)
            {
                return 0;
            }
            return a = a * b.Value;
        }

        protected virtual int Subtract(int a, int b)
        {
            return a - b;
        }
		
		protected void SubtractAndRun(int a, int b)
		{
			RunEvent(Subtract(a,-b));
		}
    }

    public class DerivedClass : ClassUnderTest
    {
        protected override int Subtract(int a, int b)
        {
            return base.Subtract(a, b) * 2;
        }
    }
    public class ClassWithLocal
    {
        int count = 0;
        public int UseLocalFunc(int a)
        {
           
            return GetLocal(a);

            int GetLocal(int b)
            {
                count++;
                return b;
            }
        }

        static int UseStaticLocalFunc(int a)
        {
            return GetLocal(a);
            int GetLocal(int b)
            {
                return b;
            }
        }
    }
}
