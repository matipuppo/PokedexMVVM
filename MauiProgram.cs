using Microsoft.Extensions.Logging;
using PokeDexMVVM.Repositories;
using PokeDexMVVM.ViewModels;
using PokeDexMVVM.Services;
using PokeDexMVVM.Views;

namespace PokeDexMVVM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            // Ruta donde se va a guardar el archivo de la bd en el dispositivo
            string rutaDb = Path.Combine(FileSystem.AppDataDirectory, "pokedex.db");

            // Singleton: una sola instancia a la bd para tooda la app
            builder.Services.AddSingleton<IPokemonLocalRepository>(new PokemonLocalRepository(rutaDb));

            // Singletos: una sola instancia del servicio HTTP para toda la app
            builder.Services.AddSingleton<PokemonService>();

            //Transient: una nueva instancia del viwModel cada vez que se navega a la pantalla
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<DetailViewModel>();

            //Transient: se crea una nueva instancia de la pagina cada ve que se navea a ella
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
