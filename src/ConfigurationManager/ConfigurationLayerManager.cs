using System.Collections.Generic;

namespace ConfigurationManager
    {
    public class ConfigurationLayerManager : ILayerManager
        {
        private readonly Stack<ILayer> m_layers;

        public ConfigurationLayerManager()
            {
            m_layers = new Stack<ILayer> ();
            }

        public void AddLayer(ILayer configurationsList)
            {
            m_layers.Push (configurationsList);
            }

        public Configuration GetConfigurationById(string configurationId)
            {
            foreach (var layer in m_layers)
                if (layer.TryGetConfigurationById (configurationId, out var configuration))
                    return configuration;

            throw new KeyNotFoundException ($"Configuration by the id: {configurationId} not found");
            }

        public ILayer GetLayerByName(string layerName)
            {
            foreach (var layer in m_layers)
                if (layer.GetLayerName () == layerName)
                    return layer;

            throw new KeyNotFoundException ();
            }
        }
    }