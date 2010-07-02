//-------------------------------------------------------------------------------
// <copyright file="NamedScopeActivationStrategy.cs" company="bbv Software Services AG">
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
    using System.Linq;
    using Activation;
    using Activation.Strategies;
    using Ninject.Parameters;

    /// <summary>
    /// Activation strategy that creates a reference from the owner to the named scopes.
    /// </summary>
    public class NamedScopeActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// Activates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            var namedScopeParameters = context.Parameters.OfType<NamedScopeParameter>();
            foreach (var namedScopeParameter in namedScopeParameters)
            {
                context.Kernel.Get<NamedScopeReference>(
                    new NamedScopeReferenceScopeParameter(reference.Instance),
                    new ConstructorArgument("scope", namedScopeParameter.Scope));
            }
        }
    }
}