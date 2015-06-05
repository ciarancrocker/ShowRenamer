using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Interface for file name providers to the ShowRenamer algorithm.
    /// </summary>
    public interface IFileNameProvider
    {
        /// <summary>
        /// Attempt to recognise the given file name, returning a <see cref="FileNameContract"/> if successful
        /// or throwing a meaningful exception otherwise
        /// </summary>
        /// <param name="fileName">The file name that should be recognised</param>
        /// <returns>A fully recognised file name.</returns>
        /// <exception cref="FileNameNotRecognisedException">Thrown if a file name is not recognised.</exception>
        FileNameContract Recognize(string fileName);
    }
}
