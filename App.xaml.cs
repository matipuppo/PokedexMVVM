using Microsoft.Extensions.DependencyInjection;

namespace PokeDexMVVM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new Views.StartPage());
        }
    }
}