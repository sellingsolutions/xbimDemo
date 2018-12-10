using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Xbim.Ifc4.Interfaces;
using BuildcraftCore.B5D;
using BuildcraftCore.B5D.Relationships;

namespace B5dExample.RelationshipFactories.Core
{
    /// <summary>
    /// ifcRelAggregates
    /// IsDecomposedBy
    /// Project, Site, Building, Story, Space
    /// </summary>
    public class RelAggregates
    {
        public static IEnumerable<IIfcObjectDefinition> ConvertSpatialStructuresIn(IIfcObject ifcObject)
        {
            // Use isDecomposedy to recursively create nodes for Project, Site, Building, Story and Space 
            var spatialStructures = ifcObject.IsDecomposedBy.SelectMany(r => r.RelatedObjects);
            return spatialStructures;
        }
    }
}
