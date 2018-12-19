using Starcounter;
using Starcounter.Linq;
using System.Linq;

using BuildcraftCore.B5D;
using System.IO;

using Xbim.Ifc4;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using System;

namespace B5dExample
{
    [Database]
    public class GltfRef: FileRef
    {
        public static GltfRef TryCreatingGltfRef(string ifcObjectGuid, B5dProjectNode project, B5dObject b5dObject)
        {
            var existingRef = DbLinq.Objects<GltfRef>().FirstOrDefault(o => o.BelongsTo == b5dObject);
            if (existingRef != null)
            {
                return existingRef;
            }

            string documentsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string conversionPath = $"{documentsDirectory}\\Conversions";

            var projectName = project.Name;
            var b5dObjectType = b5dObject.TypeSpecifier.ToLower();
            b5dObjectType = b5dObjectType.Replace("ifc", "");
            // The ifcTo3DTiles conversion renames WallStandardCase to Wall
            b5dObjectType = b5dObjectType.Replace("wallstandardcase", "wall");

            var ifcDirectory = $"{conversionPath}\\Conversion_{projectName}\\IFCs";
            var files = Directory.GetFiles(ifcDirectory);
            var ifcFiles = files.Where(f => f.ToLower().Contains(b5dObjectType));
            if (ifcFiles.Count() == 0)
            {
                return null;
            }

            foreach (string ifcFilePath in ifcFiles)
            {
                var ifcModel = IfcStore.Open(ifcFilePath);
                var ifcObject = ifcModel.Instances.FirstOrDefault() as IIfcObject;
                if (ifcObject.GlobalId == ifcObjectGuid)
                {
                    var gltfFilepath = ifcFilePath.Replace("IFCs","GLTFs").Replace(".ifc", ".gltf");
                    FileStream gltfFile = File.Open(gltfFilepath, FileMode.Open);
                    var filename = Path.GetFileName(gltfFilepath);

                    if (ifcObject != null && gltfFile != null)
                    {
                        var fileRef = new GltfRef()
                        {
                            BelongsTo = b5dObject,
                            Extension = "gltf",
                            FileId = ifcObjectGuid,
                            FileName = filename,
                            MimeType = "model/gltf+json",
                            FileSize = gltfFilepath.Length,
                        };
                        return fileRef;
                    }
                    break;
                }
            }

            return null;

        }
    }
}
