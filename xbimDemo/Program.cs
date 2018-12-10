using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xbim.Ifc;
using Xbim.ModelGeometry.Scene;
using B5dExample.RelationshipFactories;
using B5dExample;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;

using Xbim.Ifc4.MeasureResource;
using System.Reflection;
using Xbim.Common.Collections;

namespace xbimDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            string ModelName = "albano.ifc";
            string Path = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + ModelName;
            

            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
        }
        static void PrintRelationshipsInProject()
        {
            var ifcRepo = new IFCRepository();
            var ifcProject = ifcRepo.Model.Instances.FirstOrDefault<IIfcProject>();
            var relationships = ifcRepo.Model.Instances.Where<IIfcRelationship>(r => r.EntityLabel > 0);

            // All? ifcRelationships are either 1 to 1 or 1 to many.. 
            // 1 to many:
            // First prop is always an object
            // Second prop is always a list of objects
            // 1 to 1:
            // First prop is always an object

            // Just to get started..
            // All ifcProps will be b5dProps
            // All ifcObjects will be ProjectNodes
            // All relationships will be ProjectNodeChildren

            /*
             *  Relationship Xbim.Ifc2x3.ProductExtension.IfcRelAssociatesMaterial Has #Props 2
                ------------------------------------------------------------
                relPropValue = Xbim.Ifc2x3.MaterialResource.IfcMaterial : Xbim.Ifc2x3.MaterialResource.IfcMaterial
                object list count 2
                object type = Xbim.Ifc2x3.SharedFacilitiesElements.IfcFurnitureType
                object type = Xbim.Ifc2x3.ProductExtension.IfcFurnishingElement
             */
            //IIfcRelAssociatesMaterial
            foreach (var relationship in relationships)
            {
                var relProps = relationship.GetType().GetProperties().Where(p => p.Name.Contains("Rel")).ToList();
                Console.WriteLine($"-----------------------------------------------------------------------");
                Console.WriteLine($"Relationship {relationship.GetType().Name} Has #Props {relProps.Count}");
                Console.WriteLine($"-----------------------------------------------------------------------");

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
        // Creates a new wexbim viewer file and stores it in the ifcViewer project
        static void CreateWexFile (string IfcFileName)
        {
            string IfcFilePath = "C:\\Users\\zno\\source\\repos\\ifcViewer\\ifcViewer\\IFC_Models\\" + IfcFileName;
            var WexFilePath = "C:\\Users\\zno\\source\\repos\\ifcViewer\\ifcViewer\\wwwroot\\wexbim_files" + IfcFileName.Replace(".ifc", "") + ".wexbim";
            WEXBIMFactory.CreateWEXFileFromIFCModel(IfcFilePath, WexFilePath);
        }
    }
}
