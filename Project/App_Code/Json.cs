using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Text;

public class ConvertJson
{

    #region 私有方法
    /// <summary>
    /// 过滤特殊字符
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>json字符串</returns>
    private static string String2Json(String s)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s.ToCharArray()[i];
            switch (c)
            {
                case '\"':
                    sb.Append("\\\""); break;
                case '\\':
                    sb.Append("\\\\"); break;
                case '/':
                    sb.Append("\\/"); break;
                case '\b':
                    sb.Append("\\b"); break;
                case '\f':
                    sb.Append("\\f"); break;
                case '\n':
                    sb.Append("\\n"); break;
                case '\r':
                    sb.Append("\\r"); break;
                case '\t':
                    sb.Append("\\t"); break;
                default:
                    sb.Append(c); break;
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// 格式化字符型、日期型、布尔型
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string StringFormat(string str, Type type)
    {
        if (type == typeof(string))
        {
            str = String2Json(str);
            str = "\"" + str + "\"";
        }
        else if (type == typeof(DateTime))
        {
            str = "\"" + str + "\"";
        }
        else if (type == typeof(bool))
        {
            str = str.ToLower();
        }
        else if (type != typeof(string) && string.IsNullOrEmpty(str))
        {
            str = "\"" + str + "\"";
        }
        return str;
    }
    #endregion

    #region list转换成JSON
    /// <summary>
    /// list转换为Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ListToJson<T>(IList<T> list)
    {
        object obj = list[0];
        return ListToJson<T>(list, obj.GetType().Name);
    }
    /// <summary>
    /// list转换为json
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="list"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    private static string ListToJson<T>(IList<T> list, string JsonName)
    {
        StringBuilder Json = new StringBuilder();
        if (string.IsNullOrEmpty(JsonName))
            JsonName = list[0].GetType().Name;
        Json.Append("{\"" + JsonName + "\":[");
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] pi = obj.GetType().GetProperties();
                Json.Append("{");
                for (int j = 0; j < pi.Length; j++)
                {
                    Type type = pi[j].GetValue(list[i], null).GetType();
                    Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                    if (j < pi.Length - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < list.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    #endregion

    #region 对象转换为Json
    /// <summary>
    /// 对象转换为json
    /// </summary>
    /// <param name="jsonObject">json对象</param>
    /// <returns>json字符串</returns>
    public static string ToJson(object jsonObject)
    {
        string jsonString = "{";
        PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
        for (int i = 0; i < propertyInfo.Length; i++)
        {
            object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
            string value = string.Empty;
            if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
            {
                value = "'" + objectValue.ToString() + "'";
            }
            else if (objectValue is string)
            {
                value = "'" + ToJson(objectValue.ToString()) + "'";
            }
            else if (objectValue is IEnumerable)
            {
                value = ToJson((IEnumerable)objectValue);
            }
            else
            {
                value = ToJson(objectValue.ToString());
            }
            jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);
        return jsonString + "}";
    }
    #endregion

    #region 对象集合转换为json
    /// <summary>
    /// 对象集合转换为json
    /// </summary>
    /// <param name="array">对象集合</param>
    /// <returns>json字符串</returns>
    public static string ToJson(IEnumerable array)
    {
        string jsonString = "{";
        foreach (object item in array)
        {
            jsonString += ToJson(item) + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);
        return jsonString + "]";
    }
    #endregion

    #region 普通集合转换Json
    /// <summary> 
    /// 普通集合转换Json 
    /// </summary> 
    /// <param name="array">集合对象</param> 
    /// <returns>Json字符串</returns> 
    public static string ToArrayString(IEnumerable array)
    {
        string jsonString = "[";
        foreach (object item in array)
        {
            jsonString = ToJson(item.ToString()) + ",";
        }
        jsonString.Remove(jsonString.Length - 1, jsonString.Length);
        return jsonString + "]";
    }
    #endregion

    #region DataSet转换为Json
    /// <summary> 
    /// DataSet转换为Json 
    /// </summary> 
    /// <param name="dataSet">DataSet对象</param> 
    /// <returns>Json字符串</returns> 
    public static string ToJson(DataSet dataSet)
    {
        string jsonString = "{";
        foreach (DataTable table in dataSet.Tables)
        {
            jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
        }
        jsonString = jsonString.TrimEnd(',');
        return jsonString + "}";
    }
    #endregion

    #region Datatable转换为Json
    /// <summary>  
    /// Datatable转换为Json  
    /// </summary> 
    /// <param name="table">Datatable对象</param>  
    /// <returns>Json字符串</returns>  
    public static string ToJson(DataTable dt)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        DataRowCollection drc = dt.Rows;
        for (int i = 0; i < drc.Count; i++)
        {
            jsonString.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string strKey = dt.Columns[j].ColumnName;
                string strValue = drc[i][j].ToString();
                Type type = dt.Columns[j].DataType;
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (j < dt.Columns.Count - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
            }
            jsonString.Append("},");
        }
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");
        return jsonString.ToString();
    }
    /// <summary> 
    /// DataTable转换为Json  
    /// </summary> 
    public static string ToJson(DataTable dt, string jsonName)
    {
        StringBuilder Json = new StringBuilder();
        if (string.IsNullOrEmpty(jsonName))
            jsonName = dt.TableName;
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Type type = dt.Rows[i][j].GetType();
                    Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    #endregion

    #region DataReader转换为Json
    /// <summary>  
    /// DataReader转换为Json  
    /// </summary>  
    /// <param name="dataReader">DataReader对象</param>  
    /// <returns>Json字符串</returns> 
    public static string ToJson(DbDataReader dataReader)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        while (dataReader.Read())
        {
            jsonString.Append("{");
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Type type = dataReader.GetFieldType(i);
                string strKey = dataReader.GetName(i);
                string strValue = dataReader[i].ToString();
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (i < dataReader.FieldCount - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
            }
            jsonString.Append("},");
        }
        dataReader.Close();
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");
        return jsonString.ToString();
    }
    #endregion
}


public static class JSON
{
    #region
    public static List<LoginUserInfo> Login(string jsonString)
    {
        List<LoginUserInfo> userInfoList = JsonConvert.DeserializeObject<List<LoginUserInfo>>(jsonString);
        return userInfoList;
    }
    public static List<GetUserId> GetUserId(string jsonString)
    {
        List<GetUserId> user = JsonConvert.DeserializeObject<List<GetUserId>>(jsonString);
        return user;
    }
    public static List<ChangePwd> ChangePwd(string jsonString)
    {
        List<ChangePwd> changePwd = JsonConvert.DeserializeObject<List<ChangePwd>>(jsonString);
        return changePwd;
    }
    public static List<UpdateNickName> UpdateNickName(string jsonString)
    {
        List<UpdateNickName> nick = JsonConvert.DeserializeObject<List<UpdateNickName>>(jsonString);
        return nick;
    }
    public static List<ChangeInfo> ChangeInfo(string jsonString)
    {
        List<ChangeInfo> info = JsonConvert.DeserializeObject<List<ChangeInfo>>(jsonString);
        return info;
    }
    public static List<ResetPwd> ResetPwd(string jsonString)
    {
        List<ResetPwd> reset = JsonConvert.DeserializeObject<List<ResetPwd>>(jsonString);
        return reset;
    }

    public static List<GetOrderList> GetOrderList(string jsonString)
    {
        List<GetOrderList> list = JsonConvert.DeserializeObject<List<GetOrderList>>(jsonString);
        return list;
    }
    public static List<getOrderInfo> getOrderInfo(string jsonString)
    {
        List<getOrderInfo> list = JsonConvert.DeserializeObject<List<getOrderInfo>>(jsonString);
        return list;
    }
    public static List<treateOrderInfo> treateOrderInfo(string jsonString)
    {
        List<treateOrderInfo> list = JsonConvert.DeserializeObject<List<treateOrderInfo>>(jsonString);
        return list;
    }
    public static List<appoIntOrderInfo> appoIntOrderInfo(string jsonString)
    {
        List<appoIntOrderInfo> list = JsonConvert.DeserializeObject<List<appoIntOrderInfo>>(jsonString);
        return list;
    }
    public static List<hangUpOrderInfo> hangUpOrderInfo(string jsonString)
    {
        List<hangUpOrderInfo> list = JsonConvert.DeserializeObject<List<hangUpOrderInfo>>(jsonString);
        return list;
    }
    public static List<backOrderInfo> backOrderInfo(string jsonString)
    {
        List<backOrderInfo> list = JsonConvert.DeserializeObject<List<backOrderInfo>>(jsonString);
        return list;
    }
    public static List<applyOrderInfo> applyOrderInfo(string jsonString)
    {
        List<applyOrderInfo> list = JsonConvert.DeserializeObject<List<applyOrderInfo>>(jsonString);
        return list;
    }
    public static List<uploadImg> uploadImg(string jsonString)
    {
        List<uploadImg> list = JsonConvert.DeserializeObject<List<uploadImg>>(jsonString);
        return list;
    }
    public static List<entryOrderCost> entryOrderCost(string jsonString)
    {
        List<entryOrderCost> list = JsonConvert.DeserializeObject<List<entryOrderCost>>(jsonString);
        return list;
    }
    public static List<deleteOrderCost> deleteOrderCost(string jsonString)
    {
        List<deleteOrderCost> list = JsonConvert.DeserializeObject<List<deleteOrderCost>>(jsonString);
        return list;
    }
    public static List<updateOrderCost> updateOrderCost(string jsonString)
    {
        List<updateOrderCost> list = JsonConvert.DeserializeObject<List<updateOrderCost>>(jsonString);
        return list;
    }
    public static List<getMessageList> getMessageList(string jsonString)
    {
        List<getMessageList> list = JsonConvert.DeserializeObject<List<getMessageList>>(jsonString);
        return list;
    }
    public static List<viewMessageList> viewMessageList(string jsonString)
    {
        List<viewMessageList> list = JsonConvert.DeserializeObject<List<viewMessageList>>(jsonString);
        return list;
    }
    public static List<getUserList> getUserList(string jsonString)
    {
        List<getUserList> list = JsonConvert.DeserializeObject<List<getUserList>>(jsonString);
        return list;
    }
    public static List<addOrderUsers> addOrderUsers(string jsonString)
    {
        List<addOrderUsers> list = JsonConvert.DeserializeObject<List<addOrderUsers>>(jsonString);
        return list;
    }

    public static List<entryOrderFee> entryOrderFee(string jsonString)
    {
        List<entryOrderFee> list = JsonConvert.DeserializeObject<List<entryOrderFee>>(jsonString);
        return list;
    }
    public static List<deleteOrderFee> deleteOrderFee(string jsonString)
    {
        List<deleteOrderFee> list = JsonConvert.DeserializeObject<List<deleteOrderFee>>(jsonString);
        return list;
    }
    public static List<updateOrderFee> updateOrderFee(string jsonString)
    {
        List<updateOrderFee> list = JsonConvert.DeserializeObject<List<updateOrderFee>>(jsonString);
        return list;
    }

    public static List<setFee> setFee(string jsonString)
    {
        List<setFee> list = JsonConvert.DeserializeObject<List<setFee>>(jsonString);
        return list;
    }

    public static List<getMeterInfo> getMeterInfo(string jsonString)
    {
        List<getMeterInfo> list = JsonConvert.DeserializeObject<List<getMeterInfo>>(jsonString);
        return list;
    }
    public static List<meterReadout> meterReadout(string jsonString)
    {
        List<meterReadout> list = JsonConvert.DeserializeObject<List<meterReadout>>(jsonString);
        return list;
    }
    #endregion
}

public class LoginUserInfo
{
    public LoginUserInfo() { }

    private string _userName;
    private string _password;

    public string userName
    {
        get { return _userName; }
        set { _userName = value; }
    }
    public string password
    {
        get { return _password; }
        set { _password = value; }
    }
}
public class GetUserId
{
    public GetUserId() { }

    private string _userID;

    public string userID
    {
        get { return _userID; }
        set { _userID = value; }
    }
}
public class ChangePwd
{
    public ChangePwd() { }

    private string _userID;
    private string _newPWD;
    private string _verifyCode;

    public string userID
    {
        get { return _userID; }
        set { _userID = value; }
    }
    public string newPWD
    {
        get { return _newPWD; }
        set { _newPWD = value; }
    }
    public string verifyCode
    {
        get { return _verifyCode; }
        set { _verifyCode = value; }
    }
}
public class UpdateNickName
{
    public UpdateNickName() { }

    private string _nickName;
    private string _userType;

    public string nickName
    {
        get { return _nickName; }
        set { _nickName = value; }
    }
    public string userType
    {
        get { return _userType; }
        set { _userType = value; }
    }
}
public class ChangeInfo
{
    public ChangeInfo() { }

    private string _userName;
    private string _jobTitle;
    private string _pic;

    public string userName
    {
        get { return _userName; }
        set { _userName = value; }
    }
    public string jobTitle
    {
        get { return _jobTitle; }
        set { _jobTitle = value; }
    }
    public string pic
    {
        get { return _pic; }
        set { _pic = value; }
    }
}
public class ResetPwd
{
    public ResetPwd() { }

    private string _oldPwd;
    private string _newPwd;

    public string oldPwd
    {
        get { return _oldPwd; }
        set { _oldPwd = value; }
    }
    public string newPwd
    {
        get { return _newPwd; }
        set { _newPwd = value; }
    }
}

public class GetOrderList
{
    public GetOrderList() { }

    private string _orderType;
    private string _dispType;
    private string _orderNo;
    private string _startDate;
    private string _endDate;
    private int _pageSize;
    private int _pageIndex;

    public string orderType
    {
        get { return _orderType; }
        set { _orderType = value; }
    }
    public string dispType
    {
        get { return _dispType; }
        set { _dispType = value; }
    }
    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string startDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }
    public string endDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }
    public int pageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
    public int pageIndex
    {
        get { return _pageIndex; }
        set { _pageIndex = value; }
    }
}
public class getOrderInfo
{
    public getOrderInfo() { }

