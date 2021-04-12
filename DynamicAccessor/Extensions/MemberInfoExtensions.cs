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
    internal static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="forObject">For object.</param>
        /// <returns>System.Nullable&lt;System.Object&gt;.</returns>
        internal static object? GetValue(this MemberInfo memberInfo, object forObject)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo) memberInfo).GetValue(forObject),
                MemberTypes.Property => ((PropertyInfo) memberInfo).GetValue(forObject),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Tries the set value.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="forObject">For object.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        internal static bool TrySetValue(this MemberInfo memberInfo, object forObject, object? value)
        {
            var valueType = value?.GetType();

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    {
                        // A field type
                        if (memberInfo is not FieldInfo fieldinfo)
                        {
                            // throw new InvalidCastException($"Member has {MemberTypes.Field} but is not assignable to {typeof(FieldInfo)}");
                            return false;
                        }

                        var fieldType = fieldinfo.FieldType;

                        // Handle null assignments
                        if (value is null && fieldType.IsNullable())
                        {
                            fieldinfo.SetValue(forObject, null);
                            return true;
                        }

                        if (fieldType.IsAssignableFrom(valueType))
                        {
                            fieldinfo.SetValue(forObject, value);
                            return true;
                        }

                        return false;
                    }
                case MemberTypes.Property:
                    {
                        if (memberInfo is not PropertyInfo propertyInfo)
                        {
                            // throw new InvalidCastException($"Property has {MemberTypes.Property} but is not assignable to {typeof(PropertyInfo)}");
                            return false;
                        }

                        // Cant assign to get only properties
                        if (!propertyInfo.CanWrite)
                        {
                            return false;
                        }

                        var propertyType = propertyInfo.PropertyType;

                        // Handle null assignments
                        if (value is null && propertyType.IsNullable())
                        {
                            propertyInfo.SetValue(forObject, null);
                            return true;
                        }

                        // Ensure type can be assigned to property
                        if (propertyType.IsAssignableFrom(valueType))
                        {
                            propertyInfo.SetValue(forObject, value);
                            return true;
                        }

                        return false;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
