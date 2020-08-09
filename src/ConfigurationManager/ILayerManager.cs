using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    public interface ILayerManager
        {
        void AddLayer(ILayer configurationsLayer);
        Configuration GetConfigurationById(string configurationId);
        }
    }