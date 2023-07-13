namespace EduHome.App.Extentions
{
    public static class FileUpload
    {
        public static string createimage(this IFormFile formFile, string root, string path)
        {


            string FileName = Guid.NewGuid().ToString() + formFile.FileName;
            string FullPath = Path.Combine(root, path, FileName);

            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }
            return FileName;
        }
    }
}
