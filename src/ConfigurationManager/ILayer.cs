using System.Collections.Generic;
using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    public interface ILayer
        {
        bool TryGetConfigurationById(string configurationId, out Configuration configuration);
        IEnumerable<Configuration> GetAllConfigurations();
        }
    }