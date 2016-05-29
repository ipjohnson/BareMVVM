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
using System.Collections;

namespace BareMVVM
{
    public class ViewModelLocator : IDictionary<object,object>
    {
        #region implementation
        protected DependencyInjectionContainer _container;

        public DependencyInjectionContainer Container { get { return _container; } }

        public ICollection<object> Keys
        {
            get
            {
                InitializeContainer();

                return _container.GetAllStrategies(ExportsThat.AreExportedAsName(s => s.EndsWith("ViewModel"))).Select(e => e.ExportNames.FirstOrDefault()).ToList<object>(); }
        }

        public ICollection<object> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public object this[object key]
        {
            get
            {
                InitializeContainer();
                return _container.Locate(key.ToString());
            }

            set
            {
                throw new NotImplementedException();
            }
        }

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

                                            if (t.Name.EndsWith("ViewModel"))
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

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(object key)
        {
            return _container.GetStrategy(key.ToString()) != null;
        }

        public bool Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            value = _container.Locate(key.ToString());

            return value != null;
        }

        public void Add(KeyValuePair<object, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<object, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
