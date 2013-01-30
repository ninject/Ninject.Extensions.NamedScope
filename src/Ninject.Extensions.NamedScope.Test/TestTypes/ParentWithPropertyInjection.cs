//-------------------------------------------------------------------------------
// <copyright file="Parent.cs" company="bbv Software Services AG">
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
    /// The parent used in the tests
    /// </summary>
    public class ParentWithPropertyInjection : DisposeNotifyingObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentWithPropertyInjection"/> class.
        /// </summary>
        /// <param name="grandChild">The grand child.</param>
        public ParentWithPropertyInjection(IGrandChild grandChild)
        {
            this.GrandChild = grandChild;
        }

        /// <summary>
        /// Gets the first child.
        /// </summary>
        /// <value>The first child.</value>
        [Inject]
        public Child FirstChild { get; set; }

        /// <summary>
        /// Gets the second child.
        /// </summary>
        /// <value>The second child.</value>
        [Inject]
        public Child SecondChild { get; set; }

        /// <summary>
        /// Gets the grand child.
        /// </summary>
        /// <value>The second child.</value>
        public IGrandChild GrandChild { get; private set; }
    }
}