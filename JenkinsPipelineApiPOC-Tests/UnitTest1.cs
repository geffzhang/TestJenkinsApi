using JenkinsPipelineApiPOC;
using JenkinsPipelineApiPOC.Controllers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsPipelineApiPOC_tests
{
    public class Tests
    {
        private readonly ILogger<WeatherForecastController> _logger;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestWeatherForecastListCount()
        {
            WeatherForecastController wfc1 = new WeatherForecastController(_logger);
            IEnumerable<WeatherForecast> lst = wfc1.Get();
            Assert.AreEqual(lst.Count() > 0, true);
        }

        [Test]
        public void TestWeatherForecastContainsScorching()
        {
            WeatherForecastController wfc1 = new WeatherForecastController(_logger);
            IEnumerable<WeatherForecast> lst = wfc1.Get();
            Assert.AreEqual(lst.Where(x => x.Summary == "Scorching").Count() > 0, true);
        }
    }

}
