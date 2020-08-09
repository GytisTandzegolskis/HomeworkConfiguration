using System;
using System.Collections.Generic;
using System.IO;
using ConfigurationManager;
using ConfigurationManager.Models;

namespace HomeworkConfiguration
    {
    internal class Program
        {
        private static void Main(string[] args)
            {
            var layerManager = new ConfigurationLayerManager ();
            var hasEnded = false;

            while (!hasEnded)
                {
                ShowInstructions ();
                var command = Console.ReadLine ();
                switch (command!.ToLower ())
                    {
                        case "upload":
                            Console.WriteLine ("Paste path to file");
                            var filePath = Console.ReadLine ();
                            ReadConfiguration (filePath, layerManager);
                            break;

                        case "id":
                            Console.WriteLine ("Write configurationId which you want to be found");
                            GetConfigurationById (layerManager);
                            break;

                        case "all":
                            foreach (var configurationValue in layerManager.GetAllConfigurationValues ())
                                Console.WriteLine (
                                    $"{configurationValue.ConfigurationId}: {configurationValue.ConfigurationValue}");

                            break;

                        case "correct":
                            var correctDataModel = new CorrectModel ();
                            GetDataModelInfo (layerManager, correctDataModel);
                            break;

                        case "incorrect":
                            var inCorrectDataModel = new IncorrectModel ();
                            GetDataModelInfo (layerManager, inCorrectDataModel);
                            break;

                        case "q":
                            hasEnded = true;
                            break;

                        default:
                            Console.WriteLine ("Command not supported");
                            break;
                    }
                }
            }

        private static void GetDataModelInfo<T>(ConfigurationLayerManager layerManager, T inCorrectDataModel)
            {
            try
                {
                layerManager.GetConfigurationDataModel (ref inCorrectDataModel);
                foreach (var property in inCorrectDataModel.GetType ().GetProperties ())
                    Console.WriteLine ($"{property.Name}: {property.GetValue (inCorrectDataModel)}");
                }
            catch (AggregateException aggregateExceptions)
                {
                foreach (var exception in aggregateExceptions.InnerExceptions)
                    Console.WriteLine (exception.Message);
                }
            }

        private static void GetConfigurationById(ConfigurationLayerManager layerManager)
            {
            var configurationId = Console.ReadLine ();
            try
                {
                var configuration = layerManager.GetConfigurationById (configurationId);
                Console.WriteLine ($"{configuration.ConfigurationId}: {configuration.ConfigurationValue}");
                }
            catch (KeyNotFoundException exception)
                {
                Console.WriteLine ($"Error: {exception.Message}");
                }
            }

        private static void ReadConfiguration(string filePath, ConfigurationLayerManager layerManager)
            {
            var fileReader = new ConfigurationFileReader ();
            try
                {
                var configurationLayer = new ConfigurationLayer (fileReader.ReadFile (filePath));
                layerManager.AddLayer (configurationLayer);
                }
            catch (Exception exception) when (exception is ArgumentNullException || exception is FileNotFoundException)
                {
                Console.WriteLine ($"Error: {exception.Message}");
                }
            }

        private static void ShowInstructions()
            {
            Console.WriteLine ("\nWrite \"upload\" to upload a file\n" +
                               "Write \"id\" to find configuration by id\n" +
                               "Write \"all\" to get all configurations\n" +
                               "Write \"correct\" to get correct data model\n" +
                               "Write \"incorrect\" to get incorrect data model\n" +
                               "Write \"q\" to quit\n");
            }
        }
    }