using System.Text;
using System.Security.Cryptography;


namespace MZXRM.PSS.Common
{
    public class EncryptDecrypt
    {
        #region Local Member Variables

        SymmetricAlgorithm cryptographyService;
        private const int CRYPTOKEY_LENGTH = 8;
        private const string CRYPTOKEY = "A22B2CDEFGHIKL";
        private const string INITIALVECTOR = "ABC*^DEFGHIKL";

        #endregion

        /// <summary>
        /// Gets the CryptoKey from config file.It will truncate the key if key is greater than 8 characters.
        /// </summary>
        private string cryptoKey
        {
            get
            {
                string key = CRYPTOKEY;
                if (key.Length > CRYPTOKEY_LENGTH)
                {
                    key = key.Substring(0, CRYPTOKEY_LENGTH);
                }
                return key;
            }

        }

        /// <summary>
        /// Gets the Initialization Vector from config file.It will truncate if  greater than 8 characters.
        /// </summary>
        private string initializationVector
        {
            get
            {
                string vector = INITIALVECTOR;
                if (vector.Length > CRYPTOKEY_LENGTH)
                {
                    vector = vector.Substring(0, CRYPTOKEY_LENGTH);
                }
                return vector;
            }
        }

        /// <summary>
        /// Default constructor for encryption.
        /// </summary>
        public EncryptDecrypt()
        {
            cryptographyService = new DESCryptoServiceProvider();
        }

        /// <summary>
        /// Encrypt the input string using DESCryptoServiceProvider.
        /// </summary>
        /// <param name="inputData">Input string to be encrypted.</param>
        /// <returns>Encrypted string in base64 format.</returns>
        public string EncrypData(string inputData)
        {
            if (string.IsNullOrEmpty(inputData) || string.IsNullOrEmpty(cryptoKey))
            {
                return string.Empty;
            }
            string result;
            byte[] bytInputData = System.Text.ASCIIEncoding.ASCII.GetBytes(inputData);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {

                cryptographyService.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(cryptoKey);
                cryptographyService.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(initializationVector);
                using (CryptoStream crptoStream = new CryptoStream(memoryStream, cryptographyService.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    crptoStream.Write(bytInputData, 0, bytInputData.Length);
                    crptoStream.FlushFinalBlock();
                    result = System.Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                return result;

            }


        }

        /// <summary>
        /// Decrypt the input string to actual data using DESCryptoServiceProvider.
        /// </summary>
        /// <param name="encryptedString">String to be decrypted.</param>
        /// <returns>Decrypted data.</returns>
        public string DecryptData(string encryptedString)
        {
            if (string.IsNullOrEmpty(encryptedString) || string.IsNullOrEmpty(cryptoKey))
                return null;
            encryptedString = encryptedString.Replace(" ", "+");
            byte[] bytencryptData = System.Convert.FromBase64String(encryptedString);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bytencryptData, 0, bytencryptData.Length))
            {
                cryptographyService.Key = ASCIIEncoding.ASCII.GetBytes(cryptoKey); ;
                cryptographyService.IV = ASCIIEncoding.ASCII.GetBytes(initializationVector);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptographyService.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(cryptoStream);
                    string data = reader.ReadToEnd();
                    return data;
                }
            }
        }
    }
}
