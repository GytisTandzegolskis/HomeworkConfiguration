namespace ConfigurationManager
    {
    public interface ILayer
        {
        bool TryGetConfigurationById(string configurationId, out Configuration configuration);
        string GetLayerName();
        }
    }