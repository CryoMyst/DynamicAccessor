// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="MemberInfoExtensions.cs" company="DynamicAccessor">
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
    using System.Reflection;

    /// <summary>
    /// Class MemberInfoExtensions.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="forObject">For object.</param>
        /// <returns>System.Nullable&lt;System.Object&gt;.</returns>
        public static object? GetValue(this MemberInfo memberInfo, object forObject)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo) memberInfo).GetValue(forObject),
                MemberTypes.Property => ((PropertyInfo) memberInfo).GetValue(forObject),
                _ => throw new NotImplementedException()
            };
        }
    }
}
