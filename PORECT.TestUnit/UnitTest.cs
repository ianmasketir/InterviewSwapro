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
using Tes.Business;

namespace PORECT.TestUnit
{
    public class PORECTApiUnitTest
    {
        private readonly ProductController _pController;
        private readonly UserController _uController;
        private readonly Mock<IProductRepository> _pServiceMock;
        private readonly Mock<IUserRepository> _uServiceMock;

        public PORECTApiUnitTest()
        {
            _pServiceMock = new Mock<IProductRepository>();
            _uServiceMock = new Mock<IUserRepository>();
            _pController = new ProductController(_pServiceMock.Object);
            _uController = new UserController(_uServiceMock.Object);
        }

        #region User
        [Fact]
        public void GetListUser_CheckUsername()
        {
            // Arrange
            var sampleData = new MsUserResponse
            {
                Username = "v-taufiq"
            };
            _uServiceMock.Setup(service => service.GetListUser(null, null)).Returns(new List<MsUserResponse> { sampleData });

            // Act
            var result = _uController.GetList();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<MsUserResponse>>();
            var returnedData = okResult.Value as IEnumerable<MsUserResponse>;

            returnedData.Should().ContainEquivalentOf(sampleData, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void InsertUser()
        {
            // Arrange
            var dto = new MsUserRequest
            {
                TransactionType = "Insert",
                Username = "v-taufiq",
                FirstName = "Taufiq",
                LastName = "Fitriansyah"
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "User Taufiq Fitriansyah submitted successfully."
            };
            _uServiceMock.Setup(service => service.SubmitUser(It.IsAny<MsUserRequest>())).Returns(response).Verifiable();

            // Act
            var result = _uController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _uServiceMock.Verify(service => service.SubmitUser(It.IsAny<MsUserRequest>()), Times.Once());
        }
        [Fact]
        public void UpdateUser()
        {
            // Arrange
            var dto = new MsUserRequest
            {
                ID = 1,
                TransactionType = "Update",
                Username = "vtaufiq",
                FirstName = "Taufiq_edited",
                LastName = "Fitriansyah_edited"
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "User Taufiq Fitriansyah submitted successfully."
            };
            _uServiceMock.Setup(service => service.SubmitUser(It.IsAny<MsUserRequest>())).Returns(response).Verifiable();

            // Act
            var result = _uController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _uServiceMock.Verify(service => service.SubmitUser(It.IsAny<MsUserRequest>()), Times.Once());
        }
        [Fact]
        public void DeleteUser()
        {
            // Arrange
            var dto = new MsUserRequest
            {
                ID = 1,
                TransactionType = "Delete",
                Username = "vtaufiq"
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "User Taufiq Fitriansyah deleted successfully."
            };
            _uServiceMock.Setup(service => service.SubmitUser(It.IsAny<MsUserRequest>())).Returns(response).Verifiable();

            // Act
            var result = _uController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _uServiceMock.Verify(service => service.SubmitUser(It.IsAny<MsUserRequest>()), Times.Once());
        }
        #endregion User

        #region Product
        [Fact]
        public void GetListProduct_CheckCode()
        {
            // Arrange
            var sampleData = new ProductResponse
            {
                ProductCode = "TES-001"
            };
            _pServiceMock.Setup(service => service.GetListProduct(new())).Returns(new List<ProductResponse> { sampleData });

            // Act
            var result = _uController.GetList();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<MsUserResponse>>();
            var returnedData = okResult.Value as IEnumerable<MsUserResponse>;

            returnedData.Should().ContainEquivalentOf(sampleData, options => options.ExcludingMissingMembers());
        }
        [Fact]
        public void InsertProduct()
        {
            // Arrange
            var dto = new ProductRequest
            {
                TransactionType = "Insert",
                ProductCode = "TES-001",
                Name = "Tes",
                Price = Convert.ToDecimal(1234)
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "Product Tes submitted successfully."
            };
            _pServiceMock.Setup(service => service.SubmitProduct(It.IsAny<ProductRequest>())).Returns(response).Verifiable();

            // Act
            var result = _pController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _pServiceMock.Verify(service => service.SubmitProduct(It.IsAny<ProductRequest>()), Times.Once());
        }
        [Fact]
        public void UpdateProduct()
        {
            // Arrange
            var dto = new ProductRequest
            {
                TransactionType = "Update",
                ProductCode = "TES-001",
                Name = "Tes_edited",
                Price = Convert.ToDecimal(1234567.33)
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "Product Tes submitted successfully."
            };
            _pServiceMock.Setup(service => service.SubmitProduct(It.IsAny<ProductRequest>())).Returns(response).Verifiable();

            // Act
            var result = _pController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _pServiceMock.Verify(service => service.SubmitProduct(It.IsAny<ProductRequest>()), Times.Once());
        }
        [Fact]
        public void DeleteProduct()
        {
            // Arrange
            var dto = new ProductRequest
            {
                ID = 1,
                TransactionType = "Delete",
                ProductCode = "TES-001"
            };
            var response = new TransactionResponse
            {
                IsSuccess = true,
                Message = "Product Tes deleted successfully."
            };
            _pServiceMock.Setup(service => service.SubmitProduct(It.IsAny<ProductRequest>())).Returns(response).Verifiable();

            // Act
            var result = _pController.Submit(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(response);
            _pServiceMock.Verify(service => service.SubmitProduct(It.IsAny<ProductRequest>()), Times.Once());
        }
        #endregion Product

    }
}