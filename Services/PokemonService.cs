using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using pokedex.Models;

namespace pokedex.Services
{
    public class PokemonService: IPokemonService
    {
           private readonly IMongoCollection<Pokemon> _pokemonCollection;
        public async Task<Pokemon> UpdatePokemon(string id, Pokemon updatedPokemon)
        {
            var filter = Builders<Pokemon>.Filter.Eq(p => p.Id, id);
            var result = await _pokemonCollection.ReplaceOneAsync(filter, updatedPokemon);

            if (result.MatchedCount == 0)
            {
                throw new Exception("Pokemon not found");
            }
            return updatedPokemon;
        }


        public PokemonService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _pokemonCollection = database.GetCollection<Pokemon>(config["DatabaseSettings:CollectionName"]);
        }


        public async Task<List<Pokemon>> GetPokemons()
        {
            return await _pokemonCollection.Find(pokemon => true).ToListAsync();
        }

        public async Task<Pokemon> GetPokemonByName(string name)
        {
            return await _pokemonCollection.Find(pokemon => pokemon.Name == name).FirstOrDefaultAsync();
        }

       public async Task<Pokemon> AddPokemon(Pokemon newPokemon)
        {
            newPokemon.Id = ObjectId.GenerateNewId().ToString(); // Generate a new ObjectId
            await _pokemonCollection.InsertOneAsync(newPokemon);
            return newPokemon;
        }

        public async Task<Pokemon> GetPokemonById(string id)
        {
            return await _pokemonCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        
        public async Task<bool> DeletePokemon(string id)
        {
            var filter = Builders<Pokemon>.Filter.Eq(p => p.Id, id);
            var result = await _pokemonCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

    }
}