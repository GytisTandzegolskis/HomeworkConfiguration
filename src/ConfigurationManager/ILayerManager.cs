namespace ConfigurationManager
    {
    public interface ILayerManager
        {
        void AddLayer(ILayer configurationsList);
        Configuration GetConfigurationById(string configurationId);
        ILayer GetLayerByName(string layerName);
        }
    }