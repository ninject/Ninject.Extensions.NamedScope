// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeReferenceScopeParameter.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;
    using Ninject.Parameters;

    /// <summary>
    /// Parameter to pass the scope to <see cref="NamedScopeReference"/> when it is created.
    /// </summary>
    public class NamedScopeReferenceScopeParameter : Parameter
    {
        /// <summary>
        /// Weak reference to the scope
        /// </summary>
        private readonly WeakReference scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedScopeReferenceScopeParameter"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public NamedScopeReferenceScopeParameter(object scope)
            : base("NamedScopeReferenceScope", ctx => null, false)
        {
            this.scope = new WeakReference(scope);
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public object Scope
        {
            get
            {
                return this.scope.Target;
            }
        }
    }
}