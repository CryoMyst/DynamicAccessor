// ***********************************************************************
// Assembly         : DynamicAccessor.Tests
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="AccessorMethodTests.cs" company="DynamicAccessor.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamicAccessor.Extensions;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicAccessor.Tests
{
    /// <summary>
    /// Defines test class AccessorMethodTests.
    /// </summary>
    [TestClass]
    public class AccessorMethodTests
    {
        /// <summary>
        /// The default method string value
        /// </summary>
        private const string DefaultMethodStringValue = @"Some test value";

        /// <summary>
        /// Defines the test method InvokePublicMethodTest.
        /// </summary>
        [TestMethod]
        public void InvokePublicMethodTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (string) accessor.PublicMethod();

            Assert.AreEqual(DefaultMethodStringValue, value);
        }

        /// <summary>
        /// Defines the test method GetThenInvokePublicMethodTest.
        /// </summary>
        [TestMethod]
        public void GetThenInvokePublicMethodTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var method = accessor.PublicMethod;
            var value = (string) method();

            Assert.AreEqual(DefaultMethodStringValue, value);
        }

        /// <summary>
        /// Defines the test method InvokePrivateMethodTest.
        /// </summary>
        [TestMethod]
        public void InvokePrivateMethodTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (string)accessor.PrivateMethod();

            Assert.AreEqual(DefaultMethodStringValue, value);
        }

        /// <summary>
        /// Defines the test method InvokeHiddenMethodTest.
        /// </summary>
        [TestMethod]
        public void InvokeHiddenMethodTest()
        {
            // This should only invoke the first method going down the types
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (string)accessor.HiddenMethod();

            Assert.AreEqual(DefaultMethodStringValue, value);
        }

        /// <summary>
        /// Defines the test method InvokeMethodWithParametersTest.
        /// </summary>
        [TestMethod]
        public void InvokeMethodWithParametersTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (int) accessor.AddMethod(1, 2);

            Assert.AreEqual(3, value);
        }

        /// <summary>
        /// Defines the test method InvokeMethodWithDerivedParametersTest.
        /// </summary>
        [TestMethod]
        public void InvokeMethodWithDerivedParametersTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (string) accessor.GetObjectString(12345);

            Assert.AreEqual("12345", value);
        }

        /// <summary>
        /// Defines the test method InvokeMethodWithInvalidParametersTest.
        /// </summary>
        [TestMethod]
        public void InvokeMethodWithInvalidParametersTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.AddMethod("1", "2"));
        }

        /// <summary>
        /// Defines the test method InvokeMethodWithGenericsTest.
        /// </summary>
        [TestMethod]
        public void InvokeMethodWithGenericsTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var value = (int) accessor.GenericMethod<int>(1);
            Assert.AreEqual(1, value);
        }

        /// <summary>
        /// Class TestClass.
        /// Implements the <see cref="DynamicAccessor.Tests.AccessorMethodTests.TestClassBase" />
        /// </summary>
        /// <seealso cref="DynamicAccessor.Tests.AccessorMethodTests.TestClassBase" />
        class TestClass : TestClassBase
        {
            /// <summary>
            /// Publics the method.
            /// </summary>
            /// <returns>System.String.</returns>
            public string PublicMethod() => DefaultMethodStringValue;
            /// <summary>
            /// Privates the method.
            /// </summary>
            /// <returns>System.String.</returns>
            private string PrivateMethod() => DefaultMethodStringValue;
            /// <summary>
            /// Hiddens the method.
            /// </summary>
            /// <returns>System.String.</returns>
            private string HiddenMethod() => DefaultMethodStringValue;

            /// <summary>
            /// Adds the method.
            /// </summary>
            /// <param name="a">a.</param>
            /// <param name="b">The b.</param>
            /// <returns>System.Int32.</returns>
            public int AddMethod(int a, int b) => a + b;
            /// <summary>
            /// Gets the object string.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>System.String.</returns>
            public string GetObjectString(object obj) => obj.ToString();
            /// <summary>
            /// Generics the method.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="val">The value.</param>
            /// <returns>T.</returns>
            public T GenericMethod<T>(T val) => val;
        }

        /// <summary>
        /// Class TestClassBase.
        /// </summary>
        class TestClassBase
        {
            /// <summary>
            /// Bases the method.
            /// </summary>
            /// <returns>System.String.</returns>
            private string BaseMethod() => DefaultMethodStringValue;
            /// <summary>
            /// Hiddens the method.
            /// </summary>
            /// <returns>System.String.</returns>
            private string HiddenMethod() => null;
        }
    }
}
