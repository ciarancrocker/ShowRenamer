using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Exception thrown when a file name is not recognised by an <see cref="IFileNameProvider"/>.
    /// </summary>
    public class FileNameNotRecognisedException : Exception
    {
    }
}
