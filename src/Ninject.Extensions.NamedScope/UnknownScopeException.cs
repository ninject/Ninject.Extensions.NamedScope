// -------------------------------------------------------------------------------------------------
// <copyright file="UnknownScopeException.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;

    /// <summary>
    /// This exception is thrown when a binding requests a scope that is not defined in the current scope.
    /// </summary>
    public class UnknownScopeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownScopeException"/> class.
        /// </summary>
        public UnknownScopeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownScopeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UnknownScopeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownScopeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnknownScopeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}