using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace ConfigurationManager.UnitTests
    {
    public class ConfigurationFileReaderTests
        {
        [Fact]
        public void ReadFile_WhenFileContainsInformation_ReturnsExpectedList()
            {
            //arrange
            var configurationFileReader = new ConfigurationFileReader ();
            var expectedList = new List<Configuration>
                {
                new Configuration {ConfigurationId = "ordersPerHour:", ConfigurationValue = "6000"}
                };

            //act
            //TODO: remove hardcoded path
            var configurationList = configurationFileReader.ReadFile (
                "D:\\Development\\Test\\HomeworkConfiguration\\src\\ConfigurationManager.UnitTests\\Base_Config.txt");

            //assert
            configurationList.Should ().BeEquivalentTo (expectedList);
            }

        [Fact]
        public void ReadFile_WhenFileIsEmpty_ThrowsException()
            {
            //arrange
            var configurationFileReader = new ConfigurationFileReader ();

            //act
            Action readFile = () =>
                configurationFileReader.ReadFile (
                    "D:\\Development\\Test\\HomeworkConfiguration\\src\\ConfigurationManager.UnitTests\\Empty_Config.txt");

            //assert
            readFile.Should ().Throw<ArgumentNullException> ();
            }

        [Fact]
        public void ReadFile_WhenNonExistingFilePath_ThrowsException()
            {
            //arrange
            var configurationFileReader = new ConfigurationFileReader ();

            //act
            Action readFile = () => configurationFileReader.ReadFile ("randomFilePath");

            //assert
            readFile.Should ().Throw<FileNotFoundException> ();
            }
        }
    }