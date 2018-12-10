using System;
using Starcounter;
using BuildcraftCore.B5D;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace B5dExample
{
    class Program
    {
        static IFCRepository ifcRepo = new IFCRepository();

        static void Main()
        {
            IIfcProject ifcProject = ifcRepo.Model.Instances.FirstOrDefault<IIfcProject>();
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