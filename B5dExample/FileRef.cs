using System;
using Starcounter;
using BuildcraftCore.B5D;

namespace B5dExample
{
    [Database]
    public class FileRef: B5dObject
    {
        public B5dObject BelongsTo { get; set; }

        public string FileId { get; set; }

        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }

    }
}
