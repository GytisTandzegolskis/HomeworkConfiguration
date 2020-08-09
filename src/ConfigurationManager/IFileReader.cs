using System.Collections.Generic;
using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    public interface IFileReader
        {
        IEnumerable<Configuration> ReadFile(string filePath);
        }
    }