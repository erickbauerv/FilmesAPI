using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private static int id = 0;

    [HttpPost]
    public void AdicionarFilme([FromBody] Filme filme)
    {
        filme.Id = id++;
        filmes.Add(filme);
        Console.WriteLine(filme.Titulo);
        Console.WriteLine(filme.Duracao);
    }

    [HttpGet]
    public IEnumerable<Filme> BuscarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        return filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public Filme? BuscarFilmePorId(int id)
    {
        return filmes.FirstOrDefault(filme => filme.Id == id);
    }
}
