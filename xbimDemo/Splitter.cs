using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace xbimDemo
{
    public class Splitter
    {
        XbimEditorCredentials credentials = new XbimEditorCredentials
        {
            ApplicationDevelopersName = "Alexander Selling",
            ApplicationFullName = "Buildcraft",
            ApplicationIdentifier = "se.buildcraft",
            ApplicationVersion = "4.0",
            EditorsFamilyName = "Alexander Selling",
            EditorsGivenName = "Alexander Selling",
            EditorsOrganisationName = "Buildcraft AB"
        };

        public void SplitIntoStories(IfcStore store)
        {
            var stories = store.Instances.OfType<IIfcBuildingStorey>().ToList();
            for (int i = 0; i < stories.Count(); i++)
            {
                var story = stories[i];
                var rels = store.Instances.OfType<IIfcRelContainedInSpatialStructure>().Where(x => x.RelatingStructure.EntityLabel == story.EntityLabel);
                var elements = rels.SelectMany(r => r.RelatedElements).Distinct();

                using (var model = IfcStore.Create(credentials, store.IfcSchemaVersion, XbimStoreType.InMemoryModel))
                {
                    using (var txn = model.BeginTransaction("Data creation"))
                    {
                        var map = new XbimInstanceHandleMap(store, model);
                        model.InsertCopy(elements, true, true, map);

                        txn.Commit();
                    }

                    model.SaveAs($"{i}_{story.Name.ToString()}.ifc", Xbim.IO.IfcStorageType.Ifc);
                    
                }
            }  
        }
    }
}
