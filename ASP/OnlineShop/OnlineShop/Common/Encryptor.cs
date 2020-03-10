using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineShop.Common
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(text));

            byte[] bytes = md5.Hash;
            int length = bytes.Length;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                strBuilder.Append(bytes[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}