    private string _orderNo;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
}
public class treateOrderInfo
{
    public treateOrderInfo() { }

    private string _orderNo;
    private string _GPS_X;
    private string _GPS_Y;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string GPS_X
    {
        get { return _GPS_X; }
        set { _GPS_X = value; }
    }
    public string GPS_Y
    {
        get { return _GPS_Y; }
        set { _GPS_Y = value; }
    }
}
public class appoIntOrderInfo
{
    public appoIntOrderInfo() { }

    private string _orderNo;
    private string _appoIntTime;
    private string _GPS_X;
    private string _GPS_Y;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string appoIntTime
    {
        get { return _appoIntTime; }
        set { _appoIntTime = value; }
    }
    public string GPS_X
    {
        get { return _GPS_X; }
        set { _GPS_X = value; }
    }
    public string GPS_Y
    {
        get { return _GPS_Y; }
        set { _GPS_Y = value; }
    }
}
public class hangUpOrderInfo
{
    public hangUpOrderInfo() { }

    private string _orderNo;
    private string _hangUpReason;
    private string _GPS_X;
    private string _GPS_Y;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string hangUpReason
    {
        get { return _hangUpReason; }
        set { _hangUpReason = value; }
    }
    public string GPS_X
    {
        get { return _GPS_X; }
        set { _GPS_X = value; }
    }
    public string GPS_Y
    {
        get { return _GPS_Y; }
        set { _GPS_Y = value; }
    }
}
public class backOrderInfo
{
    public backOrderInfo() { }

