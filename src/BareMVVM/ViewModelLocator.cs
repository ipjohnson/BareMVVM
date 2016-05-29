
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

        public DependencyInjectionContainer Container {  get { return _container; } }

        protected virtual void InitializeContainer()
        {
            if (_container == null)
            {
                _container = new DependencyInjectionContainer();

                ConfigureAssembly(GetType().GetTypeInfo().Assembly);
            }         
        }

        protected virtual void ConfigureAssembly(Assembly assembly)
        {
            _container.Configure(c => c.Export(assembly.ExportedTypes).
                                        ByInterfaces().
                                        ByName(t =>
                                        {
                                            var typeInfo = t.GetTypeInfo();

                                            if(t.Name.EndsWith("ViewModel") || 
                                               typeof(Window).GetTypeInfo().IsAssignableFrom(typeInfo) ||
                                               typeof(Page).GetTypeInfo().IsAssignableFrom(typeInfo) ||
                                               typeof(UserControl).GetTypeInfo().IsAssignableFrom(typeInfo))
                                            {
                                                return t.Name;
                                            }

                                            return null;
                                        }).
                                        AndCondition(ConditionsMethod).                                                                                
                                        ImportAttributedMembers().
                                        UsingLifestyle(LifestylePicker).
                                        WithPriority(t =>
                                        {
                                            if (t.GetTypeInfo().GetCustomAttributes(true).Any(a => a is DesignTimeOnlyAttribute))
                                            {
                                                return 1;
                                            }

                                            return 0;
                                        }));
        }

        protected virtual IExportCondition ConditionsMethod(Type type)
        {
            var conditionAttribute = 
                (IExportConditionAttribute)type.GetTypeInfo().GetCustomAttributes().FirstOrDefault(a => a is IExportConditionAttribute);

            if(conditionAttribute != null)
            {
                return conditionAttribute.ProvideCondition(type);
            }

            return null;
        }

        protected virtual ILifestyle LifestylePicker(Type typePicker)
        {
            var lifestyleAttribute = 
                (ILifestyleProviderAttribute)typePicker.GetTypeInfo().GetCustomAttributes(true).FirstOrDefault(a => a is ILifestyleProviderAttribute);

            if(lifestyleAttribute != null)
            {
                return lifestyleAttribute.ProvideLifestyle(typePicker);
            }

            if(typePicker.Name.EndsWith("Singleton"))
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
