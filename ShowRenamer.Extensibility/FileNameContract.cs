using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    public class FileNameContract
    {
        public int SeriesNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string ShowTitle { get; set; }
        public string Extension { get; set; }

        private string Sanitize(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Replace('.', ' ').Replace('-', ' ').Replace('_', ' ').Trim());
        }

        public override string ToString()
        {
            return $"{Sanitize(ShowTitle)} S{SeriesNumber}E{EpisodeNumber}.{Extension}";
        }
    }
}
