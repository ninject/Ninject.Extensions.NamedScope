//-------------------------------------------------------------------------------
// <copyright file="DisposeNotifyingObjectTest.cs" company="bbv Software Services AG">
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
    using Xunit;
    using Xunit.Should;

    /// <summary>
    /// Tests the implementation of <see cref="DisposeNotifyingObject"/>
    /// </summary>
    public class DisposeNotifyingObjectTest
    {
        /// <summary>
        /// Initially IsDispose returns <see langword="false"/>.
        /// </summary>
        [Fact]
        public void IsDisposedIsInitiallyFalse()
        {
            var testee = new DisposeNotifyingObject();
            testee.IsDisposed.ShouldBeFalse();
        }

        /// <summary>
        /// After the instance is disposed IsDispose returns <see langword="true"/>.
        /// </summary>
        [Fact]
        public void IsDisposedAfterDisposeIsTrue()
        {
            var testee = new DisposeNotifyingObject();

            testee.Dispose();

            testee.IsDisposed.ShouldBeTrue();
        }

        /// <summary>
        /// Disposed is fired when the object is disposed.
        /// </summary>
        [Fact]
        public void NotifiesWhenDisposed()
        {
            var disposed = false;
            var testee = new DisposeNotifyingObject();
            testee.Disposed += (sender, e) => disposed = true;

            testee.Dispose();

            disposed.ShouldBeTrue();
        }
    }
}