// -------------------------------------------------------------------------------------------------
// <copyright file="NamedScopeModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010 bbv Software Services AG
//   Copyright (c) 2011-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.NamedScope
{
    using System;
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