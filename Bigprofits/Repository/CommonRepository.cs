using Bigprofits.Data;

namespace Bigprofits.Repository
{
    public class CommonRepository
    {
        public CommonRepository()
        {

        }
        public static string UploadImage(string imgPath, IFormFile file, string root, string strememid)
        {
            string extnesion = Path.GetExtension(file.FileName);  //Getting Extension in String                    
            imgPath += strememid + "_dp" + extnesion;              //setting custom image file name        
            var serverFilePath = Path.Combine(root, imgPath);   //creating string of path where image going to save

            if (System.IO.File.Exists(serverFilePath))
            {              //deleting Image if already present there                
                System.IO.File.Delete(serverFilePath);
            }
            var stream = System.IO.File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return imgPath;
        }

        public class Swal
        {
            public int Error { get; set; }
            public string? Type { get; set; }
            public string? Title { get; set; }
            public string? Text { get; set; }
        }

    }
}
