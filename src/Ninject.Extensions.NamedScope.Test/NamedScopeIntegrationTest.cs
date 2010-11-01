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
    using Ninject;
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
    /// Integration Test for Dependency Creation Module.
    /// </summary>
    [TestClass]
    public class NameScopeTest : IDisposable
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
        /// Initializes a new instance of the <see cref="NameScopeTest"/> class.
        /// </summary>
        public NameScopeTest()
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
            this.kernel.Bind<GrandChild>().ToSelf().InNamedScope(ScopeName);

            var parent1 = this.kernel.Get<Parent>();
            var parent2 = this.kernel.Get<Parent>();
            parent1.Dispose();

            parent1.GrandChild.ShouldBeSameAs(parent1.FirstChild.GrandChild);
            parent1.GrandChild.ShouldBeSameAs(parent1.SecondChild.GrandChild);
            parent1.GrandChild.ShouldNotBeSameAs(parent2.GrandChild);

            parent1.GrandChild.IsDisposed.ShouldBeTrue();
            parent2.GrandChild.IsDisposed.ShouldBeFalse();

            parent2.Dispose();
            parent2.GrandChild.IsDisposed.ShouldBeTrue();
        }

        /// <summary>
        /// Bindings with parent scope are disposed with their parents.
        /// </summary>
        [Fact]
        public void ParentScope()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().InParentScope();
            this.kernel.Bind<GrandChild>().ToSelf().InParentScope();

            var parent1 = this.kernel.Get<Parent>();
            var parent2 = this.kernel.Get<Parent>();
            parent1.Dispose();

            parent1.FirstChild.ShouldBeSameAs(parent1.SecondChild);
            parent1.GrandChild.ShouldNotBeSameAs(parent1.FirstChild.GrandChild);
            parent1.FirstChild.ShouldNotBeSameAs(parent2.FirstChild);
            parent1.GrandChild.ShouldNotBeSameAs(parent2.GrandChild);

            parent1.FirstChild.IsDisposed.ShouldBeTrue();
            parent1.FirstChild.GrandChild.IsDisposed.ShouldBeTrue();
            parent2.FirstChild.IsDisposed.ShouldBeFalse();
            parent2.FirstChild.GrandChild.IsDisposed.ShouldBeFalse();

            parent2.Dispose();
            parent2.FirstChild.IsDisposed.ShouldBeTrue();
            parent2.FirstChild.GrandChild.IsDisposed.ShouldBeTrue();
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
            this.kernel.Bind<GrandChild>().ToSelf().InNamedScope("AnotherScopeName");

            Assert.Throws<UnknownScopeException>(() => this.kernel.Get<Parent>());
        }
    }
}