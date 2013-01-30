//-------------------------------------------------------------------------------
// <copyright file="ParentWithFactory.cs" company="bbv Software Services AG">
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
    /// A parent that has a factory for creation of other factories.
    /// </summary>
    public class ParentWithFactoryFactory : DisposeNotifyingObject
    {
        /// <summary>
        /// The factory factory.
        /// </summary>
        private readonly FactoryFactory factoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentWithFactoryFactory"/> class.
        /// </summary>
        /// <param name="factoryFactory">The factory factory.</param>
        public ParentWithFactoryFactory(FactoryFactory factoryFactory)
        {
            this.factoryFactory = factoryFactory;
        }

        /// <summary>
        /// Creates a new child using the factory factory.
        /// </summary>
        /// <returns>The created child.</returns>
        public Child CreateChild()
        {
            return this.factoryFactory.CreateFactory().CreateChild();
        }
    }
}