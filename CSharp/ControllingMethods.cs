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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TypeMock.ArrangeActAssert;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;




namespace Typemock.Examples.CSharp.Core.Methods
{
    /// <summary>
    /// This test class shows different ways of controlling the behavior of fake objects using the Isolate.WhenCalled() API.
    /// The supported behaviors are:
    /// <list type="bullet">
    ///     <item>ReturnRecursiveFake (default)- return a zero or equivalent, and return fake objects for reference types. The returned fake objects will behave in the same way.</item>
    ///     <item>WillReturn - specify a return value for the call. Only applicable for methods returning values.</item>
    ///     <item>IgnoreCall - this method will be ignored. Only applicable for void methods.</item>
    ///     <item>WillThrow - will throw an exception when the method is called.</item>
    ///     <item>CallOriginal - will call the method's original implementation.</item>
    ///     <item>WillReturnCollectionValuesOf - will replace the collection returned by the method with a provided one. Only applies to methods returning collections</item>
    /// </list>
    /// </summary>

    [TestClass]

    [Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after the test
    public class ControllingMethodBehavior
    {
        [TestMethod, Isolated]
        public void ReturnRecursiveFake()
        {
            var realDependency = new Dependency();

            // return fake objects for reference types
            Isolate.WhenCalled(() => realDependency.GetParent()).ReturnRecursiveFake();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.ReturnParentName(realDependency);

            Assert.AreEqual("", result);

        }

        [TestMethod, Isolated]
        public void WillReturn_ReturnValue()
        {
            var realDependency = new Dependency();
            Isolate.WhenCalled(() => realDependency.GetID()).WillReturn(2);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.AddToDependency(1, realDependency);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void CallOriginal_OnFakeObject()
        {
            var fakeDependency = Isolate.Fake.Instance<Dependency>();

            Isolate.WhenCalled(() => fakeDependency.GetID()).CallOriginal();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.AddToDependency(1, fakeDependency);

            // original GetID returns 10

            Assert.AreEqual(11, result);

        }

        [TestMethod, Isolated]
        public void IgnoreCall_OnRealObject()
        {
            var realDependency = new Dependency();
            Isolate.WhenCalled(() => realDependency.Check()).IgnoreCall();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.GetIDWithCheck(realDependency);

            Assert.AreEqual(10, result);

        }

        [TestMethod, Isolated]
        public void ThrowException_OnRealObject()
        {
            var realDependency = new Dependency();
            Isolate.WhenCalled(() => realDependency.GetID())
                .WillThrow(new Exception("fakes fault"));

            var classUnderTest = new ClassUnderTest();

            Assert.ThrowsException<Exception>(() => classUnderTest.AddToDependency(1, realDependency));

        }

        [TestMethod, Isolated]
        public void DoInstead_OnRealObject()
        {
            var returnValue = 2;

            var realDependency = new Dependency();

            // return value dynamically
            Isolate.WhenCalled(() => realDependency.GetID()).DoInstead(
                x => { return returnValue; });

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.AddToDependency(1, realDependency);

            Assert.AreEqual(3, result);


            returnValue = 4;
            result = classUnderTest.AddToDependency(1, realDependency);

            Assert.AreEqual(5, result);

        }

        [TestMethod, Isolated]
        public void DoInstead_WithRecursion()
        {
            var dependency = new Dependency();
            var classForRecursion = new ClassForRecursion(dependency);

            Isolate.WhenCalled(() => dependency.DoStuff(0)).DoInstead(
                context =>
                {
                    int passedVal = (int)context.Parameters[0];
                    context.ContinueFakingThisMethodRecursively = true;
                    classForRecursion.Foo(passedVal + 1);
                }
            );

            classForRecursion.Foo(1);

            Assert.AreEqual(5, classForRecursion.count);

        }

        [TestMethod, Isolated]
        public void SequencedWillReturn_OnRealObject()
        {
            var realDependency = new Dependency();

            // Sequenced calls will return values in sequence, 
            // last value will stay the default
            Isolate.WhenCalled(() => realDependency.GetID()).WillReturn(2);
            Isolate.WhenCalled(() => realDependency.GetID()).WillReturn(9);

            var result = new ClassUnderTest().AddToDependency3Times(1, realDependency);

            Assert.AreEqual(21, result);

        }

        [TestMethod, Isolated]
        public void OverloadedMethodConsideredSequenced_OnFakeObject()
        {
            var realDependency = Isolate.Fake.Instance<Dependency>();

            // Overloaded method calls without using exact argument matching
            // are considered sequenced calls
            Isolate.WhenCalled(() => realDependency.OverloadedMethod(1)).WillReturn(2);
            Isolate.WhenCalled(() => realDependency.OverloadedMethod("Typemock Rocks")).WillReturn(9);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.CallTwoOverloadedDependency(realDependency);

            Assert.AreEqual(11, result);

        }

        [TestMethod, Isolated]
        public void SequencedOverloadedByType_OnRealObject()
        {
            var realDependency = new Dependency();

            // Each overloaded method will act as a separate sequence
            Isolate.WhenCalled(() => realDependency.OverloadedMethod(1)).WillReturn(2);
            Isolate.WhenCalled(() => realDependency.OverloadedMethod(1)).WillReturn(4);
            Isolate.WhenCalled(() => realDependency.OverloadedMethod("Typemock Rocks")).WillReturn(9);
            Isolate.WhenCalled(() => realDependency.OverloadedMethod("Typemock Rocks")).WillReturn(10);

            var classUnderTest = new ClassUnderTest();

            var result = classUnderTest.CallTwoOverloadedDependency(realDependency);

            Assert.AreEqual(11, result);


            result = classUnderTest.CallTwoOverloadedDependency(realDependency);

            Assert.AreEqual(14, result);

        }

        [TestMethod, Isolated]
        public void SettingBehaviorForCallChain_OnRealObject()
        {
            var fakeDependency = new Dependency();

            Isolate.WhenCalled(() => fakeDependency.GetParent().GetID()).WillReturn(2);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.AddToChainedDependency(1, fakeDependency);

            Assert.AreEqual(3, result);

        }


        [TestMethod, Isolated] // Note: Use Isolated to clean up after the test
        public void ExtensionMethod_Example()
        {
            Dependency dependency = new Dependency();
            // Call the extension method as normal (even though it is actually a static method)
            Isolate.WhenCalled(() => dependency.Multiply(6)).WillReturn(10);

            var cut = new ClassUnderTest();

            var result = cut.AddToDependency(0, dependency);
            // Verify the returned values

            Assert.AreEqual(10, result);

        }

        [TestMethod, Isolated] // Note: Use Isolated to clean up after the test
        public void MockLinqQuery_Example()
        {
            List<int> realList = new List<int> { 1, 2, 4, 5 };
            var dummyData = new int[] { 10, 20 };

            Isolate.WhenCalled(() => from c in realList where c > 3 select c).WillReturn(dummyData);

            var result = new ClassUnderTest().DoLinq(realList);

            // Note: Returns dummyData results

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(10, result[0]);
            Assert.AreEqual(20, result[1]);


        }

        [TestMethod, Isolated] // Note: This could be any collection from mscorlib which implements IEnumerable and has default ctor
        public void MethodReturnsMscorlibRealEmptyCollection()
        {
            ClassWithCollection fakeCollection = Isolate.Fake.Instance<ClassWithCollection>();

            Hashtable hashtable = fakeCollection.ReturnIEnumrableFromMscorlib();

            Assert.IsNotNull(hashtable);
            Assert.AreEqual(0, hashtable.Count);

        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Beep(uint dwFreq, uint dwDuration);
        [TestMethod, Isolated]
        public void PInvokeMethod_Example()
        {
            Isolate.WhenCalled(() => Beep(0, 0)).WillReturn(false);
            var result = Beep(100, 200);

            Assert.IsFalse(result);

        }

        [TestMethod, Isolated]
        public void LocalFunction_WillReturn_ReturnValue()
        {
            var fake = Isolate.Fake.Instance<ClassWithLocal>(Members.CallOriginal);
            Isolate.NonPublic.WhenCalledLocal(fake, "UseLocalFunc", "GetLocal").WillReturn(5);


            var result = fake.UseLocalFunc();


            Assert.AreEqual(5, result);
        }

        [TestMethod, Isolated]
        public void LocalFunction_CallOriginal_OnFakeObject()
        {

            var fake = Isolate.Fake.Instance<ClassWithLocal>(Members.CallOriginal);
            Isolate.NonPublic.WhenCalledLocal(fake, "UseLocalFunc", "GetLocal").CallOriginal();

            var publicResult = fake.UseLocalFunc();

            Assert.AreEqual(100, publicResult);
        }

        [TestMethod, Isolated]
        public void LocalFunction_IgnoreCall_OnFakeObject()
        {
            var program = Isolate.Fake.Instance<ClassWithLocal>(Members.CallOriginal);
            Isolate.NonPublic.WhenCalledLocal(program, "UseVoidLocal", "VoidLocal").IgnoreCall();

            program.UseVoidLocal();

            Assert.AreEqual(0, program.count);

        }

        [TestMethod, Isolated]
        public void LocalFunctionThrowException_OnRealObject()
        {
            var real = new ClassWithLocal();
            Isolate.NonPublic.WhenCalledLocal(real, "UseLocalFunc", "GetLocal").WillThrow(new Exception("fakes fault"));

            Assert.ThrowsException<Exception>(() => real.UseLocalFunc());
        }


        [TestMethod, Isolated]
        public void LocalFunciton_DoInstead_OnRealObject()
        {

            var program = new ClassWithLocal();
            var wasCalled = false;
            Isolate.NonPublic.WhenCalledLocal(program, "UseLocalFunc", "GetLocal")
                .DoInstead(testContext => {
                    wasCalled = true;
                    return 0;
                });


            var called = program.UseLocalFunc();

            Assert.IsTrue(wasCalled);
            Assert.AreEqual(called, 0);

        }

    }

    //------------------
    // Classes under test
    // - ExtendDependency - an extension method of Dependency
    // - Dependency: Methods are not implemented - these need to be faked out
    // - ClassUnderTest: Class that uses Dependency
    // - ClassWithLocal: a Class that holds a local function inside public method.
    //------------------

    public static class ExtendDependency
    {
        public static int Multiply(this Dependency extendedInstance, int scalar)
        {
            return extendedInstance.GetID() * scalar;
        }

    }

    public class Dependency
    {
        public virtual void DoStuff(int i)
        {
            throw new NotImplementedException();
        }

        public virtual void Check()
        {
            throw new NotImplementedException();
        }

        public virtual Dependency GetParent()
        {
            throw new NotImplementedException();
        }

        public virtual int GetID()
        {
            return 10;
        }

        public virtual int OverloadedMethod(int arg)
        {
            return 10;
        }
        public virtual int OverloadedMethod(string arg)
        {
            return 10;
        }

        public virtual string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public class ClassUnderTest
    {
        public int SimpleCalculation(int a, int b, Dependency dependency)
        {
            dependency.Check();

            return a + b;
        }

        public string ReturnParentName(Dependency dependency)
        {
            var parent = dependency.GetParent();
            return parent.Name;

        }

        public int GetIDWithCheck(Dependency dependency)
        {
            dependency.Check();

            return dependency.GetID();
        }

        public int AddToDependency(int a, Dependency dependency)
        {
            return a + dependency.GetID();
        }

        public int AddToDependency3Times(int a, Dependency dependency)
        {
            return a + dependency.GetID() + dependency.GetID() + dependency.GetID();
        }

        public int CallTwoOverloadedDependency(Dependency dependency)
        {
            return dependency.OverloadedMethod(12) + dependency.OverloadedMethod("typemock");
        }

        public int AddToChainedDependency(int a, Dependency dependency)
        {
            return a + dependency.GetParent().GetID();
        }

        public List<int> DoLinq(List<int> list)
        {
            return (from c in list where c > 3 select c).ToList();
        }
    }

    public class ClassWithCollection
    {
        public Hashtable ReturnIEnumrableFromMscorlib()
        {
            return null;
        }

        public List<int> ReturnListFromMscorlib()
        {
            return null;
        }
    }

    public class ClassForRecursion
    {
        public int count = 0;
        private Dependency dependency;

        public ClassForRecursion(Dependency dependency)
        {
            this.dependency = dependency;
        }

        public void Foo(int i)
        {
            count++;
            if (i < 5)
            {
                dependency.DoStuff(i);
            }
        }
    }

    public class ClassWithLocal
    {
        public int count = 0;
        public int UseLocalFunc()
        {
            return GetLocal();
            int GetLocal()
            {
                count++;
                return 100;

            }
        }
        public void UseVoidLocal()
        {
            VoidLocal();
            void VoidLocal()
            {
                count++;
            }
        }
    }
}
