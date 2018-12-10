using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BuildcraftCore.B5D;
using BuildcraftCore.B5D.PropertyRefs;
using Xbim.Ifc4.Interfaces;

namespace B5dExample
{
    public class PropertyConverter
    {
        public static List<B5dProperty> ConvertProperties(IIfcObject ifcObject, B5dObject b5dObject)
        {

            var ifcProperties = GetProperties(ifcObject);
            var b5dProperties = new List<B5dProperty>();
            foreach (var prop in ifcProperties)
            {
                var b5dProperty = PropertyConverter.ConvertProperty(prop, b5dObject);
                b5dProperties.Add(b5dProperty);
            }

            return b5dProperties;
        }

        public static IEnumerable<IIfcPropertySingleValue> GetProperties(IIfcObject ifcObject)
        {
            var props = ifcObject.IsDefinedBy
               .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
               .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
               .OfType<IIfcPropertySingleValue>();
            return props;
        }
        public static B5dProperty ConvertProperty(IIfcPropertySingleValue ifcPropValue, B5dObject propOwner)
        {
            if (ifcPropValue.NominalValue == null)
            {
                return null;
            }

            var propName = ifcPropValue.Name.Value.ToString();
            var propValue = ifcPropValue.NominalValue.Value;
            var propType = ifcPropValue.NominalValue.UnderlyingSystemType.Name;

            switch (propType)
            {
                case "Boolean":
                    if (propValue is bool boolValue)
                    {
                        return new B5dBoolean() { Name = propName, Value = boolValue, BelongsTo = propOwner };
                    }
                    break;
                case "Double":
                    if (propValue is double doubleValue)
                    {
                        return new B5dDouble() { Name = propName, Value = doubleValue, BelongsTo = propOwner };
                    }
                    break;
                case "Int64":
                    if (propValue is Int64 int64Value)
                    {
                        return new B5dInteger() { Name = propName, Value = int64Value, BelongsTo = propOwner };
                    }
                    break;
                case "Int32":
                    if (propValue is Int32 int32Value)
                    {
                        return new B5dInteger() { Name = propName, Value = int32Value, BelongsTo = propOwner };
                    }
                    break;
                case "String":
                    if (propValue is string stringValue)
                    {
                        return new B5dString() { Name = propName, Value = stringValue, BelongsTo = propOwner };
                    }
                    break;
                default:
                    return null;

            }

            return null;
        }
    }
}
