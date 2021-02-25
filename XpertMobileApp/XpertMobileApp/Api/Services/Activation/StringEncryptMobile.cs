using System;
using System.Security.Cryptography;
using System.Text;

namespace Xpert.Key_Activator
{
    #region StringEncrypterKeyHashAlgorithm

    /// <summary>
    /// The hash algorithm that is used to compute hash for the encryption key.
    /// </summary>
    public enum StringEncrypterKeyHashAlgorithm
    {
        /// <summary>
        /// 128 bit MD5
        /// </summary>
        MD5,

        /// <summary>
        /// 256 bit SHA2
        /// </summary>
        SHA2_256,
    }

    #endregion


    class StringEncryptMobile
    {
        #region Constants

        /// <summary>
        /// The default hash algorithm that is used to compute hash for the encryption key.
        /// </summary>
        const StringEncrypterKeyHashAlgorithm DefaultKeyHashAlgorithm = StringEncrypterKeyHashAlgorithm.MD5;

        #endregion

        bool _isTouched = true;
        
        #region Constructors

        public StringEncryptMobile(StringEncrypterKeyHashAlgorithm keyHashAlgorithm, string key, string iv = null)
        {
            KeyHashAlgorithm = keyHashAlgorithm;
            Key = key;
            IV = iv;
        }

        public StringEncryptMobile(string key, string iv = null) : this(DefaultKeyHashAlgorithm, key, iv) { }
             
        public StringEncryptMobile() { }

        #endregion


        #region Helpers

        UTF8Encoding _utf8Encoding;

       
        private UTF8Encoding UTF8Encoding
        {
            get { return _utf8Encoding ?? (_utf8Encoding = new UTF8Encoding()); }
        }


        RijndaelManaged _rijndael;
        
        private RijndaelManaged Rijndael
        {
            get
            {
                if (_rijndael == null)
                {
                    // Creates a AES algorithm.
                    _rijndael = new RijndaelManaged();

                    // Sets cipher and padding mode.
                    _rijndael.Mode = CipherMode.CBC;
                    _rijndael.Padding = PaddingMode.PKCS7;

                    // Sets the block size.
                    _rijndael.BlockSize = 128;
                }

                return _rijndael;
            }
        }


        MD5 _md5;
               
        private MD5 MD5
        {
            get { return _md5 ?? (_md5 = new MD5CryptoServiceProvider()); }
        }


        SHA256 _sha256;

       
        private void Prepare()
        {
            if (!_isTouched)
                return;


            // Checks the Key.
            if (Key == null || Key == "")
                throw new InvalidOperationException("The Key can not be null or an empty string.");


            // Initializes the Key.
            switch (KeyHashAlgorithm)
            {
                case StringEncrypterKeyHashAlgorithm.MD5:
                    Rijndael.KeySize = MD5.HashSize;
                    Rijndael.Key = MD5.ComputeHash(UTF8Encoding.GetBytes(Key));
                    break;

                //case StringEncrypterKeyHashAlgorithm.SHA2_256:
                //    Rijndael.KeySize = SHA256.HashSize;
                //    Rijndael.Key = SHA256.ComputeHash(UTF8Encoding.GetBytes(Key));
                //    break;

                default:
                    throw new InvalidOperationException(string.Format("The KeyHashAlgorithm {0} is unknown.", KeyHashAlgorithm));
            }


            // Initializes the IV.
            if (IV == null)
                Rijndael.IV = new byte[Rijndael.BlockSize / 8];
            else
                Rijndael.IV = MD5.ComputeHash(UTF8Encoding.GetBytes(IV));


            _isTouched = false;
        }

        #endregion


        #region Public Properties

        StringEncrypterKeyHashAlgorithm _keyHashAlgorithm = DefaultKeyHashAlgorithm;
                
        public StringEncrypterKeyHashAlgorithm KeyHashAlgorithm
        {
            get { return _keyHashAlgorithm; }
            set
            {
                if (_keyHashAlgorithm == value)
                    return;

                _keyHashAlgorithm = value;
                _isTouched = true;
            }
        }

               
        public int KeySize
        {
            get
            {
                switch (KeyHashAlgorithm)
                {
                    case StringEncrypterKeyHashAlgorithm.MD5:
                        return MD5.HashSize;

                    //case StringEncrypterKeyHashAlgorithm.SHA2_256:
                    //    return SHA256.HashSize;

                    default:
                        throw new InvalidOperationException(string.Format("The KeyHashAlgorithm {0} is unknown.", KeyHashAlgorithm));
                }
            }
        }


        string _key;
                
        public string Key
        {
            get { return _key; }
            set
            {
                if (value == null || value == "")
                    throw new ArgumentException("The key can not be null or an empty string.", "key");

                if (_key == value)
                    return;

                _key = value;
                _isTouched = true;
            }
        }


        string _iv;
                
        public string IV
        {
            get { return _iv; }
            set
            {
                if (_iv == value)
                    return;

                _iv = value;
                _isTouched = true;
            }
        }

        #endregion


        #region Public Methods

        public string Decrypt(string value)
        {
            if (value == null || value == "")
                throw new ArgumentException("The cipher string can not be null or an empty string.");

            // Prepares required values.
            Prepare();

            // Gets an decryptor interface.
            ICryptoTransform transform = Rijndael.CreateDecryptor();

            // Gets the encrypted byte array from the base64 encoded string.
            byte[] encryptedValue = Convert.FromBase64String(value);
            //byte[] encryptedValue = Activatar.Server.StringEncoder32Base.Decode(value);
            //byte[] encryptedValue = Activatar.Server.Base32Converter.FromBase32String(value);

            // Decrypts the byte array.
            byte[] decryptedValue = transform.TransformFinalBlock(encryptedValue, 0, encryptedValue.Length);

            // Returns the string converted from the UTF-8 byte array.
            return UTF8Encoding.GetString(decryptedValue);
        }

        #endregion
    }

}
