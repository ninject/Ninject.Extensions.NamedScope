//-------------------------------------------------------------------------------
// <copyright file="NamedScopeIntegrationTest.cs" company="bbv Software Services AG">
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

    using Ninject;
    using Ninject.Extensions.NamedScope.TestTypes;
    using Xunit;

    /// <summary>
    /// Integration Test for Dependency Creation Module.
    /// </summary>
    public class NameScopeTest : IDisposable
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
        /// Initializes a new instance of the <see cref="NameScopeTest"/> class.
        /// </summary>
        public NameScopeTest()
        {
#if !SILVERLIGHT
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
#else
            this.kernel = new StandardKernel();
#endif
            this.kernel.Load(new NamedScopeModule());
        }

        /// <summary>
        /// Disposes the kernel.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }

        /// <summary>
        /// The life cycle of objects with Named Scope is the same as the object that defines the named scope.
        /// </summary>
        [Fact]
        public void NamedScopeLifeCycle()
        {
            this.kernel.Bind<Parent>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InNamedScope(ScopeName);

            var parent1 = this.kernel.Get<Parent>();
            var parent2 = this.kernel.Get<Parent>();
            parent1.Dispose();

            parent1.GrandChild.Should().BeSameAs(parent1.FirstChild.GrandChild);
            parent1.GrandChild.Should().BeSameAs(parent1.SecondChild.GrandChild);
            parent1.GrandChild.Should().NotBeSameAs(parent2.GrandChild);

            parent1.GrandChild.IsDisposed.Should().BeTrue();
            parent2.GrandChild.IsDisposed.Should().BeFalse();

            parent2.Dispose();
            parent2.GrandChild.IsDisposed.Should().BeTrue();
        }

        /// <summary>
        /// Bindings with parent scope are disposed with their parents.
        /// </summary>
        [Fact]
        public void ParentScope()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().InParentScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InParentScope();

            var parent1 = this.kernel.Get<Parent>();
            var parent2 = this.kernel.Get<Parent>();
            parent1.Dispose();

            parent1.FirstChild.Should().BeSameAs(parent1.SecondChild);
            parent1.GrandChild.Should().NotBeSameAs(parent1.FirstChild.GrandChild);
            parent1.FirstChild.Should().NotBeSameAs(parent2.FirstChild);
            parent1.GrandChild.Should().NotBeSameAs(parent2.GrandChild);

            parent1.FirstChild.IsDisposed.Should().BeTrue();
            parent1.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.IsDisposed.Should().BeFalse();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeFalse();

            parent2.Dispose();
            parent2.FirstChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
        }

        /// <summary>
        /// Bindings with parent scope are disposed with their parents.
        /// </summary>
        [Fact]
        public void CallScope()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().InCallScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InCallScope();

            var parent1 = this.kernel.Get<Parent>();
            var parent2 = this.kernel.Get<Parent>();
            parent1.Dispose();

            parent1.FirstChild.Should().BeSameAs(parent1.SecondChild);
            parent1.GrandChild.Should().BeSameAs(parent1.FirstChild.GrandChild);
            parent1.FirstChild.Should().NotBeSameAs(parent2.FirstChild);
            parent1.GrandChild.Should().NotBeSameAs(parent2.GrandChild);

            parent1.FirstChild.IsDisposed.Should().BeTrue();
            parent1.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.IsDisposed.Should().BeFalse();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeFalse();

            parent2.Dispose();
            parent2.FirstChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
        }

        /// <summary>
        /// Bindings with parent scope are disposed with their parents.
        /// </summary>
        [Fact]
        public void CallScopeWithPropertyInjection()
        {
            this.kernel.Bind<ParentWithPropertyInjection>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().InCallScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InCallScope();

            var parent1 = this.kernel.Get<ParentWithPropertyInjection>();
            var parent2 = this.kernel.Get<ParentWithPropertyInjection>();
            parent1.Dispose();

            parent1.FirstChild.Should().BeSameAs(parent1.SecondChild);
            parent1.GrandChild.Should().BeSameAs(parent1.FirstChild.GrandChild);
            parent1.FirstChild.Should().NotBeSameAs(parent2.FirstChild);
            parent1.GrandChild.Should().NotBeSameAs(parent2.GrandChild);

            parent1.FirstChild.IsDisposed.Should().BeTrue();
            parent1.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.IsDisposed.Should().BeFalse();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeFalse();

            parent2.Dispose();
            parent2.FirstChild.IsDisposed.Should().BeTrue();
            parent2.FirstChild.GrandChild.IsDisposed.Should().BeTrue();
        }

        /// <summary>
        /// If the scope of a binding is not found an <see cref="UnknownScopeException"/>
        /// is thrown.
        /// </summary>
        [Fact]
        public void UnknownScopeThrowsUnknownScopeException()
        {
            this.kernel.Bind<Parent>().ToSelf().DefinesNamedScope("SomeScopeName");
            this.kernel.Bind<Child>().ToSelf().InTransientScope();
            this.kernel.Bind<IGrandChild>().To<GrandChild>().InNamedScope("AnotherScopeName");

            Assert.Throws<UnknownScopeException>(() => this.kernel.Get<Parent>());
        }

        [Fact]
        public void GetNamedScope_WhenAvailable_ReturnsTheScope()
        {
            object scope = null;

            this.kernel.Bind<Parent>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.Bind<IGrandChild>().To<GrandChild>()
                .OnActivation((ctx, instance) => scope = ctx.GetNamedScope(ScopeName));

            this.kernel.Get<Parent>();

            scope.Should().NotBeNull();
        }

        [Fact]
        public void GetNamedScope_WhenAvailable_ThrowsAnException()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.Bind<IGrandChild>().To<GrandChild>()
                .OnActivation((ctx, instance) => 
                {
                    Action a = () => ctx.GetNamedScope(ScopeName);
                    a.ShouldThrow<UnknownScopeException>();
                });

            this.kernel.Get<Parent>();
        }

        [Fact]
        public void TryGetNamedScope_WhenAvailable_ReturnsTheScope()
        {
            object scope = null;

            this.kernel.Bind<Parent>().ToSelf().DefinesNamedScope(ScopeName);
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.Bind<IGrandChild>().To<GrandChild>()
                .OnActivation((ctx, instance) => scope = ctx.TryGetNamedScope(ScopeName));

            this.kernel.Get<Parent>();

            scope.Should().NotBeNull();
        }

        [Fact]
        public void TryGetNamedScope_WhenAvailable_ReturnsNull()
        {
            object scope = null;

            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.Bind<IGrandChild>().To<GrandChild>()
                .OnActivation((ctx, instance) =>
                {
                    Action a = () => scope = ctx.TryGetNamedScope(ScopeName);
                    a.ShouldNotThrow<UnknownScopeException>();
                });

            this.kernel.Get<Parent>();
            scope.Should().BeNull();
        }
    }
}