// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="BinderExtensions.cs" company="DynamicAccessor">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicAccessor.Extensions
{
    using System.Dynamic;
    using System.Reflection;

    /// <summary>
    /// Class BinderExtensions.
    /// </summary>
    public static class BinderExtensions
    {
        /// <summary>
        /// Gets the generic type parameters.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <returns>Type[].</returns>
        internal static Type[] GetGenericTypeParameters(this InvokeMemberBinder binder)
        {
            var typeArgumentsProperty = binder.GetType()
                .GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder")
                .GetProperty("TypeArguments");
            var typeArgs = typeArgumentsProperty?.GetValue(binder, null) as IList<Type>;
            return typeArgs?.ToArray() ?? Array.Empty<Type>();
        }

        /// <summary>
        /// Gets the generic type parameters.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <returns>Type[].</returns>
        internal static Type[] GetGenericTypeParameters(this InvokeBinder binder)
        {
            var typeArgumentsProperty = binder.GetType()
                .GetProperty("TypeArguments", BindingFlags.Instance | BindingFlags.NonPublic);
            var typeArgs = typeArgumentsProperty?.GetValue(binder, null) as IList<Type>;
            return typeArgs?.ToArray() ?? Array.Empty<Type>();
        }

    }
}
