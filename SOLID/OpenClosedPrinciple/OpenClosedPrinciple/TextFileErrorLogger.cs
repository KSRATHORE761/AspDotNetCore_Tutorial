using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenClosedPrinciple
{
    public class TextFileErrorLogger : IErrorLogger
    {
        public void LogError(string message)
        {
            System.IO.File.WriteAllText(@"C:\Users\Public\LogFolder\Errors.txt", message);
        }
    }
}
