using ShowRenamer.Extensibility;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Provides a simple file name provider based on a provided regular expression.
    /// </summary>
    /// <remarks>
    /// Regexes that are to use this provider should expose four capture groups:
    /// <list type="bullet">
    ///     <item>
    ///         <term>title</term>
    ///         <description>The title of the show (string)</description>
    ///     </item>
    ///     <item>
    ///         <term>season</term>
    ///         <description>The season or series number of the show (integer)</description>
    ///     </item>
    ///     <item>
    ///         <term>episode</term>
    ///         <description>The episode number of the show (integer)</description>
    ///     </item>
    ///     <item>
    ///         <term>format</term>
    ///         <description>The file format the show is stored in, without leading dot (string)</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public sealed class SimpleRegexMatcher : IFileNameProvider
    {
        private readonly Regex _sourceRegex;

        /// <summary>
        /// Initialise a matcher using the specified regex.
        /// </summary>
        /// <param name="regex"><see cref="Regex"/> this matcher should use.</param>
        public SimpleRegexMatcher(Regex regex)
        {
            if(regex == null)
            {
                throw new ArgumentNullException(nameof(regex));
            }
            string[] missingGroups = VerifyRegex(regex);
            if(missingGroups.Count() > 0)
            {
                throw new MissingGroupException(regex, missingGroups);
            }
            _sourceRegex = regex;
        }

        /// <summary>
        /// Verify that a regex is valid for use by this matcher.
        /// </summary>
        /// <param name="regex">The <see cref="Regex"/> that should be verified.</param>
        /// <returns></returns>
        private string[] VerifyRegex(Regex regex)
        {
            bool hasTitle = false, hasSeason = false, hasEpisode = false, hasFormat = false;
            List<string> missingGroups = new List<string>();
            foreach(string group in regex.GetGroupNames())
            {
                switch(group)
                {
                    case "title":
                        hasTitle = true;
                        break;
                    case "season":
                        hasSeason = true;
                        break;
                    case "episode":
                        hasEpisode = true;
                        break;
                    case "format":
                        hasFormat = true;
                        break;
                }
            }
            if (!hasTitle)
                missingGroups.Add("title");
            if (!hasSeason)
                missingGroups.Add("season");
            if (!hasEpisode)
                missingGroups.Add("episode");
            if (!hasFormat)
                missingGroups.Add("format");
            return missingGroups.ToArray();
        }

        public FileNameContract Recognize(string fileName)
        {
            if (!_sourceRegex.IsMatch(fileName))
            {
                throw new FileNameNotRecognisedException();
            }
            Match filenameMatch = _sourceRegex.Match(fileName);
            return new FileNameContract()
            {
                ShowTitle = filenameMatch.Groups["title"].ToString(),
                SeriesNumber = Convert.ToInt32(filenameMatch.Groups["season"].ToString()),
                EpisodeNumber = Convert.ToInt32(filenameMatch.Groups["episode"].ToString()),
                Extension = filenameMatch.Groups["format"].ToString()
            };
            throw new NotImplementedException();
        }
    }
}
