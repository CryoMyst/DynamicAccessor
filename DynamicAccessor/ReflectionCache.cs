// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="ReflectionCache.cs" company="DynamicAccessor">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicAccessor
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// Class ReflectionCache.
    /// </summary>
    internal static class ReflectionCache
    {
        /// <summary>
        /// The default member binding flags
        /// </summary>
        private const BindingFlags DefaultMemberBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty;
        /// <summary>
        /// The defaul method binding flags
        /// </summary>
        private const BindingFlags DefaulMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The type methods
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<MethodInfo>> TypeMethods = new();
        /// <summary>
        /// The type members
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<MemberInfo>> TypeMembers = new();

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="useCache">The use cache.</param>
        /// <returns>System.Reflection.MemberInfo?.</returns>
        internal static MemberInfo? GetMember(Type type, string name, bool useCache)
        {
            var memberInfos = useCache
                ? TypeMembers.GetOrAdd(type, (t) => GetTypeFieldsAndProperties(t).ToList())
                : GetTypeFieldsAndProperties(type);

            return memberInfos.FirstOrDefault(m => m.Name == name);
        }

        /// <summary>
        /// Determines whether the specified type has method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="useCache">The use cache.</param>
        /// <returns>bool.</returns>
        internal static bool HasMethod(Type type, string name, bool useCache)
        {
            IEnumerable<MethodInfo> methodInfos = useCache
                ? TypeMethods.GetOrAdd(type, (t) => GetTypeMethods(t).ToList())
                : GetTypeMethods(type);

            return methodInfos.Any(m => m.Name == name);
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="useCache">The use cache.</param>
        /// <returns>System.Reflection.MethodInfo?.</returns>
        internal static MethodInfo? GetMethod(Type type, string name, Type[] parameters, bool useCache)
        {
            return type.GetMethod(name, DefaulMethodBindingFlags, null, parameters, null);
        }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="useCache">The use cache.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;System.Reflection.MethodInfo&gt;.</returns>
        internal static IEnumerable<MethodInfo> GetMethods(Type type, string name, bool useCache)
        {
            IEnumerable<MethodInfo> methodInfos = useCache
                ? TypeMethods.GetOrAdd(type, (t) => GetTypeMethods(t).ToList())
                : GetTypeMethods(type);

            return methodInfos.Where(m => m.Name == name);
        }

        /// <summary>
        /// Gets the type fields and properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;System.Reflection.MemberInfo&gt;.</returns>
        internal static IEnumerable<MemberInfo> GetTypeFieldsAndProperties(Type type) => type.GetAllMembers(DefaultMemberBindingFlags)
                .Where(m => m.MemberType is MemberTypes.Property or MemberTypes.Field);

        /// <summary>
        /// Gets the type methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Collections.Generic.IEnumerable&lt;System.Reflection.MethodInfo&gt;.</returns>
        internal static IEnumerable<MethodInfo> GetTypeMethods(Type type) => type.GetAllMethods(DefaulMethodBindingFlags);
    }
}
