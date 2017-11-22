using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokusaj
{
    class ProcessDES : CommonProcess
    {
        public ProcessDES()
        {
            
        }
            
        #region Encryption Process
        public override/*static*/ string EncryptionStart(string text, string key, bool IsTextBinary)
        {
            #region Get 16 sub-keys using key

            string hex_key = this.FromTextToHex(key);
            string binary_key = this.FromHexToBinary(hex_key);
          //  string key_plus = this.DoPermutation(binary_key, DESData.pc_1);

          

            #endregion

            #region Encrypt process

            
            string binaryText = "";

            if (IsTextBinary == false)
            {
                binaryText = this.FromTextToBinary(text);
            }
            else
            {
                binaryText = text;
            }

            binaryText = this.setTextMutipleOf64Bits(binaryText);

           

            StringBuilder EncryptedTextBuilder = new StringBuilder(binaryText.Length);

            for (int i = 0; i < (binaryText.Length / 64); i++)
            {
               // string PermutatedText = this.DoPermutation(binaryText.Substring(i * 64, 64), DESData.ip);

                string L0 = "", R0 = "";

                


               // EncryptedTextBuilder.Append(FinalText);

            }

            return EncryptedTextBuilder.ToString();

            #endregion
        }
        #endregion

        

        #region Transform a text to a hex
        public string FromTextToHex(string text)
        {
            string hexstring = "";

            foreach (char word in text)
            {
                hexstring += String.Format("{0:X}", Convert.ToInt32(word));
            }

            return hexstring;
        }
        #endregion

        #region Transform a hex or a binary number to text
        public string FromHexToText(string hexstring)
        {
            StringBuilder text = new StringBuilder(hexstring.Length / 2);

            for (int i = 0; i < (hexstring.Length / 2); i++)
            {
                string word = hexstring.Substring(i * 2, 2);
                text.Append((char)Convert.ToInt32(word, 16));
            }

            return text.ToString();
        }

        public /*static*/ string FromBinaryToText(string binarystring)
        {
            StringBuilder text = new StringBuilder(binarystring.Length / 8);

            for (int i = 0; i < (binarystring.Length / 8); i++)
            {
                string word = binarystring.Substring(i * 8, 8);
                text.Append((char)Convert.ToInt32(word, 2));
                //text += (char)Convert.ToInt32(word, 16);
            }

            return text.ToString();
        }
        #endregion

        #region Set a length of text to multiple of 64 bits
        public string setTextMutipleOf64Bits(string text)
        {
            if ((text.Length % 64) != 0)
            {
                int maxLength = 0;
                maxLength = ((text.Length / 64) + 1) * 64;
                text = text.PadRight(maxLength, '0');
            }

            return text;
        }
        #endregion

        #region Transform an integer to binary number
        public string FromTextToBinary(string text)
        {
            StringBuilder binarystring = new StringBuilder(text.Length * 8);

            foreach (char word in text)
            {
                int binary = (int)word;
                int factor = 128;

                for (int i = 0; i < 8; i++)
                {
                    if (binary >= factor)
                    {
                        binary -= factor;
                        binarystring.Append("1");
                    }
                    else
                    {
                        binarystring.Append("0");
                    }
                    factor /= 2;
                }
            }

            return binarystring.ToString();
        }

        public static string FromDeciamlToBinary(int binary)
        {
            if (binary < 0)
            {
                Console.WriteLine("It requires a integer greater than 0.");
                return null;
            }

            string binarystring = "";
            int factor = 128;

            for (int i = 0; i < 8; i++)
            {
                if (binary >= factor)
                {
                    binary -= factor;
                    binarystring += "1";
                }
                else
                {
                    binarystring += "0";
                }
                factor /= 2;
            }

            return binarystring;
        }

        public static byte FromBinaryToByte(string binary)
        {
            byte value = 0;
            int factor = 128;

            for (int i = 0; i < 8; i++)
            {
                if (binary[i] == '1')
                {
                    value += (byte)factor;
                }

                factor /= 2;
            }

            return value;
        }
        #endregion

        #region Transform a hex integer to a binary number
        public string FromHexToBinary(string hexstring)
        {
            string binarystring = "";

            try
            {
                for (int i = 0; i < hexstring.Length; i++)
                {
                    int hex = Convert.ToInt32(hexstring[i].ToString(), 16);

                    int factor = 8;

                    for (int j = 0; j < 4; j++)
                    {
                        if (hex >= factor)
                        {
                            hex -= factor;
                            binarystring += "1";
                        }
                        else
                        {
                            binarystring += "0";
                        }
                        factor /= 2;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " - wrong hexa integer format.");
            }

            return binarystring;
        }

        public override string DecryptionStart(string text, string key, bool IsTextBinary)
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
