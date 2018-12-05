﻿using System;
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


namespace xbimDemo
{
    public class B5DTest
    {
        private IFCRepository ifcRepo = new IFCRepository();

        public void CreateProjectTree ()
        {
            IIfcProject ifcProject = ifcRepo.Model.Instances.FirstOrDefault<IIfcProject>();
            B5dProjectNode b5dProject = new B5dProjectNode();

            BuildTree(null, null, ifcProject);

        }

        public B5dProjectNode CreateNodeFrom(IIfcObjectDefinition ifcObject)
        {
            B5dProjectNode b5dNode = new B5dProjectNode()
            {
                TypeSpecifier = ifcObject.GetType().Name,
                Name = ifcObject.Name
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
                // A Space [WhatIs] is a [NodeChild] to [ToWhat] the Story
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

            var spatialElement = ifcChild as IIfcSpatialStructureElement;
            if (spatialElement != null)
            {
                var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                foreach (var element in containedElements)
                {
                    BuildTree(b5dChild, ifcChild, element);
                    Console.WriteLine($"{element.GetType().Name} - {element.Name} ");
                }
            }

            foreach (var relatedObject in ifcChild.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            {
                Console.WriteLine($"{relatedObject.GetType().Name} - {relatedObject.Name}");
                BuildTree(b5dChild, ifcChild, relatedObject);
            }

            return b5dChild;
        }
    }
}
