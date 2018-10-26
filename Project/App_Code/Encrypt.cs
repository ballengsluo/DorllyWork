using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Encrypt 的摘要说明
/// </summary>
public class Encrypt
{
    public Encrypt()
    {
    }

    //默认密钥向量
    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    public static string Key = "11092718";
    public static string Key1 = "82930127";
    public static string Key2 = "54332432";
    public static string Key3 = "53211124";
    
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="type">类型：后台为1，客户端为2 </param>
    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string type)
    {
        try
        {
            string KeyStr="";
            if (type == "1") KeyStr = Key;
            else if (type == "2") KeyStr = Key1;
            else if (type == "3") KeyStr = Key2;
            else  KeyStr = Key3;

            byte[] rgbKey = Encoding.UTF8.GetBytes(KeyStr.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }    

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string type)
    {
        try
        {
            string KeyStr = "";
            if (type == "1") KeyStr = Key;
            else if (type == "2") KeyStr = Key1;
            else if (type == "3") KeyStr = Key2;
            else KeyStr = Key3;

            byte[] rgbKey = Encoding.UTF8.GetBytes(KeyStr);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }


}

