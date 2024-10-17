using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;

public class SeaTools
{
    /// <summary>
    /// 获得系统语言
    /// </summary>
    /// <returns></returns>
    public static string SeaGL()
    {
        return Enum.GetName(typeof(SystemLanguage), Application.systemLanguage);
    }
    
    #region 设备信息，需要UnityEngine
    /// <summary>
    /// 获取设备名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceName()
    {
        return UnityEngine.SystemInfo.deviceName;
    }

    /// <summary>
    /// 获取设备模型
    /// </summary>
    /// <returns></returns>
    public static string SeaGDM()
    {
        return UnityEngine.SystemInfo.deviceModel;
    }

    /// <summary>
    /// 获取设备唯一标识码
    /// </summary>
    /// <returns></returns>
    public static string SeaGDUI()
    {
        return UnityEngine.SystemInfo.deviceUniqueIdentifier;
    }
    #endregion

    /// <summary>
    /// 获取局域网Ip
    /// </summary>
    /// <param name="addressType">IPv4或IPv6</param>
    /// <returns></returns>
    public static string SeaGLI(AddressType addressType)
    {
        if (addressType == AddressType.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = string.Empty;

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;
            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (addressType == AddressType.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (addressType == AddressType.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return output;
    }

    /// <summary>
    /// 获取外网Ip
    /// </summary>
    /// <returns></returns>
    public static string GetExtranetIp()
    {
        string IP = string.Empty;
        try
        {
            //从网址中获取本机ip数据  
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.Default;
            IP = client.DownloadString("http://checkip.amazonaws.com/");
            client.Dispose();
            IP = Regex.Replace(IP, @"[\r\n]", "");
        }
        catch (Exception) { }

        return IP;
    }

    /// <summary>
    /// 获取MAC地址
    /// </summary>
    /// <returns></returns>
    public static List<string> GetMACList(OperationalStatus operationalStatus=OperationalStatus.Up,bool isAppendName=true)
    {
        var list = new List<string>();
        //这里使用 NetworkInterface 获取网络设备信息，能够直接获取网络设备类型，描述，名称等信息
        NetworkInterface[] allNetWork = NetworkInterface.GetAllNetworkInterfaces();
        if (allNetWork.Length > 0)
        {
            foreach (var item in allNetWork)
            {
                if (item.OperationalStatus == operationalStatus)
                {
                    //对MAC地址加上网卡名称，方便进行对应和选择
                    string strInfo = isAppendName?item.GetPhysicalAddress().ToString() + $"({item.Name})": item.GetPhysicalAddress().ToString();
                    list.Add(strInfo);
                }
            }
        }
        else
        {
            Console.WriteLine("找不到可用的网卡！");
        }
        return list;
    }

    /// <summary>
    /// 获得系统启动时间
    /// </summary>
    /// <returns></returns>
    public static DateTime GetSystemStartupTime()
    {
        return DateTime.Now.AddMilliseconds(-Environment.TickCount);
    }

    /// <summary>
    /// 获得系统运行秒数
    /// </summary>
    /// <returns></returns>
    public static int SeaGU()
    {
        return Environment.TickCount / 1000;
    }
    
    /// <summary>
    /// 是否安装 fb
    /// </summary>
    /// <returns></returns>
    public static int IsFacebookInstalled()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        try
        {
            packageManager.Call<AndroidJavaObject>("getPackageInfo", "com.facebook.katana", 0);
            return 1;
        }
        catch (AndroidJavaException)
        {
            return 0;
        }
#else
        return 0;
#endif
    }
    
    /// <summary>
    /// 获得网络类型
    /// </summary>
    /// <returns></returns>
    public static string SeaGN()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject connectivityManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "connectivity");

        AndroidJavaObject activeNetwork = connectivityManager.Call<AndroidJavaObject>("getActiveNetworkInfo");

        if (activeNetwork != null)
        {
            int type = activeNetwork.Call<int>("getType");
            if (type == 1) return "wifi";
            else if (type == 0) return "mobile";
            else return "unknow";
        }
        else
        {
            return "NoConnection";
        }
#else
        return "Editor";
#endif
    }
    public static string SeaGP()
    {
        return Application.identifier;
    }

}

public enum AddressType
{
    IPv4,
    IPv6,
}
