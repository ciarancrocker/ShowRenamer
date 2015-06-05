using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Defines file metadata for media files renamed by ShowRenamer.
    /// </summary>
    public class FileNameContract
    {
        /// <summary>
        /// The series number that the file belongs to.
        /// </summary>
        public int SeriesNumber { get; set; }

        /// <summary>
        /// The episode number that the file contains.
        /// </summary>
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// The title of the show that the file contains.
        /// </summary>
        public string ShowTitle { get; set; }

        /// <summary>
        /// The file extension that the file uses.
        /// </summary>
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
