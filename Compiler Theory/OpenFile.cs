using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_Theory
{
    public class OpenFile
    {
        /// <summary>
        /// Read text file and return it in array of string
        /// </summary>
        /// <param name="OpenedFilePath"></param>
        /// <returns></returns>
        public string[] GetFile(string OpenedFilePath)
        {
            return System.IO.File.ReadAllLines(OpenedFilePath);
        }
    }
}
