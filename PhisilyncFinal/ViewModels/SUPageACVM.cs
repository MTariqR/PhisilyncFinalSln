﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace PhisilyncFinal.ViewModels
{

    public partial class SUPageACVM : BaseViewModel
    {
        [RelayCommand]
        private async Task AthleteDash()
        {
            await Shell.Current.GoToAsync("//athleteDash");
        }


    }


}
