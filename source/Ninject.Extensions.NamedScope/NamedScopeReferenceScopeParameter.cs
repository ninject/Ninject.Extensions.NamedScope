//-------------------------------------------------------------------------------
// <copyright file="NamedScopeReferenceScopeParameter.cs" company="bbv Software Services AG">
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