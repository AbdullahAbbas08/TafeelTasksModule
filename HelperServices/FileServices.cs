using IHelperServices;
using IHelperServices.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace HelperServices
{
    public class FileServices : _HelperService, IFileServices
    {
        private readonly AppSettings _AppSettings;
        private readonly string _RootPath;

        public FileServices(IOptions<AppSettings> appSettings)
        {
            _AppSettings = appSettings.Value;
            _RootPath = _AppSettings.FileSettings.RelativeDirectory;
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Unicode)]
        private extern static uint FindMimeFromData(
            uint pBC,
            [MarshalAs(UnmanagedType.LPStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            uint cbSize,
            [MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed,
            uint dwMimeFlags,
            out uint ppwzMimeOut,
            uint dwReserverd
        );

        public string GetFileMimeType(byte[] content)
        {
            try
            {
                uint mimeType;
                FindMimeFromData(0, null, content, (uint)256, null, 0, out mimeType, 0);

                var mimePointer = new IntPtr(mimeType);
                var mime = Marshal.PtrToStringUni(mimePointer);
                Marshal.FreeCoTaskMem(mimePointer);

                return mime ?? "application/octet-stream";
            }
            catch
            {
                return "application/octet-stream";
            }
        }

        public void SaveFile(string fileName, byte[] content)
        {
            //var subDirectory = fileName.Substring(0, 2);
            var directory = Directory.CreateDirectory(_RootPath); // Directory.CreateDirectory(Path.Combine(_RootPath, subDirectory));
            File.WriteAllBytes(Path.Combine(directory.FullName, fileName), content);
        }

        public byte[] GetFileContent(string fileName)
        {
            //var subDirectory = fileName.Substring(0, 2);
            var subDirectoryFullPath = _RootPath;// Path.Combine(_RootPath, subDirectory);
            var filePaths = Directory.GetFiles(subDirectoryFullPath, fileName + ".*");
            if (filePaths != null && filePaths.Length > 0)
            {
                return File.ReadAllBytes(filePaths[0]);
            }
            else
            {
                return new byte[] { };
            }
        }

        public void DeleteFile(string fileName)
        {
            //var subDirectory = fileName.Substring(0, 2);
            var subDirectoryFullPath = _RootPath;// Path.Combine(_RootPath, subDirectory);
            File.Delete(Path.Combine(subDirectoryFullPath, fileName));
        }

        public void DeleteFiles(string searchPattern)
        {
            //var subDirectory = fileName.Substring(0, 2);
            var subDirectoryFullPath = _RootPath;// Path.Combine(_RootPath, subDirectory);
            Directory.GetFiles(subDirectoryFullPath, searchPattern).AsParallel().ForAll(filePath =>
            {
                File.Delete(filePath);
            });
        }
    }
}