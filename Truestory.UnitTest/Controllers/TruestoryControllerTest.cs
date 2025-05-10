using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Application.UseCase.Commands;
using Truestory.Application.UseCase.Queries;
using Truestory.Domain.Models;
using Truestory_WebAPI.Controllers;

namespace Truestory.UnitTest.Controllers
{

    public class TruestoryControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<TruestoryController>> _loggerMock;
        private readonly TruestoryController _controller;
        private readonly Mock<IValidator<AddDevice>> _addDeviceValidatorMock;
        private readonly Mock<IValidator<UpdateDevice>> _updateDeviceValidatorMock;

        List<ListObjectResponse> goodResponse = new List<ListObjectResponse>
        { 
            new ListObjectResponse
            {
                id = "1",
                name = "Google Pixel 6 Pro",
                data = new
                {
                    color = "Cloudy White",
                    capacity = "128 GB"
                }
            },
            new ListObjectResponse
            {
                id = "2",
                name = "Apple iPhone 12 Mini, 256GB, Blue",
                data = null
            }
        };

        //List<ListObjectResponse> badResponse = new List<ListObjectResponse>
        //{
        //    null
        //};

        AddDevice createObject = new AddDevice
        {
            name = "Apple MacBook Pro 16",
            data = new AddDeviceProperties {
                  year = 2019,
                  price = 1849.99,
                  CPUmodel = "Intel Core i9",
                  Harddisksize = "1 TB"
               }
        };


        public TruestoryControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<TruestoryController>>();
            _addDeviceValidatorMock = new Mock<IValidator<AddDevice>>();
            _updateDeviceValidatorMock = new Mock<IValidator<UpdateDevice>>();
            _controller = new TruestoryController(_mediatorMock.Object, _loggerMock.Object, _configMock.Object,
                 _addDeviceValidatorMock.Object,
            _updateDeviceValidatorMock.Object);
        }

        [Fact]
        public async Task ListObjectsAsync_ShouldReturnPaginatedResult_WhenSuccessful()
        {
            // Arrange
            var sampleData = new List<ListObjectResponse>
                {
                    new ListObjectResponse { id = "1", name = "Obj1", data = null },
                    new ListObjectResponse { id = "2", name = "Obj2", data = null }
                };

            var paginatedResult = new PaginatedResult<ListObjectResponse>(
                sampleData, count: 2, pageNumber: 1, pageSize: 10
            );

            var mockResult = ResultViewModel.Ok(paginatedResult);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetListObjectsQuery>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetListObjects(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResult<ListObjectResponse>>(okResult.Value);
            Assert.Equal(2, response.Items.Count());
            Assert.Equal(1, response.PageNumber);
        }

        [Fact]
        public async Task ListObjectsByIdAsync_ShouldReturnOkResult_WhenObjectsRetrievedSuccessfully()
        {
            // Arrange


            var mockResult = ResultViewModel.Ok(goodResponse.AsEnumerable());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetListObjectsByIdsQuery>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetListObjectByIds(["7"]);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<ListObjectResponse>>(okResult.Value);
            Assert.Equal(2, response.Count());
            Assert.Equal("1", response.First().id);
        }

        [Fact]
        public async Task ListObjectsAsync_ShouldReturnFail_WhenRetrievalFails()
        {
            // Arrange
            var mockErrorMessage = "Unable to fetch object";
            var mockResult = ResultViewModel.Fail<PaginatedResult<ListObjectResponse>>(mockErrorMessage);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetListObjectsQuery>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetListObjects(pageNumber: 1, pageSize: 10);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResultViewModel>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal(mockErrorMessage, response.Error);
        }

       
        [Fact]
        public async Task CreateObjectAsync_ShouldReturnCreatedObject_WhenSuccessful()
        {

            var command = new AddDevice 
            {
               name = "Apple MacBook Pro 16",
               data = new AddDeviceProperties {
                   year = 2019,
                  price = 1849.99,
                  CPUmodel = "Intel Core i9",
                  Harddisksize = "1 TB"
               }
            };


            _addDeviceValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<AddDevice>(), default))
                .ReturnsAsync(new ValidationResult());

            var mockResult = ResultViewModel.Ok(createObject);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateObjectCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.CreateObjectAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedObject = Assert.IsType<Result<AddDevice>>(result);
            Assert.Equal(createObject.name, returnedObject.Value.name);
        }

        [Fact]
        public async Task CreateObjectAsync_ShouldReturnFailure_WhenObjectCreationFails()
        {
            var command = new AddDevice
            {
                name = "Apple MacBook Pro 16",
                data = new AddDeviceProperties {
                            year = 2019,
                  price = 1849.99,
                  CPUmodel = "Intel Core i9",
                  Harddisksize = "1 TB"
               }
            };


            _addDeviceValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<AddDevice>(), default))
                .ReturnsAsync(new ValidationResult());
            var mockResult = ResultViewModel.Fail("Object Data Failed To Submit.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateObjectCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.CreateObjectAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Object Data Failed To Submit.", result.Error);
        }

        [Fact]
        public async Task UpdateObjectAsync_ShouldReturnSuccessResult_WhenObjectIsUpdatedSuccessfully()
        {

            var command = new UpdateDevice
            {
                name = "Apple MacBook Pro 16",
                data = new UpdateDeviceProperties {
                            year = 2019,
                  price = 1849.99,
                  CPUmodel = "Intel Core i9",
                  Harddisksize = "1 TB",
                  color = "Silver"
               }
            };


            _updateDeviceValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UpdateDevice>(), default))
                .ReturnsAsync(new ValidationResult());

            var mockResult = ResultViewModel.Ok(command);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateObjectCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.UpdateObjectAsync("7", command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedObject = Assert.IsType<Result<UpdateDevice>>(result);
            Assert.Equal(command.name, returnedObject.Value.name);

        }

        [Fact]
        public async Task UpdateObjectAsync_ShouldReturnBadRequest_WhenObjectUpdateFails()
        {
            // Arrange
            var command = new UpdateDevice
            {
                name = "Apple MacBook Pro 16",
                data = new UpdateDeviceProperties {
                            year = 2019,
                  price = 1849.99,
                  CPUmodel = "Intel Core i9",
                  Harddisksize = "1 TB",
                  color = "Silver"
               }
            };


            _updateDeviceValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UpdateDevice>(), default))
                .ReturnsAsync(new ValidationResult());

            var mockResult = ResultViewModel.Fail("Object not found");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateObjectCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.UpdateObjectAsync("7", command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Object not found", result.Error);
        }

        
    }
}
