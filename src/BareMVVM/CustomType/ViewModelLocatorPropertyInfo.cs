using Grace.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.CustomType
{
    public class ViewModelLocatorPropertyInfo : PropertyInfo
    {
        private static MethodInfo _getMethod;

        static ViewModelLocatorPropertyInfo()
        {
            _getMethod = typeof(ViewModelLocatorPropertyInfo).GetRuntimeMethod("Get", new Type[0]);
        }

        private IInjectionScope _scope;
        private string _propertyName;
        private Type _propertyType;

        public ViewModelLocatorPropertyInfo(IInjectionScope scope, string propertyName, Type propertyType)
        {
            _scope = scope;
            _propertyName = propertyName;
            _propertyType = propertyType;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new Attribute[] { };
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override object GetValue(object obj,
                                        BindingFlags invokeAttr,
                                        Binder binder,
                                        object[] index,
                                        CultureInfo culture)
        {
            return _scope.Locate(_propertyName);
        }

        public override void SetValue(object obj,
                                      object value,
                                      BindingFlags invokeAttr,
                                      Binder binder,
                                      object[] index,
                                      CultureInfo culture)
        {

        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return _getMethod.MakeGenericMethod(_propertyType);
        }

        public T Get<T>()
        {
            return (T)_scope.Locate(_propertyName);
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {            
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return new ParameterInfo[] { };
        }

        public override string Name
        {
            get { return _propertyName; }
        }

        public override Type DeclaringType
        {
            get { return typeof(ViewModelLocator); }
        }

        public override Type ReflectedType
        {
            get { throw new NotImplementedException(); }
        }

        public override Type PropertyType
        {
            get { return _propertyType; }
        }

        public override PropertyAttributes Attributes
        {
            get { return new PropertyAttributes(); }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new Attribute[] { };
        }
    }
}
