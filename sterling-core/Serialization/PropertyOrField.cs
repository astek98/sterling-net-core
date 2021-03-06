using System;
using System.Reflection;

namespace Sterling.Core.Serialization
{
    /// <summary>
    ///     Abstraction of property or field
    /// </summary>
    public class PropertyOrField 
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly FieldInfo _fieldInfo;

        public PropertyOrField(object infoObject)
        {
            if (infoObject == null)
            {
                throw new ArgumentNullException("infoObject");
            }

            if (infoObject is PropertyInfo)
            {
                _propertyInfo = (PropertyInfo)infoObject;
            }
            else if (infoObject is FieldInfo)
            {
                _fieldInfo = (FieldInfo)infoObject;
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid type: {0}", infoObject.GetType()), "infoObject");
            }
        }

        public Type PfType
        {
            get { return _propertyInfo == null ? _fieldInfo.FieldType : _propertyInfo.PropertyType; }
        }

        public string Name
        {
            get { return _propertyInfo == null ? _fieldInfo.Name : _propertyInfo.Name; }
        }

        public Type DeclaringType
        {
            get { return _propertyInfo == null ? _fieldInfo.DeclaringType : _propertyInfo.DeclaringType; }
        }

        public object GetValue(object obj)
        {
            return _propertyInfo != null ? _propertyInfo.GetGetMethod().Invoke(obj, new object[] { }) : _fieldInfo.GetValue(obj);
        }

        public Action<object, object> Setter
        {
            get
            {
                if (_propertyInfo != null)
                {
                    return (obj, prop) => _propertyInfo.GetSetMethod().Invoke(obj, new[] { prop });
                }

                return (obj, prop) => _fieldInfo.SetValue(obj, prop);
            }
        }

        public Func<object,object> Getter
        {
            get
            {
                if (_propertyInfo != null)
                {
                    return obj => _propertyInfo.GetGetMethod().Invoke(obj, new object[] { });
                }

                return obj => _fieldInfo.GetValue(obj);
            }
        }

        public override int GetHashCode()
        {
            return _propertyInfo == null ? _fieldInfo.GetHashCode() : _propertyInfo.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyOrField && ((PropertyOrField) obj).PfType.Equals(PfType);
        }

        public override string ToString()
        {
            return _propertyInfo == null ? _fieldInfo.ToString() : _propertyInfo.ToString();
        }

    }
}