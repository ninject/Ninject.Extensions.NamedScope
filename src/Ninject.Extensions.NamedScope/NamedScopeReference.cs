// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeReference.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;
    using Ninject.Infrastructure.Disposal;

    /// <summary>
    /// References a scope object. The reference its self has the owner of the scope as scope.
    /// </summary>
    public class NamedScopeReference : DisposableObject
    {
        /// <summary>
        /// The scope object.
        /// </summary>
        private readonly IDisposable scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedScopeReference"/> class.
        /// </summary>
        /// <param name="scope">The scope object.</param>
        public NamedScopeReference(IDisposable scope)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.scope.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}