using System;
using System.Linq;
using BuildcraftCore.B5D;
using BuildcraftCore.B5D.Relationships;

using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4;

namespace B5dExample.RelationshipFactories
{
    /*
     *  Type	                IfcRelAssociatesClassification
        Oid	                    65766
        GlobalId	            2tjvP$E_5FRxpR1YBWvlT3
        OwnerHistory	        IfcOwnerHistory (65948)
        Name	                Uniformat Classification
        Description	
        RelatedObjects	        IfcFurnitureType (3cUkl32yn9qRSPvBJVyYG2)
        RelatingClassification	IfcClassificationReference (66187)
     * 
     * 
     */

    public class RelAssociatesClassification
    {
        public static void ConvertClassificationIn(IIfcObject ifcObject)
        {
            var classificationRelations = ifcObject.Model.Instances.Where<IfcRelAssociatesClassification>(r => r.RelatedObjects.Contains(ifcObject));
            if (classificationRelations == null)
            { 
                return;
            }
            
            foreach (IIfcRelAssociatesClassification classificationRelation in classificationRelations)
            {
                foreach (var type in classificationRelation.RelatedObjects)
                {
                    var b5dType = new B5dType()
                    {
                        InstanceTypeSpecifier = type.ExpressType.Name.ToString(),
                    };
                    //var typeProps = PropertyConverter.ConvertProperties(type, b5dType);
                    
                    
                }
                

                //var b5dRelationship = new B5dPhysical()
                //{
                //    WhatIs = new B5dType()
                //    {
                //         Name = classificationRelation.
                //    }
                //}
            }
        }
        public static BuildcraftCore.B5D.IfcRoot CreateRootObject(string ifcGuid, B5dObject b5dObject)
        {
            BuildcraftCore.B5D.IfcRoot b5dIfcObject = new BuildcraftCore.B5D.IfcRoot()
            {
                ExternalIfcGlobalId = ifcGuid,
                B5dObject = b5dObject
            };
            return b5dIfcObject;
        }
    }
}
