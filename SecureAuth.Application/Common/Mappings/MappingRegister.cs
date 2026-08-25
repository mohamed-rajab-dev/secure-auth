using Mapster;
using SecureAuth.Application.DTOs;
using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Application.Common.Mappings
{
    public class MappingRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, RegisterDto>()
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.UserName, src => src.UserName);
        }
    }
}
