
using BareMVVM.DependencyInjection;
using Grace.DependencyInjection;
using Grace.DependencyInjection.Attributes.Interfaces;
using Grace.DependencyInjection.Conditions;
using Grace.DependencyInjection.Lifestyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel;
using BareMVVM.Ultilities;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using BareMVVM.CustomType;
using System.Windows.Controls;
#endif

namespace BareMVVM
{
#if NETFX_CORE
    public abstract class ViewModelLocator
#else
    public abstract class ViewModelLocator : ICustomTypeProvider
#endif
    {
        protected DependencyInjectionContainer _container;
        protected bool _inDesignMode;

        public DependencyInjectionContainer Container { get { return _container; } }

        protected virtual void InitializeContainer()
        {
            _inDesignMode = DesignModeUtility.DesignModeIsEnabled;

            if (_container == null)
            {
                _container = new DependencyInjectionContainer { new BareConfiguration() };

                ConfigureAssembly(GetType().GetTypeInfo().Assembly);
            }
        }

        protected virtual void ConfigureAssembly(Assembly assembly)
        {
            _container.Configure(c => c.Export(assembly.ExportedTypes).
                                        ByTypes(ExportByInterfaces).
                                        ByName(ExportByName).
                                        ProcessAttributes().
                                        UsingLifestyle(LifestylePicker));
        }

        private string ExportByName(Type t)
        {
            var typeInfo = t.GetTypeInfo();

            if (t.Name.EndsWith("ViewModel") ||
               typeof(Window).GetTypeInfo().IsAssignableFrom(typeInfo) ||
               typeof(Page).GetTypeInfo().IsAssignableFrom(typeInfo) ||
               typeof(UserControl).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                return t.Name;
            }

            return null;
        }

        private IEnumerable<Type> ExportByInterfaces(Type exportingType)
        {
            if (!_inDesignMode && exportingType.GetTypeInfo().GetCustomAttributes().Any(a => a is DesignTimeOnlyAttribute))
            {
                yield break;
            }

            // TODO: Handle generic interfaces
            foreach(var interfaceType in exportingType.GetTypeInfo().ImplementedInterfaces)
            {
                yield return interfaceType;
            }
        }

        protected virtual IExportCondition ConditionsMethod(Type type)
        {
            var conditionAttribute =
                (IExportConditionAttribute)type.GetTypeInfo().GetCustomAttributes().FirstOrDefault(a => a is IExportConditionAttribute);

            if (conditionAttribute != null)
            {
                return conditionAttribute.ProvideCondition(type);
            }

            return null;
        }

        protected virtual ILifestyle LifestylePicker(Type typePicker)
        {
            var lifestyleAttribute =
                (ILifestyleProviderAttribute)typePicker.GetTypeInfo().GetCustomAttributes(true).FirstOrDefault(a => a is ILifestyleProviderAttribute);

            if (lifestyleAttribute != null)
            {
                return lifestyleAttribute.ProvideLifestyle(typePicker);
            }

            if (typePicker.Name.EndsWith("Singleton"))
            {
                return new SingletonLifestyle();
            }

            return null;
        }

#if !NETFX_CORE
        public Type GetCustomType()
        {
            if (_container == null)
            {
                InitializeContainer();
            }

            return new ViewModelLocatorType(_container.RootScope, GetType());
        }
#endif
    }
}
