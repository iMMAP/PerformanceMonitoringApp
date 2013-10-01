using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ITSec.DataAccessLayer.DataAccess
{
    public class Cipher
    {
        public String Decrypt(string TextFrom, bool bEncryptDecrypt, String strPassword)
        {
            // Warning!!! Optional parameters not supported
            string encrypted = String.Empty;
            string decrypted = String.Empty;
            string password;

            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash;
            byte[] buff;
            password = "ItSeCZaIGhAm610654025810097284009!";
            //password = strPassword;
            try
            {
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();
                des.Key = pwdhash;
                des.Mode = CipherMode.ECB;
                buff = Convert.FromBase64String(TextFrom);
                decrypted = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
                des = null;
                //Decrypt = decrypted;
            }
            catch (Exception ex)
            {
                string strMessage;
                strMessage = ex.Message;
            }
            return decrypted;
        }

        public String Encrypt(string TextFrom, bool bEncryptDecrypt, String strPassword)
        {
            string original;
            string encrypted = String.Empty;
            string decrypted = String.Empty;
            string password;
            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash;
            byte[] buff;
            password = "ItSeCZaIGhAm610654025810097284009!";
            //password = strPassword;
            original = TextFrom;
            try
            {
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();
                des.Key = pwdhash;
                des.Mode = CipherMode.ECB;
                buff = ASCIIEncoding.ASCII.GetBytes(original);
                encrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
                des = null;
                //Encrypt = encrypted;
            }
            catch (Exception ex)
            {
                string strMessage;
                strMessage = ex.Message;
            }
            return encrypted;
        }

    }
}