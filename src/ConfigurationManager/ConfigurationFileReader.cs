using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    public class ConfigurationFileReader : IFileReader
        {
        /// <summary>
        ///     Reads the presented file and returns list of Configurations
        /// </summary>
        /// <param name="filePath">Path to the configuration file</param>
        /// <returns></returns>
        public IEnumerable<Configuration> ReadFile(string filePath)
            {
            if (!File.Exists (filePath))
                throw new FileNotFoundException ("File not found");

            var configurationList = new List<Configuration> ();
            using (var file = new StreamReader (filePath))
                {
                ReadLines (file, configurationList);
                }

            if (configurationList.Count == 0)
                throw new ArgumentNullException (filePath, "No configurations found");

            return configurationList;
            }

        private void ReadLines(StreamReader file, List<Configuration> configurationList)
            {
            string line;
            while ((line = file.ReadLine ()) != null)
                if (TryGetConfiguration (line, out var configuration))
                    configurationList.Add (configuration);
            }

        private bool TryGetConfiguration(string line, out Configuration configuration)
            {
            var splitLine = Regex.Split (line, @"\s+");

            if (Regex.IsMatch (splitLine[0], @"^[a-z|A-Z|0-9]+[:]+$") &&
                Regex.IsMatch (splitLine[1], "^[a-z|A-Z|0-9|:]+$"))
                {
                configuration = new Configuration
                        {ConfigurationId = splitLine[0].TrimEnd (':'), ConfigurationValue = splitLine[1]};

                return true;
                }

            configuration = null;
            return false;
            }
        }
    }