    private string _orderNo;
    private string _backReason;
    private string _GPS_X;
    private string _GPS_Y;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string backReason
    {
        get { return _backReason; }
        set { _backReason = value; }
    }
    public string GPS_X
    {
        get { return _GPS_X; }
        set { _GPS_X = value; }
    }
    public string GPS_Y
    {
        get { return _GPS_Y; }
        set { _GPS_Y = value; }
    }
}
public class applyOrderInfo
{
    public applyOrderInfo() { }

    private string _orderNo;
    private string _applyReason;
    private string _GPS_X;
    private string _GPS_Y;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string applyReason
    {
        get { return _applyReason; }
        set { _applyReason = value; }
    }
    public string GPS_X
    {
        get { return _GPS_X; }
        set { _GPS_X = value; }
    }
    public string GPS_Y
    {
        get { return _GPS_Y; }
        set { _GPS_Y = value; }
    }
}
public class uploadImg
{
    public uploadImg() { }

    private string _orderNo;
    private string _img;
    private string _nodeNo;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string img
    {
        get { return _img; }
        set { _img = value; }
    }
    public string nodeNo
    {
        get { return _nodeNo; }
        set { _nodeNo = value; }
    }
}
public class entryOrderCost
{
    public entryOrderCost() { }

    private string _orderNo;
    private string _costType;
    private string _context;
    private string _costAmount;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string costType
    {
        get { return _costType; }
        set { _costType = value; }
    }
    public string context
    {
        get { return _context; }
        set { _context = value; }
    }
    public string costAmount
    {
        get { return _costAmount; }
        set { _costAmount = value; }
    }
}
public class deleteOrderCost
{
    public deleteOrderCost() { }

