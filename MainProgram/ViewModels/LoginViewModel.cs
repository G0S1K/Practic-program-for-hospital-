using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Main_Program.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class LoginViewModel : INotifyPropertyChanged
    {
        public String Name { get; set; }

#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
    }
}
