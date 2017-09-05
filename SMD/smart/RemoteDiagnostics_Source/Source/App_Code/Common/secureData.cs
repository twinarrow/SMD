using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Summary description for secureData
/// </summary>
public class secureData
{
    Byte[] bData;
    Byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
    Byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
    UnicodeEncoding ByteConverter = new UnicodeEncoding();
    TripleDESCryptoServiceProvider objTriple = new TripleDESCryptoServiceProvider();

    public secureData() { }

    public string EncryptConnectionString(string strCS)
    {
        string strEncryptedConnectionString = "";
        bData = ASCIIEncoding.ASCII.GetBytes(strCS);
        strEncryptedConnectionString = Convert.ToBase64String(bData);
        return strEncryptedConnectionString;
    }

    public string DecryptConnectionString(string strCS)
    {
        string strDecryptedConnectionString = "";
        bData = Convert.FromBase64String(strCS);
        strDecryptedConnectionString = ASCIIEncoding.ASCII.GetString(bData);
        return strDecryptedConnectionString;
    }

    private byte[] Transform(Byte[] input, ICryptoTransform CryptoTransform)
    {
        MemoryStream memStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
        cryptStream.Write(input, 0, input.Length);
        cryptStream.FlushFinalBlock();
        memStream.Position = 0;
        byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
        memStream.Read(result, 0, Convert.ToInt32(result.Length));
        memStream.Close();
        cryptStream.Close();
        return result;
    }

    public string DESEncrypt(string strData)
    {
        Byte[] input;
        Byte[] output;
        input = ByteConverter.GetBytes(strData);
        output = Transform(input, objTriple.CreateEncryptor(key, iv));
        return Convert.ToBase64String(output);
    }

    public string DESDecrypt(string strData)
    {
        Byte[] input;
        Byte[] output;
        input = Convert.FromBase64String(strData);
        output = Transform(input, objTriple.CreateDecryptor(key, iv));
        return ByteConverter.GetString(output);
    }

}