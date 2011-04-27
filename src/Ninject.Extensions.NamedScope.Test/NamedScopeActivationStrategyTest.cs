//-------------------------------------------------------------------------------
// <copyright file="NamedScopeActivationStrategyTest.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Ninject.Activation;
    using Ninject.Parameters;
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
    /// Tests the implementation of <see cref="NamedScopeActivationStrategy"/>.
    /// </summary>
    [TestClass]
    public class NamedScopeActivationStrategyTest
    {
        /// <summary>
        /// The strategy creates a <see cref="NamedScopeReference"/> for each <see cref="NamedScopeParameter"/>
        /// using the kernel to create it. The scope of the newly created reference is the instance reference. 
        /// </summary>
        [Fact]
        public void ActivationCreatesNamedScopeReferenceUsingTheKernel()
        {
            var requestParameters = new List<IParameter>();
            var reference = new InstanceReference { Instance = new object() };
            var namedScopeParameter = new NamedScopeParameter("Scope1");
            var kernelMock = new Mock<IKernel>();
            var contextMock = CreateContextMock();

            contextMock.Object.Parameters.Add(namedScopeParameter);
            contextMock.SetupGet(context => context.Kernel).Returns(kernelMock.Object);
            SetupKernelGetNamedScopeReference(kernelMock, requestParameters);

            var testee = new NamedScopeActivationStrategy();
            testee.Activate(contextMock.Object, reference);

            requestParameters.Count().ShouldBe(2);
            AssertConstructorArgumentExists("scope", namedScopeParameter.Scope, requestParameters);
            AssertNamedScopeReferenceScopeParameterExists(reference.Instance, requestParameters);
        }

        /// <summary>
        /// Asserts that one <see cref="NamedScopeReferenceScopeParameter"/> was added to the
        /// request parameters and that its scope is the specified one.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="requestParameters">The request parameters.</param>
        private static void AssertNamedScopeReferenceScopeParameterExists(object scope, IEnumerable<IParameter> requestParameters)
        {
            var parameter = requestParameters.OfType<NamedScopeReferenceScopeParameter>().SingleOrDefault();

            parameter.ShouldNotBeNull();
            parameter.Scope.ShouldBe(scope);
        }

        /// <summary>
        /// Asserts the <paramref name="requestParameters"/> contain a <see cref="ConstructorArgument"/> with the
        /// <paramref name="expectedName"/> and  <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The expected name of the <see cref="ConstructorArgument"/>.</param>
        /// <param name="expectedValue">The expected value of the <see cref="ConstructorArgument"/>.</param>
        /// <param name="requestParameters">The request parameters that are checked for the existence of the <see cref="ConstructorArgument"/>.</param>
        private static void AssertConstructorArgumentExists(string expectedName, object expectedValue, IEnumerable<IParameter> requestParameters)
        {
            var constructorArgument = requestParameters.OfType<ConstructorArgument>().Where(parameter => parameter.Name == expectedName).SingleOrDefault();
            
            constructorArgument.ShouldNotBeNull();
            constructorArgument.GetValue(new Mock<IContext>().Object, null).ShouldBe(expectedValue);
        }

        /// <summary>
        /// Creates a context mock.
        /// </summary>
        /// <returns>The newly created mock.</returns>
        private static Mock<IContext> CreateContextMock()
        {
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(context => context.Parameters).Returns(new List<IParameter>());
            return contextMock;
        }

        /// <summary>
        /// Matcher that always matches. But it adds the <paramref name="receivedParameters"/> to the
        /// <paramref name="requestParameters"/>
        /// </summary>
        /// <param name="requestParameters">The request parameters.</param>
        /// <param name="receivedParameters">The received parameters.</param>
        /// <returns>True in any case.</returns>
        private static bool MatchAlwaysButAddReceivedParametersToTheSpecifiedList(List<IParameter> requestParameters, IEnumerable<IParameter> receivedParameters)
        {
            requestParameters.AddRange(receivedParameters);
            return true;
        }
        
        /// <summary>
        /// Setups kernel.Get{NamedScopeReference};
        /// </summary>
        /// <param name="kernelMock">The kernel mock.</param>
        /// <param name="requestParameters">The request parameters.</param>
        private static void SetupKernelGetNamedScopeReference(Mock<IKernel> kernelMock, List<IParameter> requestParameters)
        {
            var request = new Mock<IRequest>();
            var namedScopeReference = new NamedScopeReference(new DisposeNotifyingObject());
            kernelMock
                .Setup(kernel => kernel.CreateRequest(
                    typeof(NamedScopeReference), 
                    null, 
                    It.Is<IEnumerable<IParameter>>(p => MatchAlwaysButAddReceivedParametersToTheSpecifiedList(requestParameters, p)), 
                    false, 
                    true))
                .Returns(request.Object);
            kernelMock
                .Setup(kernel => kernel.Resolve(request.Object))
                .Returns(new List<object> { namedScopeReference });
        }
    }
}
#endif