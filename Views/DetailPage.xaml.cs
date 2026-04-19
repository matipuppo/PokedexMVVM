using System;
using System.Collections.Generic;
using PokeDexMVVM.ViewModels;

namespace PokeDexMVVM.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            // iniciaiza los componentes de la página y establece el contexto de enlace a una nueva instancia de DetailViewModel
            InitializeComponent();
            BindingContext = new DetailViewModel();
        }
    }
}


