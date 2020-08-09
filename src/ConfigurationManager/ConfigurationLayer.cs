using System.Collections.Generic;

namespace ConfigurationManager
    {
    public class ConfigurationLayer : ILayer
        {
        private readonly IEnumerable<Configuration> m_configurationObjects;
        private readonly string m_layerName;

        public ConfigurationLayer(string layerName, IEnumerable<Configuration> configurationObjects)
            {
            m_configurationObjects = configurationObjects;
            m_layerName = layerName;
            }

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

        public string GetLayerName()
            {
            return m_layerName;
            }
        }
    }