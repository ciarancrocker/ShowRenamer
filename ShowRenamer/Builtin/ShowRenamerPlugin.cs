using ShowRenamer.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowRenamer.Builtin
{
    internal class ShowRenamerPlugin : IPlugin
    {
        public IEnumerable<IFileNameProvider> FileNameProviders { get; }
        public IEnumerable<Regex> FileNameRegexes { get; }

        public ShowRenamerPlugin()
        {
            FileNameProviders = new List<IFileNameProvider>();
            FileNameRegexes = new List<Regex>()
            {
                new Regex("(?'title'.+)[sS](?'season'\\d+)[eE](?'episode'\\d+).*\\.(?'format'avi|mp4|mkv|mpeg|mpg)"),
                new Regex("(?'title'.+)(?'season'\\d+)[xX](?'episode'\\d+).*\\.(?'format'avi|mp4|mkv|mpeg|mpg)")
            };
        }
    }
}
