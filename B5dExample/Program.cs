using System;
using Starcounter;
using BuildcraftCore.B5D;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace B5dExample
{
    class Program
    {
        static string ModelName = "20160125OTC-Conference Center-IFC4";
        static string ifcModelPath = "\\\\Mac\\Home\\Documents\\IFC_Files\\" + ModelName;

        static IFCRepository ifcRepo = new IFCRepository(ifcModelPath);

        static void Main()
        {

            using (var model = ifcRepo.Model)
            {
                using (var txn = model.BeginTransaction("Modification"))
                {
                    IIfcProject ifcProject = model.Instances.FirstOrDefault<IIfcProject>();
                    ifcProject.Name = "TEST4";

                    var ifcProjectID = ifcProject.GlobalId.ToString();

                    Db.Transact(() =>
                    {
                        new B5DTest().CreateProjectTree(ifcProject);

                        //var root = Db.SQL($"SELECT r FROM {typeof(IfcRoot)} r WHERE r.{nameof(IfcRoot.ExternalIfcGlobalId)} = ?", ifcProjectID).FirstOrDefault();
                        //if (root == null)
                        //{
                        //    new B5DTest().CreateProjectTree(ifcProject);
                        //}

                    });
                }
            }
        }
    }
}