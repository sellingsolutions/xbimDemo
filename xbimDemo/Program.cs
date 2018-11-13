using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xbim.Ifc;
using Xbim.ModelGeometry.Scene;

namespace xbimDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            string ModelName = "thermador_oven.ifc";
            string Path = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + ModelName;

            var ifcRepo = new IFCRepository(Path);
            ifcRepo.GetHierarchy(null);

            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
        }

        static void CreateWexFile (string IfcFileName)
        {
            string IfcFilePath = "C:\\Users\\zno\\source\\repos\\ifcViewer\\ifcViewer\\IFC_Models\\" + IfcFileName;
            var WexFilePath = "C:\\Users\\zno\\source\\repos\\ifcViewer\\ifcViewer\\wwwroot\\wexbim_files" + IfcFileName.Replace(".ifc", "") + ".wexbim";
            WEXBIMFactory.CreateWEXFileFromIFCModel(IfcFilePath, WexFilePath);
        }
    }
}
