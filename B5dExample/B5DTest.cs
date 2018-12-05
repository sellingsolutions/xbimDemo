using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildcraftCore.B5D;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using BuildcraftCore.B5D.ProjectNodes;
using BuildcraftCore.B5D;


namespace B5dExample
{
    public class B5DTest
    {
        private IFCRepository ifcRepo = new IFCRepository();

        /*  We don't have to map the ifc location/3d/geometry properties
         *  We can just convert each ifc object to obj and then to glTF and store the glTF properties in Starcounter
         *
         * 

         *
         * 1. Use isDecomposedy to recursively create nodes for Project, Site, Building, Story and Space 
         * 2. Use containsElements to recursively create nodes for Window, Door, Slab, Wall etc
         * 3. Use isDefinedBy to retrieve all of the properties for each node
         * 4. 
        */


        public void CreateProjectTree (IIfcProject ifcProject)
        {
            B5dProjectNode b5dProject = new B5dProjectNode();

            BuildTree(null, null, ifcProject);

        }

        public B5dProjectNode CreateNodeFrom(IIfcObjectDefinition ifcObject)
        {
            B5dProjectNode b5dNode = new B5dProjectNode()
            {
                TypeSpecifier = ifcObject.GetType().Name,
                Name = ifcObject.Name,
                WhatIs = new B5dPhysical()
                {
                    TypeSpecifier = ifcObject.GetType().Name,
                    Name = ifcObject.Name
                }
            };

            IfcRoot b5dIfcObject = new IfcRoot()
            {
                ExternalIfcGlobalId = ifcObject.GlobalId,
                B5dObject = b5dNode
            };

            return b5dNode;
        }

        public B5dProjectNodeChild CreateNodeChildFrom(B5dObject b5dParent, B5dObject b5dChild)
        {
            B5dProjectNodeChild ChildRelationship = new B5dProjectNodeChild()
            {
                // A Space [WhatIs] is a NodeChild to [ToWhat] the Story
                WhatIs = b5dChild,
                ToWhat = b5dParent
            };
            return ChildRelationship;
        }

        public B5dProjectNode BuildTree (B5dObject b5dParent, IIfcObjectDefinition ifcParent, IIfcObjectDefinition ifcChild)
        {
            B5dProjectNode b5dChild = CreateNodeFrom(ifcChild);
            if (b5dParent != null)
            {
                B5dProjectNodeChild ChildRelationship = CreateNodeChildFrom(b5dParent, b5dChild);
            }

            // Use containsElements to recursively create nodes for Window, Door, Slab, Wall etc
            if (ifcChild is IIfcSpatialStructureElement spatialElement)
            {
                IEnumerable<IIfcProduct> containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                foreach (var element in containedElements)
                {
                    BuildTree(b5dChild, ifcChild, element);
                }
            }

            // Use isDecomposedy to recursively create nodes for Project, Site, Building, Story and Space 
            foreach (var relatedObject in ifcChild.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            {
                BuildTree(b5dChild, ifcChild, relatedObject);
            }

            return b5dChild;
        }
    }
}