    private string _id;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }
}
public class updateOrderCost
{
    public updateOrderCost() { }

    private string _id;
    private string _costType;
    private string _context;
    private string _costAmount;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string costType
    {
        get { return _costType; }
        set { _costType = value; }
    }
    public string context
    {
        get { return _context; }
        set { _context = value; }
    }
    public string costAmount
    {
        get { return _costAmount; }
        set { _costAmount = value; }
    }
}
public class getMessageList
{
    public getMessageList() { }

    private string _orderNo;
    private string _startDate;
    private string _endDate;
    private int _pageSize;
    private int _pageIndex;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string startDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }
    public string endDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }
    public int pageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
    public int pageIndex
    {
        get { return _pageIndex; }
        set { _pageIndex = value; }
    }
}
public class viewMessageList
{
    public viewMessageList() { }

    private string _id;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }
}
public class getUserList
{
    public getUserList() { }

    private string _orderNo;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
}
public class addOrderUsers
{
    public addOrderUsers() { }

    private string _orderNo;
    private string _userNo;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }

    public string userNo
    {
        get { return _userNo; }
        set { _userNo = value; }
    }
}

public class entryOrderFee
{
    public entryOrderFee() { }

    private string _orderNo;
    private string _feeType;
    private string _context;
    private string _feeAmount;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string feeType
    {
        get { return _feeType; }
        set { _feeType = value; }
    }
    public string context
    {
        get { return _context; }
        set { _context = value; }
    }
    public string feeAmount
    {
        get { return _feeAmount; }
        set { _feeAmount = value; }
    }
}
public class deleteOrderFee
{
    public deleteOrderFee() { }

