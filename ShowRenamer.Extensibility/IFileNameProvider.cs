using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    public interface IFileNameProvider
    {
        FileNameContract Recognize(string fileName);
    }
}
