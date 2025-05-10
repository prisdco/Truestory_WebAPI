using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Application.UseCase.Commands;
using Truestory.Domain.Models;

namespace Truestory.Application.AutoMapperProfiles
{

    public class ApiMapperProfiles : Profile
    {
        public ApiMapperProfiles()
        {           
            CreateMap<CreateObjectCommand, AddDevice>();
            CreateMap<AddDevice, CreateObjectCommand>();

        }
    }
}
