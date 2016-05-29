using Grace.Data;
using Grace.DependencyInjection;
using BareMVVM.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM
{
    public class BareConfiguration : IConfigurationModule
    {
        public void Configure(IExportRegistrationBlock registrationBlock)
        {
            registrationBlock.ExportInstance(new ReflectionService()).
                As<IReflectionService>().
                Lifestyle.Singleton();

            registrationBlock.ExportInstance(new DispatchedMessenger()).
                As<IDispatchedMessenger>().
                Lifestyle.Singleton();
        }
    }
}
