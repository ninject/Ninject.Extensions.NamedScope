using System.IO;
using Ninject.Activation;
using Ninject.Infrastructure.Introspection;

namespace Ninject.Extensions.NamedScope
{
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
                sw.WriteLine("  1) Ensure that you have defined the scope {0}.", scopeName);//request.Service.Format());
                sw.WriteLine("  2) Ensure you have a parent resolution that defines the scope.");
                sw.WriteLine("  3) If you are using factory methods or late resolution, check that the correct IResolutionRoot is being used.");

                return sw.ToString();
            }
        }
    }
}
