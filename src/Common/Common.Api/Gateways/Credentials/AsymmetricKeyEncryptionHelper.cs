using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    public static class AsymmetricKeyEncryptionHelper
    {
        private const int SHA1OverheadSize = 42;

        public static string Encrypt(string plainText, string publicKey)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            var modulus = ParseModulusValueFromPublicKey(publicKey);

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var modulusBytes = Convert.FromBase64String(modulus);

            var encryptLength = MapEncryptionAndSegmentLength(modulusBytes, out var segmentLength);

            var hasIncompleteSegment = plainTextBytes.Length % segmentLength != 0;

            var segmentNumber = (!hasIncompleteSegment) ? (plainTextBytes.Length / segmentLength) : ((plainTextBytes.Length / segmentLength) + 1);

            var encryptedBytes = new byte[segmentNumber * encryptLength];

            for (var i = 0; i < segmentNumber; i++)
            {
                int lengthToCopy;

                if (i == segmentNumber - 1 && hasIncompleteSegment)
                {
                    lengthToCopy = plainTextBytes.Length % segmentLength;
                }
                else
                {
                    lengthToCopy = segmentLength;
                }

                var segment = new byte[lengthToCopy];

                Array.Copy(plainTextBytes, i * segmentLength, segment, 0, lengthToCopy);

                var segmentEncryptedResult = EncryptSegment(modulusBytes, segment);

                for (var j = 0; j < segmentEncryptedResult.Length; j++)
                {
                    encryptedBytes[(i * encryptLength) + j] = segmentEncryptedResult[j];
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        private static string ParseModulusValueFromPublicKey(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(System.Text.Encoding.Default.GetString(Convert.FromBase64String(publicKey)));
            var modulusNode = xmlDoc.SelectSingleNode("//RSAParameters/Modulus");

            return modulusNode?.InnerText;
        }

        private static byte[] EncryptSegment(byte[] modulus, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return data;
            }

            var maxAttempts = 3;
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                try
                {
                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        var rsaKeyInfo = rsa.ExportParameters(false);

                        rsaKeyInfo.Modulus = modulus;
                        rsa.ImportParameters(rsaKeyInfo);
                        var encryptedBytes = rsa.Encrypt(data, true);
                        return encryptedBytes;
                    }
                }
                catch (CryptographicException)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    if (attempt == maxAttempts - 1)
                    {
                        throw;
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private static int MapEncryptionAndSegmentLength(byte[] modulus, out int segmentLength)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var rsaKeyInfo = rsa.ExportParameters(false);

                rsaKeyInfo.Modulus = modulus;
                rsa.ImportParameters(rsaKeyInfo);

                var encryptionLength = (rsa.KeySize / 8);

                segmentLength = encryptionLength - SHA1OverheadSize;

                return encryptionLength;
            }
        }
    }
}
