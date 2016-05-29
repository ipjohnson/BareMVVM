using Grace.DependencyInjection.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection;
using BareMVVM.Ultilities;

namespace BareMVVM.DependencyInjection
{
    public class DesignTimeOnlyCondition : IExportCondition
    {
        public bool ConditionMeet(IInjectionScope scope, IInjectionContext injectionContext, IExportStrategy exportStrategy)
        {
            return DesignModeUtility.DesignModeIsEnabled;
        }
    }
}
