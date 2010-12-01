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
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = AssertWithThrows;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Assert = AssertWithThrows;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Extensions.NamedScope.MSTestAttributes;
    using Xunit;
    using Xunit.Should;
#endif

    /// <summary>
    /// Integration tests for named scope together with context preservation.
    /// </summary>
    [TestClass]
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
            this.SetUp();
        }

        /// <summary>
        /// Sets up all tests.
        /// </summary>
        [TestInitialize]
        public void SetUp()
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

            child1.GrandChild.ShouldBeSameAs(child2.GrandChild);
            child1.GrandChild.ShouldNotBeSameAs(child3.GrandChild);

            child1.GrandChild.IsDisposed.ShouldBeTrue();
            child3.GrandChild.IsDisposed.ShouldBeFalse();

            parent2.Dispose();
            child3.GrandChild.IsDisposed.ShouldBeTrue();
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

            parent1.FirstInterface.ShouldBeSameAs(parent1.SecondInterface);
            parent1.FirstInterface.ShouldNotBeSameAs(parent2.FirstInterface);

            (parent1.FirstInterface as DisposeNotifyingObject).IsDisposed.ShouldBeTrue();
            (parent2.FirstInterface as DisposeNotifyingObject).IsDisposed.ShouldBeFalse();

            parent2.Dispose();
            (parent2.FirstInterface as DisposeNotifyingObject).IsDisposed.ShouldBeTrue();
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
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InNamedScope(ScopeName);

            var factory = this.kernel.Get<Factory>();
            factory.Dispose();

            Assert.Throws<ScopeDisposedException>(() => factory.CreateChild());
        }

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
            this.kernel.BindInterfaceToBinding<IGrandChild, GrandChild>();
            this.kernel.Bind<GrandChild>().ToSelf().InCallScope();

            var parent = this.kernel.Get<ParentWithFactory>();
            var child1 = parent.CreateChild();
            var child2 = parent.CreateChild();
            parent.Dispose();

            child1.GrandChild.ShouldBeSameAs(child1.GrandChild2);
            child1.GrandChild.ShouldNotBeSameAs(child2.GrandChild);
            child1.GrandChild.IsDisposed.ShouldBeFalse();
        }
    }
}