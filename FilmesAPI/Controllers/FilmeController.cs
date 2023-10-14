using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
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

    /// <summary>
    /// Método para adicionar um filme ao banco de dados.
    /// </summary>
    /// <param name="filmeDTO">DTO de filme com os campos necessários.</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] FilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        _contextFilme.Filmes.Add(filme);
        _contextFilme.SaveChanges();

        return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filme);
    }

    /// <summary>
    /// Método para buscar filmes do banco de dados.
    /// </summary>
    /// <param name="skip">Número de registros para serem pulados na paginação.</param>
    /// <param name="take">Quantidade de registros para buscar por paginação.</param>
    /// <returns>Lista de filmes.</returns>
    [HttpGet]
    public IEnumerable<Filme> BuscarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        return _contextFilme.Filmes.Skip(skip).Take(take);
    }

    /// <summary>
    /// Método para buscar um filme do banco de dados pelo seu id.
    /// </summary>
    /// <param name="id">Id do filme que vai ser buscado.</param>
    /// <returns>IActionResult</returns>
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

    /// <summary>
    /// Método para atualizar um filme.
    /// </summary>
    /// <param name="id">Id do filme que vai ser atualizado.</param>
    /// <param name="filmeDTO">DTO com as informações atualizadas do filme.</param>
    /// <returns>IActionResult</returns>
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

    /// <summary>
    /// Método para atualizar um filme sem a necessidade de passar todas as informações do FilmeDTO.
    /// </summary>
    /// <param name="id">Id do filme que vai ser atualizado.</param>
    /// <param name="patch">Informações que seram atualizadas.</param>
    /// <returns>IActionResult</returns>
    [HttpPatch("{id}")]
    public IActionResult AtualizarFilme(int id, JsonPatchDocument<FilmeDTO> patch)
    {
        var filme = _contextFilme.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
        {
            return NotFound();
        }

        var filmeDTO = _mapper.Map<FilmeDTO>(filme);
        patch.ApplyTo(filmeDTO, ModelState);

        if (!TryValidateModel(filmeDTO))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeDTO, filme);
        _contextFilme.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Método para excluir um fileme do banco de dados.
    /// </summary>
    /// <param name="id">Id do filme que vai ser excluido .</param>
    /// <returns>IActionResult</returns>
    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _contextFilme.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
        {
            return NotFound();
        }

        _contextFilme.Remove(filme);
        _contextFilme.SaveChanges();
        return NoContent();
    }
}