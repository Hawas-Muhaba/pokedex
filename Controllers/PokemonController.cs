using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pokedex.Models;
using pokedex.Services;

namespace pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<List<Pokemon>> Get()
        {
            return await _pokemonService.GetPokemons();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPokemon(string id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonById(id);
                return Ok(pokemon);
            }
            catch (Exception)
            {
                return NotFound("Pokemon not found");
            }
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonByName(string name)
        {
            var pokemon = await _pokemonService.GetPokemonByName(name);
            if (pokemon == null)
            {
                return NotFound("Pokemon not found");
            }
            return Ok(pokemon);
        }

        [HttpPost]
        public async Task<ActionResult<Pokemon>> AddPokemon(Pokemon newPokemon)
        {
            var addedPokemon = await _pokemonService.AddPokemon(newPokemon);
            return CreatedAtAction(nameof(GetPokemon), new { id = addedPokemon.Id }, addedPokemon);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Pokemon>> UpdatePokemon(string id, Pokemon updatedPokemon)
        {
            try
            {
                var pokemon = await _pokemonService.UpdatePokemon(id, updatedPokemon);
                return Ok(pokemon);
            }
            catch (Exception)
            {
                return NotFound("Pokemon not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePokemon(string id)
        {
            var deleted = await _pokemonService.DeletePokemon(id);
            if (!deleted)
            {
                return NotFound("Pokemon not found");
            }
            return NoContent();
        }
    }
}
