using FilmesAPI.Data;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _contextFilme;

    public FilmeController(FilmeContext context)
    {
        _contextFilme = context;
    }

    [HttpPost]
    public CreatedAtActionResult AdicionarFilme([FromBody] Filme filme)
    {
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
}
