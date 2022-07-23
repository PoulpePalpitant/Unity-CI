using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Xml;


/*
 * **********************************************************************
 * Author   : Matthew ventures
 * Date     : 10/06/2021

 * Brief    : Outil de sauvegarde
 * 
 * In-depth : Permet de faire de la sérialization et de l'encryption 
 * 
 * Reference: https://www.mrventures.net/all-tutorials/serialization-encryption
 * **********************************************************************
*/


namespace Utils
{
    static class Data
    {
        public static string SerializeXML<T>(object inputData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, inputData);
                    return sww.ToString();
                }
            }
        }

        public static T DeserializeXML<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var sww = new StringReader(data))
            {
                using (XmlReader reader = XmlReader.Create(sww))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        static byte[] ivBytes = new byte[16]; // Generate the iv randomly and send it along with the data, to later parse out
        static byte[] keyBytes = new byte[16]; // Generate the key using a deterministic algorithm rather than storing here as a variable

        static void GenerateIVBytes()
        {
            System.Random rnd = new System.Random();
            rnd.NextBytes(ivBytes);
        }

        const string nameOfGame = "Flask";
        static void GenerateKeyBytes()
        {
            int sum = 0;
            foreach (char curChar in nameOfGame)
                sum += curChar;

            System.Random rnd = new System.Random(sum);
            rnd.NextBytes(keyBytes);
        }

        public static string EncryptAES(string data)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(keyBytes, ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer);

            return ivString + encryptedString;
        }

        public static string DecryptAES(this string text)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            int endOfIVBytes = ivBytes.Length / 2;  // Half length because unicode characters are 64-bit width

            string ivString = text.Substring(0, endOfIVBytes);
            byte[] extractedivBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = text.Substring(endOfIVBytes);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(keyBytes, extractedivBytes);
            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string decryptedString = Encoding.Unicode.GetString(outputBuffer);

            return decryptedString;
        }
    }
}
