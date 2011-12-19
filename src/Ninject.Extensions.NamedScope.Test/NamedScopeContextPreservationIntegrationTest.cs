//-------------------------------------------------------------------------------
// <copyright file="NamedScopeContextPreservationIntegrationTest.cs" company="bbv Software Services AG">
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

    using FluentAssertions;

    using Ninject.Extensions.ContextPreservation;
    using Ninject.Extensions.NamedScope.TestTypes;
    using Xunit;
    
    /// <summary>
    /// Integration tests for named scope together with context preservation.
    /// </summary>
    public class NamedScopeContextPreservationIntegrationTest : IDisposable
    {
        /// <summary>
        /// The Name of the scope used in the tests.
        /// </summary>
        private const string ScopeName = "Scope";

        /// <summary>
        /// The kernel used in the tests.
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedScopeContextPreservationIntegrationTest"/> class.
        /// </summary>
        public NamedScopeContextPreservationIntegrationTest()
        {
#if !SILVERLIGHT
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
#else
            this.kernel = new StandardKernel();
#endif
            this.kernel.Load(new NamedScopeModule());
            this.kernel.Load(new ContextPreservationModule());
        }

        /// <summary>
        /// Disposes the kernel.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }
        
        /// <summary>
        /// The named scope is passed to a new request by using <see cref="ContextPreservationModule"/>. 
        /// </summary>
        [Fact]
        public void NamedScopeOverResolutionRootBoundaries()
        {
            this.kernel.Bind<ParentWithFactory>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Factory>().ToSelf().InTransientScope();
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InNamedScope(ScopeName);

            var parent1 = this.kernel.Get<ParentWithFactory>();
            var parent2 = this.kernel.Get<ParentWithFactory>();
            var child1 = parent1.CreateChild();
            var child2 = parent1.CreateChild();
            var child3 = parent2.CreateChild();
            parent1.Dispose();

            child1.GrandChild.Should().BeSameAs(child2.GrandChild);
            child1.GrandChild.Should().NotBeSameAs(child3.GrandChild);

            child1.GrandChild.IsDisposed.Should().BeTrue();
            child3.GrandChild.IsDisposed.Should().BeFalse();

            parent2.Dispose();
            child3.GrandChild.IsDisposed.Should().BeTrue();
        }

#if !MONO_2_6
        /// <summary>
        /// Named scope supports scoping for multi interface classes
        /// </summary>
        [Fact]
        public void MultiInterfaceClassTest()
        {
            this.kernel.Bind<ParentWithMultiInterfaceClass>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<IFirstInterface, ISecondInterface>().To<MultiInterfaceClass>().InNamedScope(ScopeName);

            var parent1 = this.kernel.Get<ParentWithMultiInterfaceClass>();
            var parent2 = this.kernel.Get<ParentWithMultiInterfaceClass>();
            parent1.Dispose();

            parent1.FirstInterface.Should().BeSameAs(parent1.SecondInterface);
            parent1.FirstInterface.Should().NotBeSameAs(parent2.FirstInterface);

            (parent1.FirstInterface as DisposeNotifyingObject).IsDisposed.Should().BeTrue();
            (parent2.FirstInterface as DisposeNotifyingObject).IsDisposed.Should().BeFalse();

            parent2.Dispose();
            (parent2.FirstInterface as DisposeNotifyingObject).IsDisposed.Should().BeTrue();
        }
#endif

        /// <summary>
        /// When a binding tries to use an object that is disposed as scope a <see cref="ScopeDisposedException"/>
        /// is thrown.
        /// </summary>
        [Fact]
        public void DisposedScopeThrowsScopeDisposedException()
        {
            this.kernel.Bind<Factory>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InNamedScope(ScopeName);

            var factory = this.kernel.Get<Factory>();
            factory.Dispose();

            Assert.Throws<ScopeDisposedException>(() => factory.CreateChild());
        }

#if !MONO_2_6
        /// <summary>
        /// The call scope takes the object resolved by the last Get as scope. But ToMethod(ctx => ctx.ContextPreservingGet) 
        /// is excluded. 
        /// </summary>
        [Fact]
        public void CallScopeStopsAtResolutionRootBoundary()
        {
            this.kernel.Bind<ParentWithFactory>().ToSelf();
            this.kernel.Bind<Factory>().ToSelf().InTransientScope();
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<IGrandChild, GrandChild>().To<GrandChild>().InCallScope();

            var parent = this.kernel.Get<ParentWithFactory>();
            var child1 = parent.CreateChild();
            var child2 = parent.CreateChild();
            parent.Dispose();

            child1.GrandChild.Should().BeSameAs(child1.GrandChild2);
            child1.GrandChild.Should().NotBeSameAs(child2.GrandChild);
            child1.GrandChild.IsDisposed.Should().BeFalse();
        }
#endif
    }
}