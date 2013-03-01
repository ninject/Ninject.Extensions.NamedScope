//-------------------------------------------------------------------------------
// <copyright file="ExceptionFormatter.cs" company="Ninject Project Contributors">
//   Copyright (c) 2013 Ninject Project Contributors
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
    using System.IO;

    using Ninject.Activation;
    using Ninject.Infrastructure.Introspection;

    /// <summary>
    /// Provides meaningful exception messages.
    /// </summary>
    public static class ExceptionFormatter
    {
        /// <summary>
        /// Generates a message saying that the binding could not be resolved due to unknown scope
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="scopeName">Scope name</param>
        /// <returns>The exception message.</returns>
        public static string CouldNotFindScope(IRequest request, string scopeName)
        {
            using (var sw = new StringWriter())
            {
                sw.WriteLine("Error activating {0}", request.Service.Format());
                sw.WriteLine("The scope {0} is not known in the current context.", scopeName);
                sw.WriteLine("No matching scopes are available, and the type is declared InNamedScope({0}).", scopeName);

                sw.WriteLine("Activation path:");
                sw.WriteLine(request.FormatActivationPath());

                sw.WriteLine("Suggestions:");
                sw.WriteLine("  1) Ensure that you have defined the scope {0}.", scopeName);
                sw.WriteLine("  2) Ensure you have a parent resolution that defines the scope.");
                sw.WriteLine("  3) If you are using factory methods or late resolution, check that the correct IResolutionRoot is being used.");

                return sw.ToString();
            }
        }
    }
}
