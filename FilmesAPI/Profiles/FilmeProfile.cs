using AutoMapper;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;

namespace FilmesAPI.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<FilmeDTO, Filme>(); 
            CreateMap<Filme, FilmeDTO>();
        }
    }
}
