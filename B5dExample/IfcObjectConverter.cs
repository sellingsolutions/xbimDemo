using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildcraftCore.B5D;
using Xbim.Ifc4.Interfaces;

namespace B5dExample
{
    public class IfcObjectConverter
    {
        public static B5dProjectNode Convert(IIfcObjectDefinition ifcObject)
        {
            var ifcObjectGuid = ifcObject.GlobalId.Value.ToString();
            var node = B5dProjectNode.GetNodeWithIfcGuid(ifcObjectGuid);

            if (node != null)
            {
                return node;
            }

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
                ExternalIfcGlobalId = ifcObjectGuid,
                B5dObject = b5dNode
            };

            return b5dNode;
        }
    }
}
