//-------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="bbv Software Services AG">
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
    using Ninject.Syntax;

    /// <summary>
    /// A factory class.
    /// </summary>
    public class Factory : DisposeNotifyingObject
    {
        /// <summary>
        /// The resolution root that is used to create new objects.
        /// </summary>
        private readonly IResolutionRoot resolutionRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="resolutionRoot">The resolution root.</param>
        public Factory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <returns>The newly created child.</returns>
        public Child CreateChild()
        {
            return this.resolutionRoot.Get<Child>();
        }
    }
}