using Simple_CRUD_Project.Models;

namespace Simple_CRUD_Project.Abstractions
{
    public interface IFileUpload
    {
        public Task PostFileAsync(IFormFile fileData);

        public Task PostMultiFileAsync(IFormFile[] fileData);

        public Task<TblFiledetail> GetDownloadFileById(Guid fileId);
    }
}
