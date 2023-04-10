using CloudinaryDotNet.Actions;

namespace Expense.Repository.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
