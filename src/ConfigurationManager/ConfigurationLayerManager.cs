using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurationManager.Models;

namespace ConfigurationManager
    {
    /// <summary>
    ///     Class for managing configuration layers
    /// </summary>
    public class ConfigurationLayerManager : ILayerManager
        {
        private readonly Stack<ILayer> m_layers;

        public ConfigurationLayerManager()
            {
            m_layers = new Stack<ILayer> ();
            }

        /// <summary>
        ///     Adds new layer to the top of the stack
        /// </summary>
        /// <param name="configurationsLayer"></param>
        public void AddLayer(ILayer configurationsLayer)
            {
            m_layers.Push (configurationsLayer);
            }

        /// <summary>
        ///     Returns the requested configuration
        /// </summary>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public Configuration GetConfigurationById(string configurationId)
            {
            foreach (var layer in m_layers)
                if (layer.TryGetConfigurationById (configurationId, out var configuration))
                    return configuration;

            throw new KeyNotFoundException ("Configuration not found");
            }

        /// <summary>
        ///     Gets distinct configuration values from all layers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Configuration> GetAllConfigurationValues()
            {
            var allConfigurations = new List<Configuration> ();
            foreach (var layer in m_layers)
                foreach (var configuration in layer.GetAllConfigurations ())
                    RetrieveDistinctConfigurations (allConfigurations, configuration);

            return allConfigurations;
            }

        /// <summary>
        ///     Gets the configurations of the requested data model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public void GetConfigurationDataModel<T>(ref T dataModel)
            {
            var accumulatedExceptions = new List<Exception> ();
            foreach (var property in dataModel.GetType ().GetProperties ())
                try
                    {
                    var propertyType = property.PropertyType;
                    var propertyValue = GetConfigurationById (property.Name).ConfigurationValue;

                    property.SetValue (dataModel, Convert.ChangeType (propertyValue, propertyType), null);
                    }
                catch (KeyNotFoundException e)
                    {
                    accumulatedExceptions.Add (new KeyNotFoundException ($"{property.Name}: {e.Message}"));
                    }
                catch (FormatException e)
                    {
                    accumulatedExceptions.Add (new FormatException ($"{property.Name}: {e.Message}"));
                    }
                catch (InvalidCastException e)
                    {
                    accumulatedExceptions.Add (new InvalidCastException ($"{property.Name}: {e.Message}"));
                    }

            if (accumulatedExceptions.Count != 0)
                throw new AggregateException (accumulatedExceptions);
            }

        private static void RetrieveDistinctConfigurations(List<Configuration> allConfigurations,
            Configuration configuration)
            {
            if (allConfigurations.All (c => c.ConfigurationId != configuration.ConfigurationId))
                allConfigurations.Add (configuration);
            }
        }
    }