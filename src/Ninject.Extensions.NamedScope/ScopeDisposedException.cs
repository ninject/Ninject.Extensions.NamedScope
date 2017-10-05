// -------------------------------------------------------------------------------------------------
// <copyright file="ScopeDisposedException.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;

    /// <summary>
    /// This exception is thrown when a binding tries to use a scope that already has been disposed.
    /// </summary>
    public class ScopeDisposedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeDisposedException"/> class.
        /// </summary>
        public ScopeDisposedException()
            : this("The requested scope has already been disposed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeDisposedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ScopeDisposedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeDisposedException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ScopeDisposedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}