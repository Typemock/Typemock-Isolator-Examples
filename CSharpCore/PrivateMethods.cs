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


namespace Typemock.Examples.CSharp.Core.PrivateMethods
{
    /// <summary>
    /// This test class shows how to fake non-public (private, protected or internal) methods, properties and indexers.
    /// 
    /// Controlling non-public members is done using the Isolate.NonPublic property, 
    /// and verifying using the Isolate.Verify.NonPublic property. 
    /// Access to non-public members is through a string of member name.
    /// 
    /// Supported behaviors are:
    /// <list type="bullet">
    ///     <item>ReturnRecursiveFake - return a zero or equivalent, and return fake objects for reference types. The returned fake objects will behave in the same way.</item>
    ///     <item>WillReturn - specify a return value for the call. Only applicable for methods returning values.</item>
    ///     <item>IgnoreCall - this method will be ignored. Only applicable for void methods.</item>
    ///     <item>WillThrow - will throw an exception when the method is called.</item>
    ///     <item>CallOriginal - will call the method's original implementation.</item>
    /// </list>
    /// </summary>

    [TestClass]

    [Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after the test
    public class PrivateMethods
    {
        [TestMethod, Isolated]
        public void PrivateMethod_ReturnRecursiveFake()
        {
            var realDependency = new Dependency();
            Isolate.NonPublic.WhenCalled(realDependency, "GetGuard").ReturnRecursiveFake<IGuard>();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.CalculateAndAlert(1, 2, realDependency);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void PrivateMethod_Return()
        {
            var realDependency = new Dependency();
            Isolate.NonPublic.WhenCalled(realDependency, "InternalNumber").WillReturn(3);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Calculate(1, 2, realDependency);

            Assert.AreEqual(6, result);

        }

        [TestMethod, Isolated]
        public void PrivateMethod_Throw()
        {
            var realDependency = new Dependency();
            Isolate.NonPublic.WhenCalled(realDependency, "InternalNumber").WillThrow(new Exception("Typemock rocks"));

            var classUnderTest = new ClassUnderTest();


            Assert.ThrowsException<Exception>(() => classUnderTest.Calculate(1, 2, realDependency));

        }

        [TestMethod, Isolated]
        public void PrivateStaticMethod_Ignore()
        {
            Isolate.NonPublic.WhenCalled<Dependency>("CallGuard").IgnoreCall();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Calculate(1, 2);

            Assert.AreEqual(3, result);
        }

        [TestMethod, Isolated]
        public void PrivateMethod_CallOriginal()
        {
            var fakeDependency = Isolate.Fake.Instance<Dependency>();
            // private works on public too
            Isolate.NonPublic.WhenCalled(fakeDependency, "GetNumberFromDatabase").CallOriginal();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Calculate(1, 2, fakeDependency);

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void PrivateProperty_Return()
        {
            var realDependency = new Dependency();
            Isolate.NonPublic.Property.WhenGetCalled(realDependency, "PrivateProp").WillReturn(3);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.CalculateFromProperty(1, 2, realDependency);

            Assert.AreEqual(6, result);

        }

        [TestMethod, Isolated]
        public void PrivateGenericMethod_Return()
        {
            Isolate.NonPublic.WhenCalled<Dependency>("PrivateCallGuardGeneric").WithGenericArguments(typeof(int)).WillReturn(3);

            var result = Dependency.CallsGuardGeneric<int>();

            Assert.AreEqual(3, result);

        }

        [TestMethod, Isolated]
        public void VerifyPrivateStaticMethodWithRef_Return()
        {
            int fakeParam = 3;
            Isolate.NonPublic.WhenCalled<Dependency>("PrivateMethodOutParam").AssignRefOut(fakeParam).IgnoreCall();

            var result = Dependency.CallPrivateMethodOutParam();

            Isolate.Verify.NonPublic.WasCalled(typeof(Dependency), "PrivateMethodOutParam");

            Assert.AreEqual(3, result);

        }


        [TestMethod, Isolated]
        public void PrivateProperty_Verified()
        {
            var realDependency = new Dependency();
            Isolate.NonPublic.Property.WhenGetCalled(realDependency, "PrivateProp").WillReturn(3);

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.CalculateFromProperty(1, 2, realDependency);
            Isolate.Verify.NonPublic.Property.WasCalledGet(realDependency, "PrivateProp");
        }

        [TestMethod, Isolated]
        public void VerifyPrivateStaticMethod_WasCalledWithAnyArg()
        {
            Isolate.NonPublic.WhenCalled<Dependency>("CallGuard").IgnoreCall();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Calculate(1, 2);

            Isolate.Verify.NonPublic.WasCalled(typeof(Dependency), "CallGuard");
        }

        [TestMethod, Isolated]
        public void VerifyPrivateStaticMethod_WasCalledWithExactArg()
        {
            Isolate.NonPublic.WhenCalled<Dependency>("CallGuard").IgnoreCall();

            var classUnderTest = new ClassUnderTest();
            var result = classUnderTest.Calculate(1, 2);

            Isolate.Verify.NonPublic.WasCalled(typeof(Dependency), "CallGuard").WithArguments("typemock", "rocks");
        }

        [TestMethod, Isolated]
        public void VerifyPrivateGenericMethod_WasCalled()
        {
            Isolate.NonPublic.WhenCalled<Dependency>("PrivateCallGuardGeneric").WithGenericArguments(typeof(int)).WillReturn(3);

            var result = Dependency.CallsGuardGeneric<int>();

            Isolate.Verify.NonPublic.WasCalled(typeof(Dependency), "PrivateCallGuardGeneric", typeof(int));
        }

        [TestMethod, Isolated]
        public void FakeMethodReturningPrivateEnum()
        {
            var instance = new ClassUnderTest();
            Isolate.NonPublic.WhenCalled(instance, "GetEnumValue").WillReturn(64); // DayOfWeek.Saturday

            var result = instance.IsWeekend();

            Assert.IsTrue(result);

        }

        [TestMethod, Isolated]
        public void PrivateLocalFunction_Return()
        {
            var real = new ClassWithPrivateLocal();
            Isolate.NonPublic.WhenCalledLocal(real, "UseLocalFunc", "GetLocal").WillReturn(5);

            var call = Isolate.Invoke.Method(real, "UseLocalFunc");

            Assert.AreEqual(5, call);
        }
    }


    //------------------
    // Classes under test
    // - Dependency: Class with private Methods that need to be faked out
    // - ClassUnderTest: Class that uses Dependency
    // - IGuard: an unimplemented interface
    // - ClassWithPrivateLocal: a Class that holds a local function inside private method.
    //------------------

    public interface IGuard
    {
        void Alert();
    }
    public class Dependency
    {
        public static void CheckSecurity(string name, string password)
        {
            CallGuard(name, password);
        }
        private static void CallGuard(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void Alert()
        {
            GetGuard("unit", "testing");
        }

        private IGuard GetGuard(string name, string password)
        {
            throw new NotImplementedException();
        }

        public int GetNumberFromDatabase()
        {
            return InternalNumber();
        }

        private int InternalNumber()
        {
            throw new NotImplementedException();
        }

        public int GetNumberFromProperty()
        {
            return PrivateProp;
        }

        private int PrivateProp { get; set; }

        private static void PrivateMethodOutParam(out int outParam)
        {
            outParam = 1;
        }

        public static int CallPrivateMethodOutParam()
        {
            int i = 0;
            PrivateMethodOutParam(out i);
            return i;
        }

        private static T PrivateCallGuardGeneric<T>()
        {
            return default(T);
        }

        public static T CallsGuardGeneric<T>()
        {
            return PrivateCallGuardGeneric<T>();
        }
    }

    public class ClassUnderTest
    {
        public int Calculate(int a, int b, Dependency dependency)
        {
            return a + b + dependency.GetNumberFromDatabase();
        }

        public int CalculateAndAlert(int a, int b, Dependency dependency)
        {
            dependency.Alert();
            return a + b;
        }

        public int Calculate(int a, int b)
        {
            Dependency.CheckSecurity("typemock", "rocks");

            return a + b;
        }

        public int CalculateFromProperty(int a, int b, Dependency dependency)
        {
            return a + b + dependency.GetNumberFromProperty();
        }

        public int CallPrivateMethodWithOut()
        {
            return Dependency.CallPrivateMethodOutParam();
        }

        public bool IsWeekend()
        {
            var value = GetEnumValue();

            if ((value & (DaysOfWeek.Sunday | DaysOfWeek.Saturday)) > 0)
                return true;

            return false;
        }

        private DaysOfWeek GetEnumValue()
        {
            throw new NotImplementedException();
        }

        [Flags]
        private enum DaysOfWeek
        {
            None = 0,
            Sunday = 1,
            Monday = 2,
            Tuesday = 4,
            Wednesday = 8,
            Thursday = 16,
            Friday = 32,
            Saturday = 64
        }
    }

    public class ClassWithPrivateLocal
    {
        public int count = 0;
        private int UseLocalFunc()
        {
            return GetLocal();
            int GetLocal()
            {
                count++;
                return 100;
            }
        }
    }
}
