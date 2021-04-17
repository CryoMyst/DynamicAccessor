// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="DynamicObjectAccessor.cs" company="DynamicAccessor">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicAccessor
{
    using System;
    using System.Dynamic;
    using System.Globalization;
    using Extensions;

    /// <summary>
    /// Defines the <see cref="DynamicObjectAccessor" /> type
    /// </summary>
    public class DynamicObjectAccessor : DynamicAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectAccessor" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="treatAsType">Type of the treat as.</param>
        internal DynamicObjectAccessor(object obj, bool useCache = true, Type? treatAsType = null) : base(obj, useCache, treatAsType)
        {
        }

        /// <summary>
        /// Creates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="type">The type.</param>
        /// <returns>.</returns>
        public static dynamic Create(object obj, bool useCache = true, Type? treatAsType = null) => new DynamicObjectAccessor(obj, useCache, treatAsType);

        /// <summary>
        /// Tries the get member.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="result">The result.</param>
        /// <returns>.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return TryGetMemberInternal(binder, binder.Name, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject">DynamicObject</see> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">
        /// The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, indexes[0] is equal to 3.
        /// </param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
        {
            if (indexes.Length == 0)
            {
                result = default;
                return false;
            }

            // We can only index using strings
            if (indexes[0] is not string indexValue)
            {
                result = default;
                return false;
            }

            if (indexes.Length == 1)
            {
                return TryGetMemberInternal(binder, indexValue, out result);
            }
            else
            {
                // If there is more an one index recursively call on the child
                // Only work on a valid internal of ObjectAccessor
                if (TryGetMemberInternal(binder, indexValue, out var child) && child is DynamicObjectAccessor childAccessor)
                {
                    var newIndexes = new object[indexes.Length - 1];
                    Array.Copy(indexes, 1, newIndexes, 0, newIndexes.Length);

                    return childAccessor.TryGetIndex(binder, newIndexes, out result);
                }
                else
                {
                    // One or more calls failed in the multi-index call
                    result = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class, the value is "Test".</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            return TrySetMemberInternal(binder, binder.Name, value);
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class, indexes[0] is equal to 3.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"></see> class, value is equal to 10.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.</returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
        {
            if (indexes.Length == 0)
            {
                return false;
            }

            // We can only index using strings
            if (indexes[0] is not string indexValue)
            {
                return false;
            }

            if (indexes.Length == 1)
            {
                return TrySetMemberInternal(binder, indexValue, value);
            }
            else
            {
                // If there is more an one index recursively call on the child
                // Only work on a valid internal of ObjectAccessor
                if (TryGetMemberInternal(binder, indexValue, out var child) && child is DynamicObjectAccessor childAccessor)
                {
                    var newIndexes = new object[indexes.Length - 1];
                    Array.Copy(indexes, 1, newIndexes, 0, newIndexes.Length);

                    return childAccessor.TrySetIndex(binder, newIndexes, value);
                }
                else
                {
                    // One or more calls failed in the multi-index call
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries the convert.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="result">The result.</param>
        /// <returns>.</returns>
        public override bool TryConvert(ConvertBinder binder, out object? result)
        {
            if (binder.Type.IsInstanceOfType(this.Obj))
            {
                result = this.Obj;
                return true;
            }

            // Try converting it
            try
            {
                result = Convert.ChangeType(this.Obj, binder.Type, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                // ignored
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Tries the invoke member.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="result">The result.</param>
        /// <returns>bool.</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            var typeArgs = binder.GetGenericTypeParameters();
            var methodAccessor = new DynamicMethodAccessor(this.Obj, binder.Name, this.UseCache);
            return methodAccessor.TryInvokeInternal(args, typeArgs, out result);
        }

        /// <summary>
        /// Tries the get member internal.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TryGetMemberInternal(DynamicMetaObjectBinder binder, string memberName, out object? result)
        {
            var objectType = this.Type;
            // Try and get a member associated with this object
            var member = ReflectionCache.GetMember(objectType, memberName, this.UseCache);

            if (member is not null)
            {
                var value = member.GetValue(this.Obj);
                result = value is not null
                    ? new DynamicObjectAccessor(value, this.UseCache)
                    : null;
                return true;
            }

            // Try and get a method associated with this object
            var hasMethod = ReflectionCache.HasMethod(objectType, memberName, this.UseCache);
            if (hasMethod)
            {
                result = new DynamicMethodAccessor(this.Obj, memberName, this.UseCache);
                return true;
            }

            // Nothing found, fail
            result = null;
            return false;
        }

        /// <summary>
        /// Tries the set member internal.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TrySetMemberInternal(DynamicMetaObjectBinder binder, string memberName, object? value)
        {
            var objectType = this.Type;
            // Try and get a member associated with this object
            var member = ReflectionCache.GetMember(objectType, memberName, this.UseCache);

            if (member is null)
            {
                return false;
            }

            return member.TrySetValue(this.Obj, value);
        }
    }
}
