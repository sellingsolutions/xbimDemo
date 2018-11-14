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

            string ModelName = "albano.ifc";
            string Path = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + ModelName;

            new IFCCleaner(Path).Run();

            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
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
