//-------------------------------------------------------------------------------
// <copyright file="NamedScopeReferenceTest.cs" company="bbv Software Services AG">
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

#if !NO_MOQ
namespace Ninject.Extensions.NamedScope
{
    using System;
    using Moq;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = AssertWithThrows;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using Assert = AssertWithThrows;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Extensions.NamedScope.MSTestAttributes;
    using Xunit;
#endif

    /// <summary>
    /// Tests the implementation of <see cref="NamedScopeReference"/>.
    /// </summary>
    [TestClass]
    public class NamedScopeReferenceTest
    {
        /// <summary>
        /// When the scope reference is disposed the scope is disposed too.
        /// </summary>
        [Fact]
        public void DisposeDisposesTheReferencedScope()
        {
            var scopeMock = new Mock<IDisposable>();
            var testee = new NamedScopeReference(scopeMock.Object);

            testee.Dispose();

            scopeMock.Verify(scope => scope.Dispose());
        }
    }
}
#endif