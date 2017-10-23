// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeReferenceScopeParameter.cs" company="Ninject Project Contributors">
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