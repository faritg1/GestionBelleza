using AutoMapper;
using Api.Dtos;
using Core.Entities;

namespace Api.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Cliente Mappings
            CreateMap<Cliente, ClienteDto>();
            CreateMap<CreateClienteDto, Cliente>();
            CreateMap<UpdateClienteDto, Cliente>();

            // Servicio Mappings
            CreateMap<Servicio, ServicioDto>();
            CreateMap<CreateServicioDto, Servicio>();
            CreateMap<UpdateServicioDto, Servicio>();

            // Cita Mappings
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre))
                .ForMember(dest => dest.ApellidoCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.Apellido))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.IdUsuarioNavigation.NombreCompleto));
            CreateMap<CreateCitaDto, Cita>();

            // Usuario Mappings
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<CreateUsuarioDto, Usuario>();

            // Pago Mappings
            CreateMap<Pago, PagoDto>();
            CreateMap<CreatePagoDto, Pago>();
        }
    }
}