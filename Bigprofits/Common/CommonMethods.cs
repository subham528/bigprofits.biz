using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bigprofits.Common
{
    public class CommonMethods(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string Encrypt(string clearText)
        {
            try
            {
                string encryptionKey = _configuration.GetValue<string>("EncryptionKey")!;

                byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
                using var aes = Aes.Create();
                byte[] salt = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];

                using var pdb = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(encryptionKey), salt, 100_000, HashAlgorithmName.SHA256);

                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<JObject> GetApiData(string url)
        {
            var resReturn = new JObject();
            try
            {
                using var client = new HttpClient();
                var payload = new
                {
                    tokenName = "USDT"
                };
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                resReturn = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
            }
            return resReturn!;
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                string encryptionKey = _configuration.GetValue<string>("EncryptionKey")!;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using var aes = Aes.Create();
                byte[] salt = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];

                using var pdb = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(encryptionKey), salt, 100_000, HashAlgorithmName.SHA256);

                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static String UploadImage(string imgPath, IFormFile file,string root,string strememid)
        {
            string extnesion   = Path.GetExtension(file.FileName);  //Getting Extension in String                    
            imgPath    +=strememid+"_dp"+extnesion;              //setting custom image file name        
            var serverFilePath = Path.Combine(root, imgPath);   //creating string of path where image going to save

            if (File.Exists(serverFilePath)) {              //deleting Image if already present there                
                File.Delete(serverFilePath);
            }
            var stream = File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return  imgPath;
        } 

        public static String UploadImageMUltiple(string imgPath, IFormFile file, string root, string strememid)
        {
            string extnesion = Path.GetExtension(file.FileName);  //Getting Extension in String                    
            imgPath += strememid + DateTime.Now.ToString("yyyyMMddHHmmss") + extnesion;              //setting custom image file name       
            var serverFilePath = Path.Combine(root, imgPath);   //creating string of path where image going to save

            if (File.Exists(serverFilePath))
            {              //deleting Image if already present there                
                File.Delete(serverFilePath);
            }
            var stream = File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return imgPath;
        }

        public static String PanImage(string imgPath, IFormFile file, string root, string strememid)
        {
            string extnesion = Path.GetExtension(file.FileName);  //Getting Extension in String                    
            imgPath += strememid + "_Pan" + extnesion;              //setting custom image file name        
            var serverFilePath = Path.Combine(root, imgPath);   //creating string of path where image going to save

            if (File.Exists(serverFilePath))
            {              //deleting Image if already present there                
                File.Delete(serverFilePath);
            }
            var stream = File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return imgPath;
        }

        public static String AdharImage(string imgPath1, IFormFile file1, string root, string strememid)
        {
            string extnesion = Path.GetExtension(file1.FileName);  //Getting Extension in String                    
            imgPath1 += strememid + "_Adhar" + extnesion;              //setting custom image file name        
            var serverFilePath = Path.Combine(root, imgPath1);   //creating string of path where image going to save

            if (File.Exists(serverFilePath))
            {              //deleting Image if already present there                
                File.Delete(serverFilePath);
            }
            var stream = File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file1.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return imgPath1;
        }

        public static String PassBookImage(string imgPath2, IFormFile file2, string root, string strememid)
        {
            string extnesion = Path.GetExtension(file2.FileName);  //Getting Extension in String                    
            imgPath2 += strememid + "_Passbook" + extnesion;              //setting custom image file name        
            var serverFilePath = Path.Combine(root, imgPath2);   //creating string of path where image going to save

            if (File.Exists(serverFilePath))
            {              //deleting Image if already present there                
                File.Delete(serverFilePath);
            }
            var stream = File.Create(serverFilePath); // Creating serverFilePath/img.jpg 
            file2.CopyTo(stream);  //copying to image location
            stream.Close();//closng file stream
            return imgPath2;
        }


    }


}
