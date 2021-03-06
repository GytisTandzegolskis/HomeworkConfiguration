﻿using System.Collections.Generic;
using ConfigurationManager.Models;
using FluentAssertions;
using Xunit;

namespace ConfigurationManager.UnitTests
    {
    public class ConfigurationLayerTests
        {
        [Fact]
        public void TryGetConfigurationById_WhenFileContainsInformation_ReturnsExpectedConfiguration()
            {
            //arrange
            var expectedConfiguration = new Configuration {ConfigurationId = "Layer1", ConfigurationValue = 1};
            var configurationList = new List<Configuration>
                {
                expectedConfiguration,
                new Configuration {ConfigurationId = "Layer2", ConfigurationValue = "2"}
                };

            var configurationLayer = new ConfigurationLayer (configurationList);

            //act
            var foundConfiguration =
                configurationLayer.TryGetConfigurationById (expectedConfiguration.ConfigurationId,
                    out var configuration);

            //assert
            foundConfiguration.Should ().BeTrue ();
            configuration.Should ().BeEquivalentTo (expectedConfiguration);
            }

        [Fact]
        public void TryGetConfigurationById_WhenFileDoesNotContainInformation_ReturnsNull()
            {
            //arrange
            var configurationList = new List<Configuration>
                {
                new Configuration {ConfigurationId = "Layer1", ConfigurationValue = 1},
                new Configuration {ConfigurationId = "Layer2", ConfigurationValue = "2"}
                };

            var configurationLayer = new ConfigurationLayer (configurationList);

            //act
            var foundConfiguration =
                configurationLayer.TryGetConfigurationById ("NonExistingId", out var configuration);

            //assert
            foundConfiguration.Should ().BeFalse ();
            configuration.Should ().BeNull ();
            }
        }
    }