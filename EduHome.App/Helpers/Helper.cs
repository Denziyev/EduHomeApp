

namespace EduHome.App.Helpers
{
    public class Helper
    {
        public static bool IsImage(IFormFile formFile)
        {
            return formFile.ContentType.Contains("Image");
        }

        public static bool IsSizeOk(IFormFile formFile,double size)
        {
            return (formFile.Length / 1024 / 1024) <= size;
        }

        public static void removeimage(string root,string path,string filename)
        {
            string FullPath=Path.Combine(root,path,filename);
            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
        }
    }
}
