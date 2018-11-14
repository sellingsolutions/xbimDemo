using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace xbimDemo
{
    public class IFCCleaner
    {
        private const string ModelName = "SampleHouse.ifc";
        private const string DefaultPath = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + ModelName;

        private IfcStore Model { get; set; }
        private List<string> SpaceTypes = new List<string>();

        public IFCCleaner(string Path = DefaultPath)
        {
            Model = IfcStore.Open(Path);

            foreach (string SpaceType in new Spaces().SpaceTypes)
            {
                SpaceTypes.Add(SpaceType.ToLower());
            }
            SpaceTypes.Add("entrance hall");
            SpaceTypes.Add("living room");
            SpaceTypes.Add("roof");
        }


        public void Run ()
        {
            var Spaces = Model.Instances.Where<IIfcSpace>(s => s.EntityLabel > 0);
            foreach (IIfcSpace Space in Spaces)
            {
                var Name = $"{Space.LongName}  {Space.Name}".ToLower();
                var Props = GetPropsForProduct(Space);
                foreach (IIfcPropertySingleValue Prop in Props)
                {
                    if (Prop.NominalValue != null && Prop.NominalValue.Value != null)
                    {
                        var PropDescription = $"{Prop.Name}  {Prop.NominalValue.Value}".ToLower();

                        foreach (string SpaceType in SpaceTypes)
                        {
                            if (PropDescription.Contains(SpaceType))
                            {
                                //Space.ObjectType = SpaceType;
                                Console.WriteLine($"PROP: FOUND MATCH! {SpaceType} in {Name}");
                            }
                        }
                    }
                }

                foreach (string SpaceType in SpaceTypes)
                {
                    if (Name.Contains(SpaceType)) {
                        //Space.ObjectType = SpaceType;
                        Console.WriteLine($"Name: FOUND MATCH! {SpaceType} in {Name}");
                    }
                }


                
            }
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
    }
}
