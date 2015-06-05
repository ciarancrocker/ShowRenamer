using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Interface for classes that provide additional features to ShowRenamer
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Instances of <see cref="IFileNameProvider"/> that should be used verbatim
        /// </summary>
        IEnumerable<IFileNameProvider> FileNameProviders { get; }

        /// <summary>
        /// Regexes that should be parsed into a <see cref="SimpleRegexMatcher"/> provider.
        /// </summary>
        IEnumerable<Regex> FileNameRegexes { get; }
    }
}
