using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildcraftCore.B5D;
using Xbim.Ifc4;
using Xbim.Ifc4.Interfaces;

using BuildcraftCore.B5D.PropertyRefs;
using BuildcraftCore.B5D.Relationships;
using B5dExample.RelationshipFactories.Core;

using Starcounter;
using Starcounter.Query.Execution;

namespace B5dExample
{
    public class B5DTest
    {
        private IFCRepository ifcRepo = new IFCRepository();

        /*  We don't have to map the ifc location/3d/geometry properties
         *  We can just convert each ifc object to .obj and then to .glTF and store the glTF properties in Starcounter
         *  
         *  Our mission for the demo is to convert ifc models to our B5d data model and save it to the Starcounter database
         *  We will circumvent the complexity of CSG volume geometry in the ifc model by simply converting to .obj and then to .gltf
         *  We will NOT store meshes and blobs inside of the Starcounter database, we will simply be storing a reference to the .gltf file 
         *  Each B5dObject that has 'phsyical' properties such as geometry will have a url to a .gltf file
         * 
         *
         *
         * --------------------------------------------------------------------------------------------------
         * ----------------------------------- Core Relationships -------------------------------------------
         * 1. Use isDecomposedBy to recursively create nodes for Project, Site, Building, Story and Space 
         * 2. Use containsElements to recursively create nodes for Window, Door, Slab, Wall etc
         * 3. Use isDefinedBy to retrieve all of the properties for each node
         * --------------------------------------------------------------------------------------------------
         * 4. 
         * 5. ifcRelAssociatesClassification
        */

        public void CreateProjectTree (IIfcProject ifcProject)
        {
            BuildTree(null, null, ifcProject);
        }

        public B5dProjectNode BuildTree (B5dObject b5dParent, IIfcObjectDefinition ifcParent, IIfcObjectDefinition ifcChild)
        {
            B5dProjectNode b5dChild = IfcObjectConverter.Convert(ifcChild);
            if (b5dParent != null)
            {
                B5dProjectNodeChild ChildRelationship = B5dProjectNodeChild.CreateNodeChildRelation(b5dParent, b5dChild);
            }
            
            if (ifcChild is IIfcObject ifcObject)
            {
                // Use containsElements to recursively create nodes for Window, Door, Slab, Wall etc
                foreach (var ifcSpatialElement in RelContainedInSpatialStructure.ConvertElementsIn(ifcObject))
                {
                    BuildTree(b5dChild, ifcChild, ifcSpatialElement);
                }
                // Use isDecomposedy to recursively create nodes for Project, Site, Building, Story and Space 
                foreach (var ifcSpatialStructure in RelAggregates.ConvertSpatialStructuresIn(ifcObject))
                {
                    BuildTree(b5dChild, ifcChild, ifcSpatialStructure);
                }

                // Use isDefinedBy to convert all of the ifcProperties
                PropertyConverter.ConvertProperties(ifcObject, b5dChild);
            }

            return b5dChild;
        }
    }
}
