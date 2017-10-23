// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeModule.cs" company="Ninject Project Contributors">
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
    using System.Linq;

    using Ninject.Activation;
    using Ninject.Activation.Strategies;
    using Ninject.Modules;

    /// <summary>
    /// This module provides the definition of named scopes.
    /// </summary>
    public class NamedScopeModule : NinjectModule
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, NamedScopeActivationStrategy>();
            this.Bind<NamedScopeReference>().ToSelf().InScope(GetNamedScope);
        }

        /// <summary>
        /// Gets the named scope.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The scope for a named scope reference.</returns>
        private static object GetNamedScope(IContext context)
        {
            return context.Parameters.OfType<NamedScopeReferenceScopeParameter>().Single().Scope;
        }
    }
}