using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SixScreensController.Data.Presentation.Entities
{
    public class PresentationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string HashSum { get; set; }

        public PresentationInfo() { }
        public PresentationInfo(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            Name = fileInfo.Name;
            Size = fileInfo.Length;
            using (SHA256 sha256 = SHA256.Create())
            {
                string data = File.ReadAllText(path);
                HashSum = GetHash(sha256, data);
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
