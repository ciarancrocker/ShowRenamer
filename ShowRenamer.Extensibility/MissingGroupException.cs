﻿using System;
using System.Text.RegularExpressions;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Exception thrown when one or more groups required by <see cref="SimpleRegexMatcher"/> is not found
    /// </summary>
    public class MissingGroupException : Exception
    {
        /// <summary>
        /// The Regex that was found to be invalid.
        /// </summary>
        public Regex InvalidRegex { get; }

        /// <summary>
        /// The groups that were not detected in the regex
        /// </summary>
        public string[] MissingGroupNames { get; }

        public MissingGroupException(Regex invalidRegex, string[] missingGroupNames) : base()
        {
            InvalidRegex = invalidRegex;
            MissingGroupNames = missingGroupNames;
        }
    }
}
