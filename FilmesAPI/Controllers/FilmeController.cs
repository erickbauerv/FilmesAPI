using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _contextFilme;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _contextFilme = context;
        _mapper = mapper;
    }

    [HttpPost]
    public CreatedAtActionResult AdicionarFilme([FromBody] FilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        _contextFilme.Filmes.Add(filme);
        _contextFilme.SaveChanges();

        return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<Filme> BuscarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        return _contextFilme.Filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarFilmePorId(int id)
    {
        var filme = _contextFilme.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
        {
            return NotFound();
        }

        return Ok(filme);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] FilmeDTO filmeDTO)
    {
        var filme = _contextFilme.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) 
        {
            return NotFound();
        }
        
        _mapper.Map(filmeDTO, filme);
        _contextFilme.SaveChanges();

        return NoContent();
    }
}
