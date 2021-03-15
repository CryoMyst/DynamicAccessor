// ***********************************************************************
// Assembly         : DynamicAccessor
// Author           : CryoM
// Created          : 03-14-2021
//
// Last Modified By : CryoM
// Last Modified On : 03-14-2021
// ***********************************************************************
// <copyright file="DynamicAccessor.cs" company="DynamicAccessor">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicAccessor
{
    using System.Dynamic;

    /// <summary>
    /// <br />
    /// </summary>
    public abstract class DynamicAccessor : DynamicObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAccessor" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        internal DynamicAccessor(object obj, bool useCache = true)
        {
            this.UseCache = useCache;
            this.Obj = obj;
        }

        /// <summary>
        /// Gets a value indicating whether [use cache].
        /// </summary>
        /// <value><c>true</c> if [use cache]; otherwise, <c>false</c>.</value>
        protected bool UseCache { get; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        protected object Obj { get; }
    }
}
