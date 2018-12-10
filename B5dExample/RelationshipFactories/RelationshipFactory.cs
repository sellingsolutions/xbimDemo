using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildcraftCore.B5D;
using Xbim.Ifc4;
using Xbim.Ifc4.Interfaces;

namespace B5dExample.RelationshipFactories
{
    public class RelationshipFactory
    {
        // All? ifcRelationships are either 1 to 1 or 1 to many.. 
        // 1 to many:
        // First prop is always an object
        // Second prop is always a list of objects
        // 1 to 1:
        // First prop is always an object
        public void ConvertRelationship (IIfcRelationship relationship)
        {
            var relProps = relationship.GetType().GetProperties().Where(p => p.Name.Contains("Rel")).ToList();
            Console.WriteLine($"-----------------------------------------------------------------------");
            Console.WriteLine($"Relationship {relationship.GetType().Name} Has #Props {relProps.Count}");
            Console.WriteLine($"-----------------------------------------------------------------------");

            // 1. Convert ifcRel to a set of B5D relationships, each B5dRel should point to the original ifcRel via the ifcRoot object
            // 2. 

            foreach (var relProp in relProps)
            {
                var relPropValue = relProp.GetValue(relationship);
                var relPropValueType = relPropValue.GetType();


                if (relPropValueType.Name.Contains("ItemSet"))
                {

                    if (relPropValue is ItemSet<long> int64List && int64List.Count > 0)
                    {
                        foreach (var integer in int64List)
                        {
                            Console.WriteLine($"integer = {integer}");
                        }

                    }

                    else if (relPropValue is IEnumerable<object> list && list.ToList().Count > 0)
                    {
                        Console.WriteLine($"1 to Many Relationship: {relProp.Name} {relPropValue.GetType().Name}");
                        foreach (var item in list)
                        {
                            Console.WriteLine($"object type = {item.GetType().Name}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"1 to 1 Relationship: {relProp.Name} {relPropValue.GetType().Name}");
                }
            }
        }
    }
}
