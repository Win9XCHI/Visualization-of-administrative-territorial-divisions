using Microsoft.AspNetCore.Http;
using System.IO;

namespace Handler.Models.Authentication
{
    public class Register
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PIB { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public byte[] Diploma { get; set; }
        public IFormFile DiplomaF { get; set; }

        public void ConvertToByte()
        {
            if (DiplomaF != null)
            {
                byte[] Data = null;
                using (var binaryReader = new BinaryReader(DiplomaF.OpenReadStream()))
                {
                    Data = binaryReader.ReadBytes((int)DiplomaF.Length);
                }
                Diploma = Data;
            }
        }
    }
}