// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeReference.cs" company="Ninject Project Contributors">
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