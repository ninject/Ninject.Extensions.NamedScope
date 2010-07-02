//-------------------------------------------------------------------------------
// <copyright file="DisposeNotifyingObject.cs" company="bbv Software Services AG">
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
    using Ninject.Infrastructure.Disposal;

    /// <summary>
    /// An object that notifies when ti is disposed.
    /// </summary>
    public class DisposeNotifyingObject : DisposableObject, INotifyWhenDisposed
    {
        /// <summary>
        /// Occurs when the object is disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public override void Dispose(bool disposing)
        {
            EventHandler e = this.Disposed;
            if (e != null)
            {
                e(this, EventArgs.Empty);
            }

            this.Disposed = null;

            base.Dispose(disposing);
        }
    }
}