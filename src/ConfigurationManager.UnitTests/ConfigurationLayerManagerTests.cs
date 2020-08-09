using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using ConfigurationManager.Models;
using ConfigurationManager.UnitTests.TestModels;
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
            Action configuration = () => manager.GetConfigurationById (configurationId);

            //assert
            configuration.Should ().Throw<KeyNotFoundException> ();
            }

        [Theory]
        [AutoMockData]
        public void
            GetConfigurationValues_WhenCalled_ReturnsExpectedConfigurations(
            ILayer initialLayer,
            ILayer topLayer,
            ConfigurationLayerManager manager)
            {
            //arrange
            var firstConfiguration = new Configuration {ConfigurationId = "Test1", ConfigurationValue = 1};
            var secondInitialConfiguration = new Configuration {ConfigurationId = "Test2", ConfigurationValue = 1};
            var secondTopConfiguration = new Configuration {ConfigurationId = "Test2", ConfigurationValue = 2};

            var expectedValues = new List<Configuration> {firstConfiguration, secondTopConfiguration};

            Mock.Get (initialLayer)
                .Setup (f => f.GetAllConfigurations ())
                .Returns (new List<Configuration> {firstConfiguration, secondInitialConfiguration});

            Mock.Get (topLayer)
                .Setup (f => f.GetAllConfigurations ())
                .Returns (new List<Configuration> {secondTopConfiguration});

            manager.AddLayer (initialLayer);
            manager.AddLayer (topLayer);

            //act
            var configurations = manager.GetAllConfigurationValues ();

            //assert
            configurations.Should ().BeEquivalentTo (expectedValues);
            }


        [Theory]
        [AutoMockData]
        public void
            GetConfigurationDataModel_WhenCorrectModelAvailable_ReturnsExpectedObject(
            ILayer initialLayer,
            ConfigurationLayerManager manager)
            {
            //arrange
            var configurationValue = 1;
            var firstConfiguration = new Configuration
                    {ConfigurationId = "Test1", ConfigurationValue = configurationValue};

            Mock.Get (initialLayer)
                .Setup (f => f.TryGetConfigurationById (firstConfiguration.ConfigurationId, out firstConfiguration))
                .Returns (true);


            manager.AddLayer (initialLayer);
            var expectedModel = new TestModel {Test1 = configurationValue};
            var model = new TestModel ();

            //act
            manager.GetConfigurationDataModel (ref model);

            //assert
            model.Should ().BeEquivalentTo (expectedModel);
            }

        [Theory]
        [AutoMockData]
        public void
            GetConfigurationDataModel_WhenConfigurationsNotAvailable_ThrowsAggregatedException(
            [Frozen] ILayer initialLayer,
            ConfigurationLayerManager manager)
            {
            //arrange
            Mock.Get (initialLayer)
                .Setup (f => f.TryGetConfigurationById (It.IsAny<string> (), out It.Ref<Configuration>.IsAny))
                .Returns (false);

            manager.AddLayer (initialLayer);
            var model = new IncorrectTestModel ();

            //act
            Action getIncorrectModel = () => manager.GetConfigurationDataModel (ref model);

            //assert
            getIncorrectModel.Should ().Throw<AggregateException> ();
            }
        }
    }