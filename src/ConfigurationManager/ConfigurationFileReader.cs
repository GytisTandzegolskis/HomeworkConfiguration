using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigurationManager
    {
    public class ConfigurationFileReader
        {
        public IEnumerable<Configuration> ReadFile(string filePath)
            {
            if (!File.Exists (filePath))
                throw new FileNotFoundException ();

            var configurationList = new List<Configuration> ();
            using (var file = new StreamReader (filePath))
                {
                string line;
                while ((line = file.ReadLine ()) != null)
                    {
                    if (TryGetConfiguration (line, out var configuration))
                        configurationList.Add (configuration);
                    }
                }

            if (configurationList.Count == 0)
                throw new ArgumentNullException (filePath, "No configurations found");

            return configurationList;
            }

        private bool TryGetConfiguration(string line, out Configuration configuration)
            {
            var splitLine = Regex.Split(line, @"\s+");

            if (Regex.IsMatch (splitLine[0], @"^[a-z|A-Z|0-9]+[:]+$") &&
                Regex.IsMatch (splitLine[1], "^[a-z|A-Z|0-9|:]+$"))
                {
                configuration = new Configuration { ConfigurationId = splitLine[0], ConfigurationValue = splitLine[1] };
                return true;
                }

            configuration = null;
            return false;
            }
        }
    }