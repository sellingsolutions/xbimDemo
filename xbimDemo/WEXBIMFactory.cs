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
    public class WEXBIMFactory
    {
        public static void CreateWEXFileFromIFCModel(string IFCModelName, string DestinationPath)
        {
            string fileName = "C:\\Users\\zno\\source\\repos\\xbimDemo\\xbimDemo\\" + IFCModelName;

            using (var model = IfcStore.Open(fileName))
            {
                var context = new Xbim3DModelContext(model);
                context.CreateContext();

                using (var wexBiMfile = File.Create(DestinationPath))
                {
                    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                    {
                        model.SaveAsWexBim(wexBimBinaryWriter);
                        wexBimBinaryWriter.Close();
                    }
                    wexBiMfile.Close();
                }
            }
        } 
    }
}
