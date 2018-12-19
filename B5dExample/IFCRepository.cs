using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using System.IO;

namespace B5dExample
{
    public class IFCRepository
    {
        private const string DefaultModelName = "SampleHouse.ifc";
        private const string ProjectPath = "C:\\Users\\zno\\source\\repos\\xbimDemo\\B5dExample";
        private const string DefaultModelPath = ProjectPath + "\\" + DefaultModelName;

        public IfcStore Model { get; set; }
        public XbimEditorCredentials Editor = new XbimEditorCredentials
        {
            ApplicationDevelopersName = "Alexander Selling",
            ApplicationFullName = "Buildcraft",
            ApplicationIdentifier = "se.buildcraft",
            ApplicationVersion = "4.0",
            //your user
            EditorsFamilyName = "Alexander Selling",
            EditorsGivenName = "Alexander Selling",
            EditorsOrganisationName = "Buildcraft AB"
        };

        public IFCRepository(string Path = DefaultModelPath)
        {
            try
            {
                Model = IfcStore.Open(Path);
                var modelSize = Model.Instances.Count;
                Console.WriteLine(modelSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public IIfcObject GetProduct(string ifcProductID)
        {
            int ProductID = Convert.ToInt32(ifcProductID);

            var Product = Model.Instances.Where(o => o.EntityLabel == ProductID).FirstOrDefault() as IIfcObject;
            return Product;
        }

        public IEnumerable<IIfcPropertySingleValue> GetPropsForProduct(IIfcObject Object)
        {
            if (Object == null)
            {
                return new List<IIfcPropertySingleValue>();
            }

            var Properties = Object.IsDefinedBy
                .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                .OfType<IIfcPropertySingleValue>();

            return Properties;
        }

       
        public List<IIfcObjectDefinition> GetHierarchy (IIfcObjectDefinition Object)
        {
            var Hierarchy = PrintHierarchy(Object, 0);
            return Hierarchy;
        }

        private List<IIfcObjectDefinition> PrintHierarchy(IIfcObjectDefinition Object, int level)
        {
            var Hierarchy = new List<IIfcObjectDefinition>();

            
            //var TypeObjects = Model.Instances.Where<IIfcTypeObject>(s=>s.EntityLabel > 0);
            var Spaces = Model.Instances.Where<IIfcSpace>(s => s.EntityLabel > 0);
            var Zones = Model.Instances.Where<IIfcZone>(s => s.EntityLabel > 0);
            //var Doors = Model.Instances.Where<IIfcDoor>(s => s.EntityLabel > 0);
            //var Windows = Model.Instances.Where<IIfcWindow>(s => s.EntityLabel > 0);
            //var Walls = Model.Instances.Where<IIfcWall>(s => s.EntityLabel > 0);
            //var Roofs = Model.Instances.Where<IIfcRoof>(s => s.EntityLabel > 0);
            //var SlabTypes = Model.Instances.Where<IIfcSlabType>(s => s.EntityLabel > 0);
            IIfcValue DefaultNominalValue = new IfcText("");

            foreach (IIfcSpace Space in Spaces)
            {
                Console.WriteLine($"Space: {Space.LongName}  {Space.Name}");
                Space.ObjectType = new IfcLabel("");

                var Props = GetPropsForProduct(Space);
                foreach (IIfcPropertySingleValue Prop in Props)
                {
                    if (Prop.NominalValue != null && Prop.NominalValue.Value != null)
                    {
                        Console.WriteLine($"{Prop.Name}  {Prop.NominalValue.Value}");
                    }
                }
            }

            foreach (IIfcZone Zone in Zones)
            {
                Console.WriteLine($"Zone: {Zone.LongName}  {Zone.Name}");

                var Props = GetPropsForProduct(Zone);
                foreach (IIfcPropertySingleValue Prop in Props)
                {
                    if (Prop.NominalValue != null && Prop.NominalValue.Value != null)
                    {
                        Console.WriteLine($"{Prop.Name}  {Prop.NominalValue.Value}");
                    }
                }
            }

            IIfcElement Element = Object as IIfcElement;

            // If we're dealing with an element: Wall, Door, Window..
            // Then we'll have a direct connection to the Spatial Structure that it's contained in
            if (Element != null)
            {
                var Relation = Element.ContainedInStructure.FirstOrDefault() as IIfcRelContainedInSpatialStructure;
                var RelatingStructure = Relation.RelatingStructure;
                Hierarchy.Add(RelatingStructure);

                Console.WriteLine();
            }



            // Only spatial elements can contain building elements
            IIfcSpatialStructureElement SpatialElement = Object as IIfcSpatialStructureElement;
            if (SpatialElement != null)
            {
                //using IfcRelContainedInSpatialElement to get contained elements
                var ContainedElements = SpatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                foreach (var ContainedElement in ContainedElements)
                {

                }

            }

            if (Object != null)
            {
                // ifcRelAggregates to get spatial decomposition of spatial structure elements
                IEnumerable<IIfcObjectDefinition> ifcRelAggregates = Object.IsDecomposedBy.SelectMany(r => r.RelatedObjects);
                if (ifcRelAggregates != null)
                {
                    foreach (var SpatialStructure in ifcRelAggregates)
                    {

                    }
                }
            }           

            return Hierarchy;
        }

        private static string GetIndent(int level)
        {
            var indent = "";
            for (int i = 0; i < level; i++)
                indent += "  ";
            return indent;
        }
    }
}
