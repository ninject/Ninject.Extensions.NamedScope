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
        private IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedScopeContextPreservationIntegrationTest"/> class.
        /// </summary>
        public NamedScopeContextPreservationIntegrationTest()
        {
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
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
            this.kernel.Bind<GrandChild>().ToSelf().InNamedScope(ScopeName);

            var parent1 = this.kernel.Get<ParentWithFactory>();
            var parent2 = this.kernel.Get<ParentWithFactory>();
            var child1 = parent1.CreateChild();
            var child2 = parent1.CreateChild();
            var child3 = parent2.CreateChild();
            parent1.Dispose();

            Assert.Same(child1.GrandChild, child2.GrandChild);
            Assert.Same(child1.GrandChild, child2.GrandChild);
            Assert.NotSame(child1.GrandChild, child3.GrandChild);

            Assert.True(child1.GrandChild.IsDisposed);
            Assert.False(child3.GrandChild.IsDisposed);

            parent2.Dispose();
            Assert.True(child3.GrandChild.IsDisposed);
        }

        /// <summary>
        /// Named scope supports scoping for multi interface classes
        /// </summary>
        [Fact]
        public void MultiInterfaceClassTest()
        {
            this.kernel.Bind<ParentWithMultiInterfaceClass>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<MultiInterfaceClass>().ToSelf().InNamedScope(ScopeName);
            this.kernel.BindInterfaceToBinding<IFirstInterface, MultiInterfaceClass>().InTransientScope();
            this.kernel.BindInterfaceToBinding<ISecondInterface, MultiInterfaceClass>().InTransientScope();

            var parent1 = this.kernel.Get<ParentWithMultiInterfaceClass>();
            var parent2 = this.kernel.Get<ParentWithMultiInterfaceClass>();
            parent1.Dispose();

            Assert.Same(parent1.FirstInterface, parent1.SecondInterface);
            Assert.NotSame(parent1.FirstInterface, parent2.FirstInterface);

            Assert.True((parent1.FirstInterface as DisposeNotifyingObject).IsDisposed);
            Assert.False((parent2.FirstInterface as DisposeNotifyingObject).IsDisposed);

            parent2.Dispose();
            Assert.True((parent2.FirstInterface as DisposeNotifyingObject).IsDisposed);
        }

        /// <summary>
        /// When a binding tries to use an object that is disposed as scope a <see cref="ScopeDisposedException"/>
        /// is thrown.
        /// </summary>
        [Fact]
        public void DisposedScopeThrowsScopeDisposedException()
        {
            this.kernel.Bind<Factory>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<GrandChild>().ToSelf().InNamedScope(ScopeName);

            var factory = this.kernel.Get<Factory>();
            factory.Dispose();

            Assert.Throws<ScopeDisposedException>(() => factory.CreateChild());
        }
    }
}