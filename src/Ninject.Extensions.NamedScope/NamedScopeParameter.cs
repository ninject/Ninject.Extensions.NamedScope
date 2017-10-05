// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeParameter.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using Ninject.Infrastructure.Disposal;
    using Ninject.Parameters;

    /// <summary>
    /// Parameter for defining that an object defines a named scope.
    /// </summary>
    public class NamedScopeParameter : Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedScopeParameter"/> class.
        /// </summary>
        /// <param name="name">The name of the scope.</param>
        public NamedScopeParameter(string name)
            : base(name, ctx => null, false)
        {
            this.Scope = new DisposeNotifyingObject();
        }

        /// <summary>
        /// Gets the scope object.
        /// </summary>
        /// <value>The scope object.</value>
        public IDisposableObject Scope { get; private set; }
    }
}