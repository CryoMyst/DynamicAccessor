// ***********************************************************************
// Assembly         : DynamicAccessor.Tests
// Author           : CryoM
// Created          : 04-12-2021
//
// Last Modified By : CryoM
// Last Modified On : 04-12-2021
// ***********************************************************************
// <copyright file="AccessorMemberAssignTests.cs" company="DynamicAccessor.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DynamicAccessor.Tests
{
    using Microsoft.CSharp.RuntimeBinder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines test class AccessorMemberAssignTests.
    /// </summary>
    [TestClass]
    public class AccessorMemberAssignTests
    {
        /// <summary>
        /// The test string value
        /// </summary>
        private const string TestStringValue = "TestValue";

        /// <summary>
        /// Defines the test method TestFieldAssigning.
        /// </summary>
        [TestMethod]
        public void TestFieldAssigning()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor.TestField = TestStringValue;
            var newFieldValue = (string) accessor.TestField;

            Assert.AreEqual(TestStringValue, newFieldValue);
        }

        /// <summary>
        /// Defines the test method TestFieldNullAssigning.
        /// </summary>
        [TestMethod]
        public void TestFieldNullAssigning()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor.TestField = null;
            var newFieldValue = (string) accessor.TestField;

            Assert.AreEqual(null, newFieldValue);
        }

        /// <summary>
        /// Defines the test method TestPropertyAssigning.
        /// </summary>
        [TestMethod]
        public void TestPropertyAssigning()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor.TestProperty = TestStringValue;
            var newPropertyValue = (string)accessor.TestProperty;

            Assert.AreEqual(TestStringValue, newPropertyValue);
        }

        /// <summary>
        /// Defines the test method TestPropertyNullAssigning.
        /// </summary>
        [TestMethod]
        public void TestPropertyNullAssigning()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor.TestProperty = null;
            var newPropertyValue = (string)accessor.TestProperty;

            Assert.AreEqual(null, newPropertyValue);
        }

        /// <summary>
        /// Defines the test method TestThrowOnGetProperty.
        /// </summary>
        [TestMethod]
        public void TestThrowOnGetProperty()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.TestGetOnlyProperty = TestStringValue);
        }

        /// <summary>
        /// Defines the test method TestThrowOnInvalidTypeAssign.
        /// </summary>
        [TestMethod]
        public void TestThrowOnInvalidTypeAssign()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.TestField = 42);
        }

        /// <summary>
        /// Defines the test method TestThrowOnNullAssignOnvalueType.
        /// </summary>
        [TestMethod]
        public void TestThrowOnNullAssignOnvalueType()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            Assert.ThrowsException<RuntimeBinderException>(() => accessor.TestValueField = null);
        }


        /// <summary>
        /// Defines the test method SetByIndexTest.
        /// </summary>
        [TestMethod]
        public void SetByIndexTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor["TestField"] = TestStringValue;
            var newFieldValue = (string) accessor.TestField;

            Assert.AreEqual(TestStringValue, newFieldValue);
        }


        /// <summary>
        /// Defines the test method SetByMultiIndexTest.
        /// </summary>
        [TestMethod]
        public void SetByMultiIndexTest()
        {
            var accessor = DynamicObjectAccessor.Create(new TestClass(), true);
            accessor["Child", "PublicStringField"] = TestStringValue;
            var newFieldValue = (string) accessor.Child.PublicStringField;

            Assert.AreEqual(TestStringValue, newFieldValue);
        }

        /// <summary>
        /// Class TestClass.
        /// </summary>
        class TestClass
        {
            /// <summary>
            /// The test field
            /// </summary>
            public string TestField = string.Empty;
            /// <summary>
            /// Gets or sets the test property.
            /// </summary>
            /// <value>The test property.</value>
            public string TestProperty { set; get; } = string.Empty;
            /// <summary>
            /// Gets the test get only property.
            /// </summary>
            /// <value>The test get only property.</value>
            public string TestGetOnlyProperty => string.Empty;

            /// <summary>
            /// The test value field
            /// </summary>
            public int TestValueField = 1;

            /// <summary>
            /// The child
            /// </summary>
            public ChildClass Child = new ChildClass();
        }

        /// <summary>
        /// Class ChildClass.
        /// </summary>
        class ChildClass
        {
            /// <summary>
            /// The public string field
            /// </summary>
            public string PublicStringField = string.Empty;
        }
    }
}
