using Grace.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if NETFX_CORE
using Windows.UI.Xaml;
#endif

namespace BareMVVM.LanguageExtensions
{
    public static class FrameworkElementExtensions
    {
        public static IInjectionScope GetScope(this FrameworkElement element)
        {
#if NETFX_CORE
            var locatorObject = element.Resources["Locator"] as ViewModelLocator;
#else
            var locatorObject = element.TryFindResource("Locator") as ViewModelLocator;
#endif            
            if(locatorObject == null)
            {
                throw new Exception("Could not find resource by the name of Locator");
            }

            return locatorObject.Container.RootScope;
        }
    }
}
