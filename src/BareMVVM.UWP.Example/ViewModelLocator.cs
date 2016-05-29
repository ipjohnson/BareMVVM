using BareMVVM.UWP.Example.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.UWP.Example
{
    public class ViewModelLocator : BareMVVM.ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel { get { return _container.Locate<MainPageViewModel>(); } }

        public DetailPageViewModel DetailPageViewModel {  get { return _container.Locate<DetailPageViewModel>(); } }
    }
}
