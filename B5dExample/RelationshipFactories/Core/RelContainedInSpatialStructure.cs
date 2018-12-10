using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;

using Xbim.Ifc4.Interfaces;
using BuildcraftCore.B5D;
using BuildcraftCore.B5D.Relationships;

namespace B5dExample.RelationshipFactories.Core
{
    /// <summary>
    /// ifcRelContainedInSpatialStructure
    /// ContainsElements
    /// Window, Door, Slab, Wall
    /// </summary>
    public class RelContainedInSpatialStructure
    {
        public static IEnumerable<IIfcProduct> ConvertElementsIn(IIfcObject ifcObject)
        {
            // Use containsElements to recursively create nodes for Window, Door, Slab, Wall etc
            if (ifcObject is IIfcSpatialStructureElement spatialElement)
            {
                IEnumerable<IIfcProduct> elements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                return elements;
            }
            return new List<IIfcProduct>();
        }
    }
}
