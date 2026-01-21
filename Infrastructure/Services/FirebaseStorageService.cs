using Application.IServices;
using Microsoft.AspNetCore.Http;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _config;

        public FirebaseStorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadCourseImage(string courseName, IFormFile? file)
        {
            string firebaseBucket = _config["Firebase:Bucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";

            if (courseName.EndsWith("/"))
            {
                courseName = courseName.TrimEnd('/');
            }

            fileName = fileName.Replace("/", "-");

            var task = firebaseStorage.Child("Course").Child(courseName).Child(fileName);

            var stream = file.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }

        public async Task<string> UploadUserImage(string userName, IFormFile? file)
        {
            string firebaseBucket = _config["Firebase:Bucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";

            if (userName.EndsWith("/"))
            {
                userName = userName.TrimEnd('/');
            }

            fileName = fileName.Replace("/", "-");

            var task = firebaseStorage.Child("User").Child(userName).Child(fileName);

            var stream = file.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }
    }
}
