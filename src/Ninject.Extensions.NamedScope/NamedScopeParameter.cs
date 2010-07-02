//-------------------------------------------------------------------------------
// <copyright file="NamedScopeParameter.cs" company="bbv Software Services AG">
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
        public NamedScopeParameter(string name) : base(name, ctx => null, false)
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