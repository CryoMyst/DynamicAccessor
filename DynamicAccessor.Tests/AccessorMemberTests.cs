// ***********************************************************************
// Assembly         : DynamicAccessor.Tests
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="AccessorMemberTests.cs" company="DynamicAccessor.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicAccessor.Tests
{
    /// <summary>
    /// Defines test class AccessorMemberTests.
    /// </summary>
    [TestClass]
    public class AccessorMemberTests
    {
        /// <summary>
        /// The public field string value
        /// </summary>
        private const string PublicFieldStringValue = "This is a public field";
        /// <summary>
        /// The private field string value
        /// </summary>
        private const string PrivateFieldStringValue = "This is a private field";
        /// <summary>
        /// The static field string value
        /// </summary>
        private const string StaticFieldStringValue = "This is a static field";

        /// <summary>
        /// The public property string value
        /// </summary>
        private const string PublicPropertyStringValue = "This is a public Property";
        /// <summary>
        /// The private property string value
        /// </summary>
        private const string PrivatePropertyStringValue = "This is a private Property";
        /// <summary>
        /// The static property string value
        /// </summary>
        private const string StaticPropertyStringValue = "This is a static property";

        /// <summary>
        /// Defines the test method AccessPublicFieldTest.
        /// </summary>
        [TestMethod]
        public void AccessPublicFieldTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var publicField = accessor.PublicField;
            var publicFieldValue = (string) publicField;

            Assert.IsNotNull(publicField);
            Assert.AreEqual(PublicFieldStringValue, publicFieldValue);
        }

        /// <summary>
        /// Defines the test method AccessPrivateFieldTest.
        /// </summary>
        [TestMethod]
        public void AccessPrivateFieldTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var privateField = accessor.PrivateField;
            var privateFieldValue = (string) privateField;

            Assert.IsNotNull(privateField);
            Assert.AreEqual(PrivateFieldStringValue, privateFieldValue);
        }

        /// <summary>
        /// Defines the test method AccessStaticFieldTest.
        /// </summary>
        [TestMethod]
        public void AccessStaticFieldTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.StaticField);
        }

        /// <summary>
        /// Defines the test method AccessPublicPropertyTest.
        /// </summary>
        [TestMethod]
        public void AccessPublicPropertyTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var publicProperty = accessor.PublicProperty;
            var publicPropertyValue = (string) publicProperty;

            Assert.IsNotNull(publicProperty);
            Assert.AreEqual(PublicPropertyStringValue, publicPropertyValue);
        }

        /// <summary>
        /// Defines the test method AccessPrivatePropertyTest.
        /// </summary>
        [TestMethod]
        public void AccessPrivatePropertyTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var privateProperty = accessor.PrivateProperty;
            var privatePropertyValue = (string) privateProperty;

            Assert.IsNotNull(privateProperty);
            Assert.AreEqual(PrivatePropertyStringValue, privatePropertyValue);
        }


        /// <summary>Defines the test method AccessPrivateFieldOnBaseClassTest.</summary>
        [TestMethod]
        public void AccessPrivateFieldOnBaseClassTest()
        {
            var accessor = DynamicObjectAccessor.Create(new B(), true);
            var privateBaseClassProperty = accessor.PrivateBaseClassString;
            var privateBaseClassPropertyValue = (string)privateBaseClassProperty;

            Assert.IsNotNull(privateBaseClassProperty);
            Assert.AreEqual(PrivateFieldStringValue, privateBaseClassPropertyValue);
        }

        /// <summary>
        /// Defines the test method AccessStaticPropertyTest.
        /// </summary>
        [TestMethod]
        public void AccessStaticPropertyTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.StaticProperty);
        }

        /// <summary>
        /// Defines the test method AccessNonExistentMemberTest.
        /// </summary>
        [TestMethod]
        public void AccessNonExistentMemberTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.SomeMemebrThatDoesNotExist);
        }

        /// <summary>
        /// Defines the test method CastReturnToBaseTypeTest.
        /// </summary>
        [TestMethod]
        public void CastReturnToBaseTypeTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var publicField = accessor.PublicTestObject;
            var publicFieldValue = (AccessorMemberTests.A)publicField;

            Assert.IsNotNull(publicFieldValue);
            Assert.IsTrue(publicFieldValue is AccessorMemberTests.A);
        }

        /// <summary>
        /// Defines the test method CastReturnToConvertableTypeTest.
        /// </summary>
        [TestMethod]
        public void CastReturnToConvertableTypeTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var publicField = accessor.PublicTestInt;
            var publicFieldValue = (long) publicField;

            Assert.IsNotNull(publicFieldValue);
        }

        /// <summary>
        /// Defines the test method CastReturnToInvalidTypeTest.
        /// </summary>
        [TestMethod]
        public void CastReturnToInvalidTypeTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => (int)accessor.PublicField);
        }

        /// <summary>
        /// Defines the test method GetByIndexTest.
        /// </summary>
        [TestMethod]
        public void GetByIndexTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var publicField = accessor["PublicTestObject"];
            var publicFieldValue = (AccessorMemberTests.A)publicField;

            Assert.IsNotNull(publicFieldValue);
            Assert.IsTrue(publicFieldValue is AccessorMemberTests.A);
        }

        /// <summary>
        /// Defines the test method GetByMultiIndexTest.
        /// </summary>
        [TestMethod]
        public void GetByMultiIndexTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            var privateClassField = accessor["PublicTestObject", "PrivateClassString"];
            var privateClassFieldValue = (string)privateClassField;

            Assert.IsNotNull(privateClassField);
            Assert.AreEqual(PrivateFieldStringValue, privateClassFieldValue);
        }

        /// <summary>
        /// Defines the test method AccessInvalidIndexTest.
        /// </summary>
        [TestMethod]
        public void AccessInvalidIndexTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor["PropertyThatDoesNotExist"]);
        }

        /// <summary>
        /// Defines the test method AccessUsingInvalidIndexType.
        /// </summary>
        [TestMethod]
        public void AccessUsingInvalidIndexType()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor[42]);
        }

        /// <summary>
        /// Class TestClass.
        /// </summary>
        class TestClass
        {
#pragma warning disable 414 // Remove unused field
            /// <summary>
            /// The public field
            /// </summary>
            public string PublicField = PublicFieldStringValue;
            /// <summary>
            /// The private field
            /// </summary>
            private readonly string PrivateField = PrivateFieldStringValue;
            /// <summary>
            /// The static field
            /// </summary>
            public static string StaticField = StaticFieldStringValue;

            /// <summary>
            /// The public property
            /// </summary>
            public string PublicProperty = PublicPropertyStringValue;
            /// <summary>
            /// The private property
            /// </summary>
            private string PrivateProperty = PrivatePropertyStringValue;
            /// <summary>
            /// The static property
            /// </summary>
            public static string StaticProperty = StaticPropertyStringValue;

            /// <summary>
            /// The public test int
            /// </summary>
            public int PublicTestInt = 42;
            /// <summary>
            /// The public test object
            /// </summary>
            public B PublicTestObject = new B();
        }

        /// <summary>
        /// Class A.
        /// </summary>
        public class A
        {
            /// <summary>
            /// The private base class string
            /// </summary>
            private string PrivateBaseClassString = PrivateFieldStringValue;
        }

        /// <summary>
        /// Class B.
        /// Implements the <see cref="DynamicAccessor.Tests.AccessorMemberTests.TestClass.A" />
        /// </summary>
        /// <seealso cref="DynamicAccessor.Tests.AccessorMemberTests.TestClass.A" />
        public class B : A
        {
            /// <summary>
            /// The private class string
            /// </summary>
            private string PrivateClassString = PrivateFieldStringValue;
        }
#pragma warning restore 414
    }
}
