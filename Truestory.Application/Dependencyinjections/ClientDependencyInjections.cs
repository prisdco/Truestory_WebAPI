using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Infrastructure.Contracts;
using Truestory.Infrastructure.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Truestory.Domain.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FluentValidation;

using Truestory.Application.UseCase.Validations;
using MediatR;
using Truestory.Application.Extensions;

namespace Truestory.Application.Dependencyinjections
{
    public static class ClientDependencyInjections
    {
        public static IServiceCollection AddApplicationClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IApplicationClientFactory, ApplicationClientFactory>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));


            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorsInModelState = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(pair => pair.Key,
                            pair => pair.Value.Errors.Select(x => x.ErrorMessage))
                        .ToArray();

                    var errorViewModel = new ErrorViewModel();

                    //cycle through the errors and add to response
                    foreach (var (key, value) in errorsInModelState)
                    {
                        foreach (var subError in value)
                        {
                            errorViewModel.Errors.Add(new Error
                            {
                                Code = key,
                                Message = subError
                            });
                        }
                    }

                    var error = Result<List<ListObjectResponse>>.Fail(
                        errorViewModel,
                        $" {StatusCodes.Status400BadRequest.ToString()} : Error occured"
                    );

                    return new BadRequestObjectResult(error);

                };
            });

            services.AddValidatorsFromAssemblyContaining<ObjectCommandValidators>(); 



            #region swagger implementation
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Truestory's - WebApi",
                    Description = "Truestory's Developer web api portal for third party consumption"
                });


            });
            #endregion


            return services;
        }
    }
}
