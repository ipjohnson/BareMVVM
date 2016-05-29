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
    public class ViewModelLocatorType : Type
    {
        private Type viewModelType;
        private IInjectionScope _scope;

        public ViewModelLocatorType(IInjectionScope scope, Type type)
        {
            _scope = scope;
            viewModelType = type;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return new ConstructorInfo[0];
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            return null;
        }

        public override Type[] GetInterfaces()
        {
            return new Type[0];
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return new EventInfo[0];
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return new Type[0];
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override Type GetElementType()
        {
            return null;
        }

        protected override bool HasElementTypeImpl()
        {
            return false;
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            var strategy = _scope.GetStrategy(name);

            return new ViewModelLocatorPropertyInfo(_scope, name, strategy.ActivationType);
        }


        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (var strategy in _scope.GetAllStrategies())
            {
                var names = strategy.ExportNames;

                if (names != null)
                {
                    foreach (var name in names)
                    {
                        if (name.EndsWith("ViewModel"))
                        {
                            properties.Add(new ViewModelLocatorPropertyInfo(_scope, name, strategy.ActivationType));
                        }
                    }
                }
            }

            return properties.ToArray();
        }

        protected override MethodInfo GetMethodImpl(string name,
                                                    BindingFlags bindingAttr,
                                                    Binder binder,
                                                    CallingConventions callConvention,
                                                    Type[] types,
                                                    ParameterModifier[] modifiers)
        {
            return null;
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return new MethodInfo[0];
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return new FieldInfo[0];
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return new MemberInfo[0];
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            return false;
        }

        protected override bool IsByRefImpl()
        {
            return false;
        }

        protected override bool IsPointerImpl()
        {
            return false;
        }

        protected override bool IsPrimitiveImpl()
        {
            return false;
        }

        protected override bool IsCOMObjectImpl()
        {
            return false;
        }

        public override object InvokeMember(string name,
                                            BindingFlags invokeAttr,
                                            Binder binder,
                                            object target,
                                            object[] args,
                                            ParameterModifier[] modifiers,
                                            CultureInfo culture,
                                            string[] namedParameters)
        {
            return null;
        }

        public override Type UnderlyingSystemType
        {
            get { return viewModelType; }
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr,
                                                              Binder binder,
                                                              CallingConventions callConvention,
                                                              Type[] types,
                                                              ParameterModifier[] modifiers)
        {
            return null;
        }

        public override string Name
        {
            get { return viewModelType.Name; }
        }

        public override Guid GUID
        {
            get { return viewModelType.GUID; }
        }

        public override System.Reflection.Module Module
        {
            get { return viewModelType.Module; }
        }

        public override Assembly Assembly
        {
            get { return viewModelType.Assembly; }
        }

        public override string FullName
        {
            get { return viewModelType.FullName; }
        }

        public override string Namespace
        {
            get { return viewModelType.Namespace; }
        }

        public override string AssemblyQualifiedName
        {
            get { return viewModelType.AssemblyQualifiedName; }
        }

        public override Type BaseType
        {
            get { return viewModelType.BaseType; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return viewModelType.GetCustomAttributes(attributeType, inherit);
        }

        public override bool ContainsGenericParameters
        {
            get { return viewModelType.ContainsGenericParameters; }
        }

        public override System.Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return viewModelType.CustomAttributes; }
        }

        public override MethodBase DeclaringMethod
        {
            get { return viewModelType.DeclaringMethod; }
        }

        public override Type DeclaringType
        {
            get { return viewModelType.DeclaringType; }
        }

        public override bool Equals(Type o)
        {
            return viewModelType == o;
        }

        public override Type[] FindInterfaces(TypeFilter filter, object filterCriteria)
        {
            return viewModelType.FindInterfaces(filter, filterCriteria);
        }

        public override MemberInfo[] FindMembers(MemberTypes memberType,
                                                 BindingFlags bindingAttr,
                                                 MemberFilter filter,
                                                 object filterCriteria)
        {
            return viewModelType.FindMembers(memberType, bindingAttr, filter, filterCriteria);
        }

        public override GenericParameterAttributes GenericParameterAttributes
        {
            get { return viewModelType.GenericParameterAttributes; }
        }

        public override int GenericParameterPosition
        {
            get { return viewModelType.GenericParameterPosition; }
        }

        public override Type[] GenericTypeArguments
        {
            get { return viewModelType.GenericTypeArguments; }
        }

        public override int GetArrayRank()
        {
            return viewModelType.GetArrayRank();
        }

        public override System.Collections.Generic.IList<CustomAttributeData> GetCustomAttributesData()
        {
            return viewModelType.GetCustomAttributesData();
        }

        public override MemberInfo[] GetDefaultMembers()
        {
            return viewModelType.GetDefaultMembers();
        }

        public override string GetEnumName(object value)
        {
            return viewModelType.GetEnumName(value);
        }

        public override string[] GetEnumNames()
        {
            return viewModelType.GetEnumNames();
        }

        public override Type GetEnumUnderlyingType()
        {
            return viewModelType.GetEnumUnderlyingType();
        }

        public override Array GetEnumValues()
        {
            return viewModelType.GetEnumValues();
        }

        public override EventInfo[] GetEvents()
        {
            return viewModelType.GetEvents();
        }

        public override Type[] GetGenericArguments()
        {
            return viewModelType.GetGenericArguments();
        }

        public override Type[] GetGenericParameterConstraints()
        {
            return viewModelType.GetGenericParameterConstraints();
        }

        public override Type GetGenericTypeDefinition()
        {
            return viewModelType.GetGenericTypeDefinition();
        }

        public override InterfaceMapping GetInterfaceMap(Type interfaceType)
        {
            return viewModelType.GetInterfaceMap(interfaceType);
        }

        public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            return viewModelType.GetMember(name, bindingAttr);
        }

        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            return viewModelType.GetMember(name, type, bindingAttr);
        }

        public override bool IsAssignableFrom(Type c)
        {
            return viewModelType.IsAssignableFrom(c);
        }

        public override bool IsConstructedGenericType
        {
            get { return viewModelType.IsConstructedGenericType; }
        }

        public override bool IsEnum
        {
            get { return viewModelType.IsEnum; }
        }

        public override bool IsEnumDefined(object value)
        {
            return viewModelType.IsEnumDefined(value);
        }

        public override bool IsEquivalentTo(Type other)
        {
            return viewModelType.IsEquivalentTo(other);
        }

        public override bool IsGenericParameter
        {
            get { return viewModelType.IsGenericParameter; }
        }

        public override bool IsGenericType
        {
            get { return viewModelType.IsGenericType; }
        }

        public override bool IsGenericTypeDefinition
        {
            get { return viewModelType.IsGenericTypeDefinition; }
        }

        public override bool IsInstanceOfType(object o)
        {
            return viewModelType.IsInstanceOfType(o);
        }

        public override bool IsSerializable
        {
            get { return viewModelType.IsSerializable; }
        }

        public override bool IsSubclassOf(Type c)
        {
            return viewModelType.IsSubclassOf(c);
        }

        public override RuntimeTypeHandle TypeHandle
        {
            get { return viewModelType.TypeHandle; }
        }

        public override Type MakeArrayType()
        {
            return viewModelType.MakeArrayType();
        }

        public override Type MakeArrayType(int rank)
        {
            return viewModelType.MakeArrayType(rank);
        }

        public override Type MakeByRefType()
        {
            return viewModelType.MakeByRefType();
        }

        public override Type MakeGenericType(params Type[] typeArguments)
        {
            return viewModelType.MakeGenericType(typeArguments);
        }

        public override Type MakePointerType()
        {
            return viewModelType.MakePointerType();
        }

        public override MemberTypes MemberType
        {
            get { return viewModelType.MemberType; }
        }

        public override int MetadataToken
        {
            get { return viewModelType.MetadataToken; }
        }

        public override Type ReflectedType
        {
            get { return viewModelType.ReflectedType; }
        }

        public override System.Runtime.InteropServices.StructLayoutAttribute StructLayoutAttribute
        {
            get { return viewModelType.StructLayoutAttribute; }
        }

        //public override Assembly Assembly { get { return _type.Assembly; } }

        //public override string AssemblyQualifiedName { get { return _type.AssemblyQualifiedName; } }

        //public override Type BaseType { get { return _type.BaseType; } }

        //public override string FullName { get { return _type.FullName; } }

        //public override Guid GUID { get { return _type.GUID; } }


        //public override Module Module { get { return _type.Module; } }


        //public override string Name { get { return _type.Name; } }


        //public override string Namespace { get { return _type.Namespace; } }


        //public override Type UnderlyingSystemType { get { return _type.UnderlyingSystemType; } }


        //public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) { return _type.GetConstructors(bindingAttr); }


        //public override object[] GetCustomAttributes(bool inherit) { return _type.GetCustomAttributes(inherit); }

        //public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        //{
        //    return _type.GetCustomAttributes(attributeType, inherit);
        //}

        //public override Type GetElementType() { return _type.GetElementType(); }

        //public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        //{
        //    return _type.GetEvent(name, bindingAttr);
        //}

        //public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        //{
        //    return _type.GetEvents(bindingAttr);
        //}

        //public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        //{
        //    return _type.GetField(name, bindingAttr);
        //}

        //public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        //{
        //    return _type.GetFields(bindingAttr);
        //}

        //public override Type GetInterface(string name, bool ignoreCase)
        //{
        //    return _type.GetInterface(name, ignoreCase);
        //}

        //public override Type[] GetInterfaces()
        //{
        //    return _type.GetInterfaces();
        //}

        //public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        //{
        //    return _type.GetMembers(bindingAttr);
        //}

        //public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        //{
        //    return _type.GetMethods(bindingAttr);
        //}

        //public override Type GetNestedType(string name, BindingFlags bindingAttr)
        //{
        //    return _type.GetNestedType(name, bindingAttr);
        //}

        //public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        //{
        //    return _type.GetNestedTypes(bindingAttr);
        //}

        //public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        //{
        //    List<PropertyInfo> properties = new List<PropertyInfo>();

        //    foreach (var strategy in _scope.GetAllStrategies())
        //    {
        //        var names = strategy.ExportNames;

        //        if (names != null)
        //        {
        //            foreach (var name in names)
        //            {
        //                if (name.EndsWith("ViewModel"))
        //                {
        //                    properties.Add(new ViewModelLocatorPropertyInfo(_scope, name, strategy.ActivationType));
        //                }
        //            }
        //        }
        //    }

        //    return properties.ToArray();
        //}

        //public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        //{
        //    return _type.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        //}

        //public override bool IsDefined(Type attributeType, bool inherit)
        //{
        //    return _type.IsDefined(attributeType, inherit);
        //}

        //protected override TypeAttributes GetAttributeFlagsImpl()
        //{
        //    return TypeAttributes.Class | TypeAttributes.Public;
        //}

        //protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        //{
        //    var strategy = _scope.GetStrategy(name);

        //    return new ViewModelLocatorPropertyInfo(_scope, name, strategy.ActivationType);
        //}

        //protected override bool HasElementTypeImpl()
        //{
        //    return false;
        //}

        //protected override bool IsArrayImpl()
        //{
        //    return false;
        //}

        //protected override bool IsByRefImpl()
        //{
        //    return false;
        //}

        //protected override bool IsPointerImpl()
        //{
        //    return false;
        //}

        //protected override bool IsPrimitiveImpl()
        //{
        //    return false;
        //}

        //protected override bool IsCOMObjectImpl()
        //{
        //    return false;
        //}

    }
}
