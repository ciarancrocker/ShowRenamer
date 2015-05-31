using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    public interface IPlugin
    {
        IEnumerable<IFileNameProvider> FileNameProviders { get; }
        IEnumerable<Regex> FileNameRegexes { get; }
    }
}
