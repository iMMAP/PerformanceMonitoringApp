using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for AccessToken
/// </summary>
/// Authors: Shawn Bullock, Asad Aziz

/// </summary>

namespace OCHA.Security.Library
{

    public enum SwitchType
    {
        On = 0,
        Off = 1
    }

    public class AccessToken
    {

        /***********************************************************************************************/

        private readonly int BIT_COUNT = 8;
        private readonly byte[] BIT_ON = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
        private readonly byte[] BIT_OFF = new byte[] { 0xFE, 0xFD, 0xFB, 0xF7, 0xEF, 0xDF, 0xBF, 0x7F };
        /***********************************************************************************************/
        /// <summary>
        /// Method creates a 
        /// </summary>

        /// <param name="resourceId">The Bit position to be toggeled</param>
        /// <param name="switchType">Inidicated whether to set or clear the bit</param>
        /// <param name="accessToken">The access token to be evaluated</param>
        /// <returns>Modified Access Token</returns>

        public string CreateToken(int resourceId, SwitchType switchType, string accessToken)
        {

            string result = string.Empty;
            int position = 0;
            int count = 0;
            byte[] binary;

            // DONE: Remove the first and last characters from the Token
            // Cconvert the accessToken in to Character Array
            char[] accessChars = accessToken.ToCharArray();
            // If resourceId is less then or equal to CharacterArray times 8 then continue
            //

            if (resourceId <= accessChars.Length * BIT_COUNT)
            {

                // One base instead of Zero base to support Nexsure
                //
                if (resourceId > 0)
                {
                    resourceId--;
                }

                // Convert the Character Array to Byte Array
                //
                binary = System.Text.Encoding.Default.GetBytes(accessChars);
                Array.Reverse(binary);

                // If the bit position is greater then 7 get the byte where the
                //  bit is located
                if (resourceId > 7)
                {
                    position = (byte)Math.Floor((double)(resourceId / BIT_COUNT));
                    count = resourceId - (position * BIT_COUNT);
                }

              // Else we know the bit is located in the first byte
                //
                else
                {
                    position = 0;
                    count = resourceId;
                }

                // Get the byte from the byte Array that holds the bit
                //
                byte x = binary[position];
                //If Switch Type is On then we change the value to 1
                //
                if (switchType == SwitchType.On)
                {
                    x |= BIT_ON[count];
                }

                // else we change the value to 0
                //
                else
                {
                    x &= BIT_OFF[count];
                }

                // Insert the bit back into byte Array
                //

                binary[position] = x;
                result = String.Empty;
                Array.Reverse(binary);
                // Convert the byte Array into string
                //
                result = System.Text.Encoding.Default.GetString(binary, 0, binary.Length);
            }

            else //If resourceId length is greater then the token length throw an exception
            {
                throw new ArgumentOutOfRangeException("ResourceId", resourceId, "ResourceId too large");
            }
            return result;//return the access token to the caller
        }

        /***********************************************************************************************/
        private string ToBinary(byte value)
        {
            string result = "";
            byte x = 0x00;
            for (int index = 0; index < 8; index++)
            {
                x = (byte)((value >> index) & 0x01);
                if (x > 0)
                {
                    result += "1";
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }

        public string AdminToken(int maxId, string accessToken)
        {
            for (int i = 1; i < maxId * 8; i++)
            {
                accessToken = CreateToken(i, SwitchType.On, accessToken);
            }

            return accessToken;
        }

        public string EmptyToken(int maxId, string accessToken)
        {
            for (int i = 1; i < maxId * 8; i++)
            {
                accessToken = CreateToken(i, SwitchType.Off, accessToken);
            }

            return accessToken;
        }

        public string EmptyString()
        {
            string accessToken = "";
            for (int i = 0; i < 128; i++)
            {
                accessToken += "0";
            }

            return accessToken;
        }


        /// <summary>
        /// 

        /// </summary>

        /// <param name="resourceId">The Bit position to check</param>

        /// <param name="accessToken">User access token to be evelauted</param>

        /// <returns>True, if the bit value is 1</returns>

        /// 

        public bool CheckRights(int resourceId, string accessToken)
        {

            int position = 0;
            byte result = (byte)0;
            byte[] binary;
            int count = 0;



            if (!(accessToken == ""))
            {

                // Process the token if the length of resourceId is less then or equal to

                //  token length times 8

                //

                if (resourceId <= accessToken.Length * BIT_COUNT)
                {

                    // One base instead of Zero base to support Nexsure

                    //

                    if (resourceId > 0)
                    {

                        resourceId--;

                    }



                    // Convert the token into byte Array

                    //

                    binary = System.Text.Encoding.Default.GetBytes(accessToken);

                    Array.Reverse(binary);



                    // If the bit position is greater then 7 get the byte where the

                    //  bit is located

                    //

                    if (resourceId > 7)
                    {

                        position = (byte)Math.Floor((double)(resourceId / BIT_COUNT));

                        count = resourceId - (position * BIT_COUNT);

                    }

                  // Else we know the bit is located in the first byte

                  //

                    else
                    {

                        position = 0;

                        count = resourceId;

                    }



                    // Get the byte from the byte Array that holds the bit

                    //

                    byte x = binary[position];



                    // Move the bit to the LeastSignificantPosition (First Position) in the container

                    //

                    result = (byte)(x >> count);



                    // Check whether the bit value is 1 or 0

                    //

                    result &= 0x01;



                    // If the value is 0 return false //User is not authorized to use the module

                    //  else if the value is 1 return true. //User has rights to use the module

                    //

                    if (result == 0)
                    {

                        return false;

                    }

                }

                else
                {

                    throw new ArgumentOutOfRangeException("ResourceId", resourceId, "ResourceId too large");

                }

            }

            return true;

        }

        /***********************************************************************************************/

        public static string ReturnEmptyToken()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(64);
            for (int i = 0; i < 64; i++)
            {
                builder.Append("0");
            }

            return builder.ToString();
        }

        /// <summary>

        /// Removes the First and the last characters from AccessToken

        /// </summary>

        /// <param name="line">Source String</param>

        /// <returns>Updated string</returns>

        public string RemoveSafetyPad(string source)
        {

            string r;

            if (source.Length > 2)
            {

                r = source.Substring(1, source.Length - 2);

            }

            else
            {

                r = "";

            }

            return r;

        }

        /***********************************************************************************************/

    }//End Class



}