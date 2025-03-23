namespace AdminDashBoard.Helper
{
    public class PictureSetting
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            var folderpath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Images",folderName);

            var filename=Guid.NewGuid()+file.FileName;

            var filepath=Path.Combine(folderpath,filename);

            var fs=new FileStream(filepath, FileMode.Create);

            file.CopyTo(fs);

            return Path.Combine("Images//Products",filename);
        }

        public static void DeleteFile(string foldername,string filename)
        {
            var Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Images", foldername,filename);
            if (File.Exists(Filepath)) 
            { 
            File.Delete(Filepath);
            
            }


        }
    }
}
