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

namespace Typemock.Examples.CSharp.FakingPrivateConstructors
{
    /// <summary>
    /// This test class demonstrates how to create a real object with private constructor
    /// </summary>
    [TestClass, Isolated(DesignMode.Pragmatic)] // Note: Use Isolated to clean up after all tests in class
    public class FakingPrivateConstructors
    {
        [TestMethod]
        public void CreateInstanceWithPrivateCounstructor()
        {
            var fake = Isolate.NonPublic.CreateInstance<ClassUnderTest>();

            Assert.IsTrue(fake.ConstructorWasCalled);
        }
    }

    //------------------
    // Classes under test
    // - ClassUnderTest: Class with private constructor
    //------------------

    public class ClassUnderTest
    {
        public bool ConstructorWasCalled = false;

        private ClassUnderTest()
        {
            ConstructorWasCalled = true;
        }
    }
}
