using System;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Exception thrown when a file name is not recognised by an <see cref="IFileNameProvider"/>.
    /// </summary>
    public class FileNameNotRecognisedException : Exception
    {
    }
}
