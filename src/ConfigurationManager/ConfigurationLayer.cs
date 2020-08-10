using System.Collections.Generic;
using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    /// <summary>
    ///     Class for managing layer level information
    /// </summary>
    public class ConfigurationLayer : ILayer
        {
        private readonly IEnumerable<Configuration> m_configurationObjects;

        public ConfigurationLayer(IEnumerable<Configuration> configurationObjects)
            {
            m_configurationObjects = configurationObjects;
            }

        /// <summary>
        ///     Tries to get configuration from layer, if available, returns through out parameter and returns true
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public bool TryGetConfigurationById(string configurationId, out Configuration configuration)
            {
            configuration = null;
            foreach (var configurationObject in m_configurationObjects)
                if (configurationId.ToLower ().Equals (configurationObject.ConfigurationId.ToLower ()))
                    {
                    configuration = configurationObject;
                    return true;
                    }

            return false;
            }

        public IEnumerable<Configuration> GetAllConfigurations()
            {
            return m_configurationObjects;
            }
        }
    }