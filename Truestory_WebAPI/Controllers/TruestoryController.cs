using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Truestory.Application.UseCase.Commands;
using Truestory.Application.UseCase.Queries;
using Truestory.Application.UseCase.Validations;
using Truestory.Domain.Models;

namespace Truestory_WebAPI.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TruestoryController : BaseController
    {
        private readonly IValidator<AddDevice> _validator;
        private readonly IValidator<UpdateDevice> _updateDeviceValidator;

        public TruestoryController(IMediator mediator, ILogger<TruestoryController> logger, IConfiguration config, 
            IValidator<AddDevice> validator, IValidator<UpdateDevice> updateDeviceValidator)
        : base(mediator, logger, config)
        {
            _validator = validator;
            _updateDeviceValidator = updateDeviceValidator;
        }
        /// <summary>
        /// This endpoint is used to retrieve a list of objects data from truestory public api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///  GET /listobjects
        ///  {
        ///  }
        /// </remarks>
        ///
        [AllowAnonymous]
        [HttpGet("listobjects")]
        [ProducesResponseType(typeof(Result<PaginatedResult<ListObjectResponse>>), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]

        public async Task<IActionResult> GetListObjects([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            Logger.LogInformation($"Received request to retrieve all objects");
            int page = pageNumber ?? 1;
            int size = pageSize ?? 10;
            var response = await this.Mediator.Send(new GetListObjectsQuery
            {
                PageNumber = page,
                PageSize = size
            });

            if (!response.IsSuccess)
            {
                Logger.LogWarning("Failed to retrieve objects: {Message}", response.Error);
                return BadRequest(ResultViewModel.Fail(response.Error)); // Ensure this contains the failure message.
            }

            Logger.LogInformation($"Finished retriveing objects: {response}");
            Logger.LogInformation("====================================================================================");
            return Ok(response.Value);
        }

        /// <summary>
        /// This endpoint is used to retrieve a list of objects by Ids from truestory api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///  GET /listobjectbyids
        ///  {
        ///  }
        /// </remarks>
        ///
        [AllowAnonymous]
        [HttpGet("listobjectbyids")]
        [ProducesResponseType(typeof(Result<IEnumerable<ListObjectResponse>>), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]

        public async Task<IActionResult> GetListObjectByIds([FromQuery] string[] Id)
        {
            if (Id.Length == 0)
                return BadRequest(ResultViewModel.Fail("Validation failed: Id is null or empty"));
            Logger.LogInformation($"Received request to retrieve all objects by Ids");
            var response = await this.Mediator.Send(new GetListObjectsByIdsQuery(Id));

            if (!response.IsSuccess)
            {
                Logger.LogWarning("Failed to retrieve objects: {Message}", response.Error);
                return BadRequest(ResultViewModel.Fail(response.Error)); // Ensure this contains the failure message.
            }

            Logger.LogInformation($"Finished retriveing objects: {response}");
            Logger.LogInformation("====================================================================================");
            return Ok(response.Value);
        }

        /// <summary>
        /// this endpoint is used to Create Object
        /// </summary>
        /// <returns></returns>
        [HttpPost("saveObject")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> CreateObjectAsync([FromBody] AddDevice model)
        {
            var validateObject = await _validator.ValidateAsync(model);

            if (!validateObject.IsValid)
            {
                var errorMessages = string.Join("; ", validateObject.Errors.Select(e => e.ErrorMessage));
                return ResultViewModel.Fail("Validation failed", errorMessages);
            }
            Logger.LogInformation($"Received request to create object");
            var result = await this.Mediator.Send(new CreateObjectCommand(model.name, model.data));
            Logger.LogInformation($"Finished creating object: {result}");
            Logger.LogInformation("====================================================================================");
            return (ResultViewModel)result;
        }

        /// <summary>
        /// this endpoint is used to Update Object
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateObject/{Id}")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> UpdateObjectAsync(string Id, [FromBody] UpdateDevice model)
        {
            var validateObject = await _updateDeviceValidator.ValidateAsync(model);

            if (!validateObject.IsValid)
            {
                var errorMessages = string.Join("; ", validateObject.Errors.Select(e => e.ErrorMessage));
                return ResultViewModel.Fail("Validation failed", errorMessages);
            }

            if(string.IsNullOrEmpty(Id))
                return ResultViewModel.Fail("Validation failed: Id is null or empty");

            Logger.LogInformation($"Received request to update object");
            var result = await this.Mediator.Send(new UpdateObjectCommand(Id, model));
            Logger.LogInformation($"Finished updating object: {result}");
            Logger.LogInformation("====================================================================================");
            return result;
        }

        /// <summary>
        /// this endpoint is used to delete object
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deleteObject/{Id}")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> DeleteEmployeeAsync(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return ResultViewModel.Fail("Validation failed: Id is null or empty");
            Logger.LogInformation($"Received request to delete object");
            var result = await this.Mediator.Send(new DeleteObjectCommand(Id));
            Logger.LogInformation($"Finished deleting object: {result}");
            Logger.LogInformation("====================================================================================");
            return result;
        }
    }
}
