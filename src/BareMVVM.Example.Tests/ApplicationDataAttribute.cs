using SimpleFixture.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFixture;
using System.Reflection;
using TestStack.White.UIItems.WindowItems;
using TestStack.White;

namespace BareMVVM.Example.Tests
{
    public class ApplicationDataAttribute : AutoDataAttribute
    {
        private string _applicationName;

        public ApplicationDataAttribute(string applicationName, params object[] parameters) : base(parameters)
        {
            _applicationName = applicationName;
        }

        protected override object ProvideValueForParameter(Fixture fixture, ParameterInfo parameter)
        {
            if(parameter.ParameterType == typeof(Application))
            {
                return Application.Launch(_applicationName);
            }

            return base.ProvideValueForParameter(fixture, parameter);
        }
    }
}
