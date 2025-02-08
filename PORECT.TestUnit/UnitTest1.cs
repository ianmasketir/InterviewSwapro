using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PORECT.API.Controllers;
using PORECT.API.Services;
using PORECT.API;
using PORECT.Helper;
using Tes.Domain;
using Newtonsoft.Json;

namespace PORECT.TestUnit
{
    public class UnitTest1
    {
        private readonly ApiProcessHelper _api = new ApiProcessHelper();

        [Fact]
        public void Get()
        {
            var credential = new AppUserRequest { Username = "admin", Password = "password" };
            string json = JsonConvert.SerializeObject(credential);
            string _token = _api.PostString(json, "https://localhost:7278/api/Auth/", "Login", null, false, true);
            var token = JsonConvert.DeserializeObject<ReturnToken>(_token);
            var header = new List<ParamTaskViewModel>
            {
                new ParamTaskViewModel
                {
                    colName = "Authorization",
                    value = string.Concat("Bearer ", token.Token)
                }
            };
            var listUser = _api.Get<List<MsUserResponse>>(new(), "https://localhost:7278/api/User/", "List", true, header);
            Assert.Equal("vtaufiq", listUser.FirstOrDefault().Username);
        }
    }

    public class WeatherForecastControllerTests
    {
        private readonly WeatherForecastController _controller;
        private readonly Mock<IWeatherService> _weatherServiceMock;

        public WeatherForecastControllerTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _controller = new WeatherForecastController(_weatherServiceMock.Object);
        }

        [Fact]
        public void GetWeatherForecast_ShouldReturnOkResult()
        {
            // Arrange
            var sampleData = new List<WeatherForecast>
            {
                new WeatherForecast { Date = DateTime.Now, TemperatureC = 25, Summary = "Sunny" }
            };
            _weatherServiceMock.Setup(service => service.ListWeatherForecast()).Returns(sampleData);

            // Act
            var result = _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(sampleData);
        }
    }
}