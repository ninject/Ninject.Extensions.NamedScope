//-------------------------------------------------------------------------------
// <copyright file="UnknownScopeException.cs" company="bbv Software Services AG">
//   Copyright (c) 2010 bbv Software Services AG
//   Author: Remo Gloor remo.gloor@bbv.ch
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This exception is thrown when a binding requests a scope that is not defined in the current scope.
    /// </summary>
    public class UnknownScopeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownScopeException"/> class.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        public UnknownScopeException(string scopeName)
            : base(string.Format(CultureInfo.InvariantCulture, "The scope {0} is not known in the current context.", scopeName))
        {
        }
    }
}