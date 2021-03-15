// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="TypeExtensions.cs" company="DynamicAccessor">
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
    /// Class TypeExtensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets all methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type, BindingFlags bindingFlags)
        {
            while (true)
            {
                foreach (var method in type.GetMethods(bindingFlags))
                {
                    yield return method;
                }

                var baseType = type.BaseType;

                if (baseType is null)
                {
                    break;
                }
                type = baseType;
            }
        }

        /// <summary>
        /// Gets all methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type, string name, BindingFlags bindingFlags)
        {
            while (true)
            {
                foreach (var method in type.GetMethods(bindingFlags))
                {
                    if (method.Name == name)
                    {
                        yield return method;
                    }
                }

                var baseType = type.BaseType;

                if (baseType is null)
                {
                    break;
                }
                type = baseType;
            }
        }

        /// <summary>
        /// Gets the method ex.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns>System.Nullable&lt;MethodInfo&gt;.</returns>
        public static MethodInfo? GetMethodEx(this Type type, string name, BindingFlags bindingFlags, Type[] parameterTypes, Type[] genericTypeArguments)
        {
            foreach (var method in type.GetAllMethods(name, bindingFlags))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != parameterTypes.Length)
                {

                }

                var genericArguments = method.GetGenericArguments();
                if (genericArguments.Length != genericTypeArguments.Length)
                {
                    continue;
                }

                // Check all arguments are assignable
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!parameters[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                    {
                        continue;
                    }
                }

                // Check all generic constraints
                for (int i = 0; i < parameters.Length; i++)
                {
                    genericArguments[i].GetGenericParameterConstraints();
                }

                return method;
            }

            return null;
        }
    }
}
