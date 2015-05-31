using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    public class MissingGroupException : Exception
    {
        public Regex InvalidRegex { get; }
        public string[] MissingGroupNames { get; }
        
        public MissingGroupException(Regex invalidRegex, string[] missingGroupNames) : base()
        {
            InvalidRegex = invalidRegex;
            MissingGroupNames = missingGroupNames;
        }
    }
}
