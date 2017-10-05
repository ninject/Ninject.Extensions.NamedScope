// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeActivationStrategy.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

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