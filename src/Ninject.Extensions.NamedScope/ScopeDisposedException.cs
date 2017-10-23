// -------------------------------------------------------------------------------------------------
// <copyright file="ScopeDisposedException.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
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