    private string _id;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }
}

public class setFee
{
    public setFee() { }

    private string _orderNo;
    private string _materialFee;
    private string _serviceFee;
    private string _context;

    public string orderNo
    {
        get { return _orderNo; }
        set { _orderNo = value; }
    }
    public string materialFee
    {
        get { return _materialFee; }
        set { _materialFee = value; }
    }
    public string serviceFee
    {
        get { return _serviceFee; }
        set { _serviceFee = value; }
    }
    public string context
    {
        get { return _context; }
        set { _context = value; }
    }
}
public class updateOrderFee
{
    public updateOrderFee() { }

    private string _id;
    private string _feeType;
    private string _context;
    private string _feeAmount;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string feeType
    {
        get { return _feeType; }
        set { _feeType = value; }
    }
    public string context
    {
        get { return _context; }
        set { _context = value; }
    }
    public string feeAmount
    {
        get { return _feeAmount; }
        set { _feeAmount = value; }
    }
}

public class getMeterInfo
{
    public getMeterInfo() { }

    private string _meterNo;

    public string meterNo
    {
        get { return _meterNo; }
        set { _meterNo = value; }
    }
}
public class meterReadout
{
    public meterReadout() { }

    private string _meterNo;
    private string _readoutType;
    private decimal _lastReadout;
    private decimal _readout;
    private decimal _joinReadings;
    private decimal _readings;
    public string imgName { get; set; }

    public string meterNo
    {
        get { return _meterNo; }
        set { _meterNo = value; }
    }
    public string readoutType
    {
        get { return _readoutType; }
        set { _readoutType = value; }
    }
    public decimal lastReadout
    {
        get { return _lastReadout; }
        set { _lastReadout = value; }
    }
    public decimal readout
    {
        get { return _readout; }
        set { _readout = value; }
    }
    public decimal joinReadings
    {
        get { return _joinReadings; }
        set { _joinReadings = value; }
    }
    public decimal readings
    {
        get { return _readings; }
        set { _readings = value; }
    }
}