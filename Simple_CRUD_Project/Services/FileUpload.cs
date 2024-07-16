using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Simple_CRUD_Project.Abstractions;
using Simple_CRUD_Project.Models;
using System.IO;

namespace Simple_CRUD_Project.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly MainDBContext _context;

        public FileUpload(MainDBContext context)
        {
            _context = context;
        }

        public async Task PostFileAsync(IFormFile fileData)
        {
            try
            {
                string fileExtension = Path.GetExtension(fileData.FileName).TrimStart('.');
                var fileSizeInBytes = fileData.Length;

                var fileDetails = new TblFiledetail()
                {
                    Id = Guid.NewGuid(),
                    FileName = fileData.FileName,
                    FileType = (FileType) Enum.Parse(typeof(FileType), fileExtension),
                    FileSize = (int)(fileSizeInBytes / 1024),
                    UploadedAt = DateTime.Now,
                };

                using (var stream = new MemoryStream()) 
                { 
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = _context.TblFiledetails.Add(fileDetails);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(IFormFile[] fileData)
        {
            try
            {
                foreach (var file in fileData)
                {
                    string fileExtension = Path.GetExtension(file.FileName).TrimStart('.');
                    var fileSizeInBytes = fileData.Length;

                    var fileDetails = new TblFiledetail()
                    {
                        Id = Guid.NewGuid(),
                        FileName = file.FileName,
                        FileType = (FileType)Enum.Parse(typeof(FileType), fileExtension),
                        FileSize = (int)(fileSizeInBytes / 1024),
                        UploadedAt = DateTime.Now
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = _context.TblFiledetails.Add(fileDetails);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TblFiledetail> GetDownloadFileById(Guid fileId)
        {
            try
            {
                var file = await _context.TblFiledetails.FindAsync(fileId);

                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new FileNotFoundException("File not found in the database.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
