using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace PORECT.Helper
{
    public static class FileHelper
    {
        #region Download
        public static DownloadFile Download
        {
            get { return new DownloadFile(); }
        }

        /// <summary>
        /// Download file based on the path provided
        /// </summary>
        /// <param name="path">File's path</param>
        /// <returns>Byte array of the file</returns>
        public static DownloadFile DoDownload(this string path)
        {
            return new DownloadFile(path);
        }

        public class DownloadFile
        {
            private string _path = string.Empty;
            public byte[] FileByte { get; set; }

            public DownloadFile()
            {
                FileByte = new byte[0];
            }
            public DownloadFile(string path)
            {
                this._path = path;
                FileByte = File.ReadAllBytes(_path);
            }
        }
        #endregion Download

    }
}
