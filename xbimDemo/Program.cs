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
            var ModelName = "hmb_knivsta.ifc";
            var ModelPath = ModelName;
            var DestinationPath = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + ModelName.Replace(".ifc", "") + ".wexbim";
            WEXBIMFactory.CreateWEXFileFromIFCModel(ModelPath, DestinationPath);

            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
        }
    }
}
