using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.Mappings
{
    class AttributeMapping : IPropertyMapping
    {
        readonly XAttribute _attribute;
        readonly PropertyInfo _property;
      
        public AttributeMapping(XAttribute attribute, PropertyInfo property)
        {
            _attribute = attribute;
            _property = property;
        }

        public void Apply(object instance)
        {
            try
            {
                var value = TypeDescriptor.GetConverter(_property.PropertyType).ConvertFromString(_attribute.Value);
                _property.SetValue(instance, value, null);
            }
            catch (Exception ex)
            {
                var message =
                    string.Format(
                        "Tried to map attribute '{0}' with value '{1}' to property '{2}' of '{3}', and barfed. See inner exception for details.",
                        _attribute.Name.LocalName,
                        _attribute.Value,
                        _property.Name,
                        instance.GetType());

                throw new AutoConfigMappingException(message, ex);
            }
        }
    }
}