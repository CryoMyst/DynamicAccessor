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
    using System.Linq;
#pragma warning disable SA1135 // Using directives should be qualified
    using Extensions;
#pragma warning restore SA1135 // Using directives should be qualified

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
        internal DynamicObjectAccessor(object obj, bool useCache = true) : base(obj, useCache)
        {
        }


        /// <summary>
        /// Creates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns>.</returns>
        public static dynamic Create(object obj, bool useCache = true) => new DynamicObjectAccessor(obj, useCache);

        /// <summary>
        /// Tries the get member.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="result">The result.</param>
        /// <returns>.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            var objectType = this.Obj.GetType();
            // Try and get a member associated with this object
            var member = ReflectionCache.GetMember(objectType, binder.Name, this.UseCache);

            if (member is not null)
            {
                var value = member.GetValue(this.Obj);
                result = value is not null
                    ? new DynamicObjectAccessor(value, this.UseCache)
                    : null;
                return true;
            }

            // Try and get a method associated with this object
            var hasMethod = ReflectionCache.HasMethod(objectType, binder.Name, this.UseCache);
            if (hasMethod)
            {
                result = new DynamicMethodAccessor(this.Obj, binder.Name, this.UseCache);
                return true;
            }

            // Nothing found, fail
            result = null;
            return false;
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
    }
}
