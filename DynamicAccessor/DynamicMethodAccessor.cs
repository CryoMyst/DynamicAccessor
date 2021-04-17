// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="DynamicMethodAccessor.cs" company="DynamicAccessor">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicAccessor
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using Extensions;


    /// <summary>
    /// Class DynamicMethodAccessor.
    /// Implements the <see cref="DynamicAccessor" />
    /// </summary>
    public class DynamicMethodAccessor : DynamicAccessor
    {
        /// <summary>
        /// The method name
        /// </summary>
        private readonly string _methodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethodAccessor" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        internal DynamicMethodAccessor(object obj, string methodName, bool useCache)
            : base(obj, useCache) => this._methodName = methodName;

        /// <summary>
        /// Provides the implementation for operations that invoke an object. Classes derived from the <see cref="T:System.Dynamic.DynamicObject">DynamicObject</see> class can override this method to specify dynamic behavior for operations such as invoking an object or a delegate.
        /// </summary>
        /// <param name="binder">Provides information about the invoke operation.</param>
        /// <param name="args">The arguments that are passed to the object during the invoke operation. For example, for the sampleObject(100) operation, where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject">DynamicObject</see> class, args[0] is equal to 100.</param>
        /// <param name="result">The result of the object invocation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.</returns>
        public override bool TryInvoke(InvokeBinder binder, object?[]? args, out object? result) =>
            this.TryInvokeInternal(args, binder.GetGenericTypeParameters(), out result);

        /// <summary>
        /// Tries the invoke internal.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="typeArgs">The type arguments.</param>
        /// <param name="result">The result.</param>
        /// <returns>bool.</returns>
        internal bool TryInvokeInternal(object?[]? args, Type[] typeArgs, out object? result)
        {
            var parameterTypes = args?.Select(a => a?.GetType()).ToArray() ?? Array.Empty<Type>();

            if (typeArgs.Length == 0)
            {
                var matchingMethod = ReflectionCache.GetMethod(this.Type, this._methodName, parameterTypes!, this.UseCache);

                if (matchingMethod is not null)
                {
                    var invokeResult = matchingMethod.Invoke(this.Obj, args);
                    result = invokeResult is not null
                        ? new DynamicObjectAccessor(invokeResult)
                        : null;
                    return true;
                }
            }
            else
            {
                // Have to find the correct generic method.
                // 1. Find method with name.
                // 2. Ensure parameter and generic parameter counts
                // 3. Ensure parameters are assignable to
                // 4. try create the generic method for each
                // 5. If multiple work then just choose first?

                var methods = ReflectionCache.GetMethods(this.Type, this._methodName, this.UseCache)
                    .Where(m => m.IsGenericMethodDefinition);

                var potentialMethods = new List<MethodInfo>();

                foreach (var method in methods)
                {
                    var valid = true;
                    var methodParameters = method.GetParameters();
                    var genericArguments = method.GetGenericArguments();

                    // Parameter counts are not the same
                    if (methodParameters.Length != parameterTypes.Length)
                    {
                        continue;
                    }

                    if (genericArguments.Length != typeArgs.Length)
                    {
                        continue;
                    }

                    // Check parameter validity
                    for (var i = 0; i < methodParameters.Length; i++)
                    {
                        var methodParameter = methodParameters[i];
                        var proposedParameterType = parameterTypes[i];

                        if (methodParameter.ParameterType.IsGenericParameter)
                        {
                            continue;
                        }

                        // For out parameter match exactly
                        if (methodParameter.IsOut)
                        {
                            if (methodParameter.ParameterType != proposedParameterType)
                            {
                                valid = false;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (!methodParameter.ParameterType.IsAssignableFrom(proposedParameterType))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        continue;
                    }

                    // Check generic parameter validity
                    for (var i = 0; i < genericArguments.Length; i++)
                    {
                        var genericArgument = genericArguments[i];
                        var proposedGenericType = typeArgs[i];

                        if (genericArgument.IsGenericParameter)
                        {
                            continue;
                        }

                        if (genericArgument.IsAssignableFrom(proposedGenericType))
                        {
                            continue;
                        }

                        valid = false;
                        break;
                    }

                    if (!valid)
                    {
                        continue;
                    }

                    potentialMethods.Add(method);
                }

                foreach (var potentialMethod in potentialMethods)
                {
                    try
                    {
                        // Try and create the generic method, if not try the next and so on
                        var genericMethod = potentialMethod.MakeGenericMethod(typeArgs);
                        var invokeResult = genericMethod.Invoke(this.Obj, args);
                        result = invokeResult is not null
                            ? new DynamicObjectAccessor(invokeResult)
                            : null;
                        return true;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            result = null;
            return false;
        }
    }
}
