using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDictionary.Interfaces
{
    public interface IConsole
    {
        void PrintError(string text);
        void PrintSuccess(string text);
        void PrintWarning(string text);
        void PrintNormal(string text);
    }
}
