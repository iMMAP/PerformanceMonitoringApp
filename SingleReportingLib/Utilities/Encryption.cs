using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;

namespace SingleReporting.Utilities
{
  
    public class Encryption
    {
        static private Byte[] m_Key = new Byte[8];
        static private Byte[] m_IV = new Byte[8];
        static private string key;

        static Encryption()
        {
            Key = "Custom_SecurityKey";
            if (!InitKey(Key))
                throw new Exception("Unable to generate key for encryption.");
        }

        //=============================================================================================
        /// <summary>
        /// Sets or Gets the Encryption Key 
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { if (value == string.Empty || value == null) throw new ApplicationException("Must provide Encryption Key"); key = value; }
        }
        //=============================================================================================
        /// <summary>
        /// 
        /// Description: Encrypt the given string.
        /// </summary>
        /// <param name="strOriginal">Data to be encrypted</param>
        /// <returns>Returns encrypted value</returns>
        //===================================================================================================
        public static string Encrypt(string strOriginal)
        {
            if (string.IsNullOrEmpty(strOriginal)) return strOriginal;

            
            string strResult = "";  // Return result

            // 4. Encrypt the Data
            byte[] rbData = Encoding.ASCII.GetBytes(strOriginal);

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV);

            // 5. Perpare the streams
            // mOut is the output stream.
            // mStream is the input stream.
            // cs is the transformation stream.
            MemoryStream mStream = new MemoryStream(rbData);
            CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);
            MemoryStream mOut = new MemoryStream();

            try
            {
                // 6. Start performing the encryption
                int bytesRead;
                byte[] output = new byte[1024];

                do
                {
                    bytesRead = cs.Read(output, 0, 1024);
                    if (bytesRead != 0)
                        mOut.Write(output, 0, bytesRead);
                } while (bytesRead > 0);

                // 7. Returns the encrypted result after it is base64 encoded
                // In this case, the actual result is converted to base64 so that
                // it can be transported over the HTTP protocol without deformation.
                if (mOut.Length > 0)
                    strResult = Convert.ToBase64String(mOut.ToArray());
            }
            catch (CryptographicException ce)
            {
                strResult = ce.ToString();
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                mOut.Close();
                cs.Close();
                mStream.Close();
            }
            return strResult;
        }
        //===================================================================================================
        /// <summary>
        /// Author:
        /// Description: Decrypt the given string.
        /// Edited by:sz
        /// </summary>
        /// <param name="strEncrypted">Data to be decrypted</param>
        /// <returns>Returns decrypted value</returns>
        //===================================================================================================
        public static string Decrypt(string strEncrypted)
        {
            if (string.IsNullOrEmpty(strEncrypted)) return strEncrypted;

            strEncrypted = strEncrypted.Replace(" ", "+");//sz : if it contains space it means it was passed as QueryString and we need to change it to +

            string strResult;

            // 2. Initialize the service provider
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV);

            // 3. Prepare the streams
            // mOut is the output stream.
            // cs is the transformation stream.
            MemoryStream mOut = new MemoryStream();
            CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);

            try
            {
                // 4. Remember to revert the base64 encoding into a byte array to
                //    restore the original encrypted data stream
                byte[] bPlain = Convert.FromBase64CharArray(strEncrypted.ToCharArray(),
                    0, strEncrypted.Length);

                // 5. Perform the actual decryption
                cs.Write(bPlain, 0, (int)bPlain.Length);
                cs.FlushFinalBlock();

                strResult = Encoding.ASCII.GetString(mOut.ToArray());

                // 6. Trim the string to return only the meaningful data
                // Remember that in the encrypt function, the first 5 character
                // holds the length of the actual data
                // This is the simplest way to remember to original length of the
                // data, without resorting to complicated computations.
                //string strLen = strResult.Substring(0, 5);
                //strResult = strResult.Substring(5, Convert.ToInt32(strLen));
            }
            catch (CryptographicException ce)
            {
                strResult = ce.ToString();
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                cs.Close();
                mOut.Close();
            }
            return strResult;
        }
        //===================================================================================================
        /// <summary>
        /// Author: Asad Aziz
        /// Description: Private function to generate the keys into member
        /// variables.
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        //===================================================================================================
        static private bool InitKey(string strKey)
        {
            try
            {
                // Convert Key to byte array
                byte[] bp = new byte[strKey.Length];
                ASCIIEncoding aEnc = new ASCIIEncoding();
                aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);

                // Hash the key using SHA1
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] bpHash = sha.ComputeHash(bp);

                int lcv;
                // Use the low 64-bits for the key value
                for (lcv = 0; lcv < 8; lcv++)
                    m_Key[lcv] = bpHash[lcv];
                for (lcv = 8; lcv < 16; lcv++)
                    m_IV[lcv - 8] = bpHash[lcv];
                return true;
            }
            catch (Exception)
            {
                // Error Performing Operations
                return false;
            }
        }
        //===================================================================================================
    }


}