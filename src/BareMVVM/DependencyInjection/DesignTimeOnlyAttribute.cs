using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection.Conditions;
using Grace.DependencyInjection.Attributes.Interfaces;

namespace BareMVVM.DependencyInjection
{
    public class DesignTimeOnlyAttribute : Attribute, IExportConditionAttribute
    {
        public IExportCondition ProvideCondition(Type attributedType)
        {
            return new DesignTimeOnlyCondition();
        }
    }
}
