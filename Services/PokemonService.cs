using PokeDexMVVM.Models;
using System.Net;               // Para HttpStatusCode
using System.Net.Http;          // Para HttpClient y HttpRequestException
using System.Text.Json;         // Para JsonSerializer
using System.Threading.Tasks;   // Para Task

namespace PokeDexMVVM.Services
{
    internal class PokemonService
    {
        // Cliente HTTP para hacer las peticiones
        private readonly HttpClient clientehttp;

        // URL base de la API de Pokémon
        private const string UrlBase = $"https://pokeapi.co/api/v2/pokemon";

        // Constructor que inicializa el HttpClient
        public PokemonService()
        {
            
            clientehttp = new HttpClient();
        }

        // Método auxiliar para traducir errores HTTP
        private string ObtenerMensajeError(HttpRequestException ex)
        {
            return ex.StatusCode switch
            {
                HttpStatusCode.NotFound => "Error 404: Recurso no encontrado.",
                HttpStatusCode.InternalServerError => "Error 500: Problema en el servidor de la API.",
                HttpStatusCode.BadRequest => "Error 400: Solicitud inválida.",
                _ => $"Error de conexión o HTTP ({ex.StatusCode}): {ex.Message}"
            };
        }

        // Método para obtener lista de pokemones, trame 99 por defecto
        public async Task<ListaPokemon> ObtenerListaPokemon(int cantidad = 99)
        {
            try
            {
                // Construye la URL con el parámetro de cantidad
                string url = $"{UrlBase}?limit={cantidad}";

                // Peticion GET a la API
                HttpResponseMessage respuesta = await clientehttp.GetAsync(url);

                //Si la respuesta no es exitosa, lanza una excepción con el código de estado
                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(null, null, respuesta.StatusCode);
                }

                // Lee el contenido de la respuesta como string
                string contenidoJson = await respuesta.Content.ReadAsStringAsync();

                // configura el JsonSerializer para ignorar mayúsculas/minúsculas en los nombres de las propiedades
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserializa el JSON a un objeto ListaPokemon y lo devuelve
                return JsonSerializer.Deserialize<ListaPokemon>(contenidoJson, opciones);
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error HTTP, lanza una excepción con un mensaje entendible por el usuario
                throw new Exception(ObtenerMensajeError(ex));
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier otro error, lanza una excepción con un mensaje genérico que incluye el tipo de error
                throw new Exception($"Error inesperado en el servicio ({ex.GetType().Name}): {ex.Message}");
            }
        }

        // Método para obtener detalles de un pokemon
        // Recibe la URL específica del pokemon y devuelve un objeto DetallePokemon con la información detallada
        public async Task<DetallePokemon> ObtenerDetallePokemonAsync(string urlPokemon)
        {
            try
            {
                // Peticion GET a la API para obtener los detalles del pokemon
                HttpResponseMessage respuesta = await clientehttp.GetAsync(urlPokemon);

                // Si la respuesta no es exitosa, lanza una excepción con el código de estado
                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(null, null, respuesta.StatusCode);
                }

                // Lee el contenido de la respuesta como string
                string contenidoJson = await respuesta.Content.ReadAsStringAsync();

                // Configura el JsonSerializer para ignorar mayúsculas/minúsculas en los nombres de las propiedades
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserializa el JSON a un objeto DetallePokemon y lo devuelve
                return JsonSerializer.Deserialize<DetallePokemon>(contenidoJson, opciones);
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error HTTP, lanza una excepción con un mensaje entendible por el usuario
                throw new Exception(ObtenerMensajeError(ex));
            }
            catch (Exception ex)
            {   // Si ocurre cualquier otro error, lanza una excepción con un mensaje genérico que incluye el tipo de error
                throw new Exception($"Error inesperado en el servicio ({ex.GetType().Name}): {ex.Message}");
            }
        }
    }
}

