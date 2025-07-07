#region Copyright (c) 2004-2018, Typemock     http://www.typemock.com
/************************************************************************************
                 
 Copyright © 2004-2018 Typemock Ltd

 This code is protected by international laws

 ***********************************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThirdPartyProject
{

    public class VisibleType
    {
        private AnotherNotVisibleTypeToFake internalField = new AnotherNotVisibleTypeToFake();

        public int Calculate(int i)
        {
            new NotVisibleTypeToFake().Check();

            return i * i;
        }

        public double Calculate(double i)
        {
            internalField.Check();

            return i * i;
        }

        public double CalculateWithSingleton(long i)
        {
            return InternalSingleton.Instance.ReturnZero() + i;
        }

        public int Calculate(int a, int b)
        {
            var instance = GetInternalInstance();
            instance.Check();
            return a + b;
        }

        private int Calculate(int a, int b, NotVisibleTypeToFake dependancy)
        {
            return a + b + dependancy.Id;
        }

        private double Calculate(double a, double b, NotVisibleTypeToFake dependancy)
        {
            dependancy.Check();
            return a + b;
        }

        private NotVisibleTypeToFake GetInternalInstance()
        {
            throw new NotImplementedException();
        }
    }

    internal class NotVisibleTypeToFake
    {
        public void Check()
        {
            throw new Exception("No Entry");
        }

        public int Id { get { return 3; } }
    }

    internal class AnotherNotVisibleTypeToFake
    {
        public void Check()
        {
            throw new Exception("No Entry");
        }
    }

    internal class InternalSingleton
    {
        private InternalSingleton() { }
        static readonly InternalSingleton instance = new InternalSingleton();

        public static InternalSingleton Instance { get { return instance; } }

        public int ReturnZero()
        {
            return 0;
        }
    }
}
