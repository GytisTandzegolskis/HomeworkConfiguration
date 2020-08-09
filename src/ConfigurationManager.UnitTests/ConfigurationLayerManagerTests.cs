using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Xunit;

namespace ConfigurationManager.UnitTests
    {
    public class ConfigurationLayerManagerTests
        {
        [Theory]
        [AutoMockData]
        public void
            GetConfigurationById_WhenCalledWithConfigurationWhichExistsInInitialLayer_ReturnsExpectedConfiguration(
            ILayer initialLayer,
            ILayer topLayer,
            ConfigurationLayerManager manager)
            {
            //arrange
            var configurationId = "Test";
            var expectedConfiguration = new Configuration {ConfigurationId = configurationId, ConfigurationValue = 1};

            Mock.Get (initialLayer)
                .Setup (f => f.TryGetConfigurationById (configurationId, out It.Ref<Configuration>.IsAny))
                .Returns (false);

            Mock.Get (topLayer)
                .Setup (f => f.TryGetConfigurationById (configurationId, out expectedConfiguration))
                .Returns (true);

            manager.AddLayer (initialLayer);
            manager.AddLayer (topLayer);

            //act
            var configuration = manager.GetConfigurationById (configurationId);

            //assert
            configuration.Should ().Be (expectedConfiguration);
            }

        [Theory]
        [AutoMockData]
        public void
            GetConfigurationById_WhenCalledWithConfigurationWhichExistsInBothLayers_ReturnsTopLayerConfiguration(
            ILayer initialLayer,
            ILayer topLayer,
            ConfigurationLayerManager manager)
            {
            //arrange
            var configurationId = "Test";
            var expectedConfiguration = new Configuration {ConfigurationId = configurationId, ConfigurationValue = 1};

            Mock.Get (initialLayer)
                .Setup (f => f.TryGetConfigurationById (configurationId, out expectedConfiguration))
                .Returns (true);

            Mock.Get (topLayer)
                .Setup (f => f.TryGetConfigurationById (configurationId, out expectedConfiguration))
                .Returns (true);

            manager.AddLayer (initialLayer);
            manager.AddLayer (topLayer);

            //act
            var configuration = manager.GetConfigurationById (configurationId);

            //assert
            configuration.Should ().Be (expectedConfiguration);

            Mock.Get (topLayer)
                .Verify (f => f.TryGetConfigurationById (configurationId, out expectedConfiguration), Times.Once);

            Mock.Get (initialLayer)
                .Verify (f => f.TryGetConfigurationById (configurationId, out expectedConfiguration), Times.Never);
            }

        [Theory]
        [AutoMockData]
        public void
            GetConfigurationById_WhenCalledWithConfigurationWhichDoesNotExist_Throws(
            ConfigurationLayerManager manager)
            {
            //arrange
            var configurationId = "Test";
            
            //act
            Action configuration = () => manager.GetConfigurationById(configurationId);

            //assert
            configuration.Should().Throw<KeyNotFoundException>();

            }
    }
    }