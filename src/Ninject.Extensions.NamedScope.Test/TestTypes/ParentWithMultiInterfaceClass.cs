//-------------------------------------------------------------------------------
// <copyright file="ParentWithMultiInterfaceClass.cs" company="bbv Software Services AG">
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

namespace Ninject.Extensions.NamedScope.TestTypes
{
    /// <summary>
    /// A parent class of a class with multiple interfaces.
    /// The parent has a reference to both interfaces.
    /// </summary>
    public class ParentWithMultiInterfaceClass : DisposeNotifyingObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentWithMultiInterfaceClass"/> class.
        /// </summary>
        /// <param name="firstInterface">The first interface.</param>
        /// <param name="secondInterface">The second interface.</param>
        public ParentWithMultiInterfaceClass(IFirstInterface firstInterface, ISecondInterface secondInterface)
        {
            this.FirstInterface = firstInterface;
            this.SecondInterface = secondInterface;
        }

        /// <summary>
        /// Gets the first interface.
        /// </summary>
        /// <value>The first interface.</value>
        public IFirstInterface FirstInterface { get; private set; }

        /// <summary>
        /// Gets the second interface.
        /// </summary>
        /// <value>The second interface.</value>
        public ISecondInterface SecondInterface { get; private set; }
    }
}