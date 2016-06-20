using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection.Conditions;
using Grace.DependencyInjection.Attributes.Interfaces;

namespace BareMVVM.DependencyInjection
{
    public class DesignTimeOnlyAttribute : Attribute, IExportPriorityAttribute
    {
        public int ProvidePriority(Type attributedType)
        {
            return 10;
        }
    }
}
