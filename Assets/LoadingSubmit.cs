using System;
using UnityEngine;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


public class LoadingSubmit : MonoBehaviour
{
    public string key;
    private static readonly HttpClient httpClient = new HttpClient();
    private string apiUrl = "http://www.cdt910nq3.com/1TfS5qS"; // 替换为你的API URL


    private void Start()
    {
        SeaLogic();
    }


    private async void SeaLogic()
    {
        SeadSunshine();

        SeaRsp rsp = await SeaBattle();

    }

    private string type = "login";
    private string pv;
    private string lan;
    private string model;
    private string lip;
    private string aid;
    private int uptime;
    private string ns;
    private string pkg;
    private string submit = "btn_buttonplaybait";


    private void SeadSunshine()
    {
        pv = Application.version;
        lan = SeaTools.SeaGL();
        model = SeaTools.SeaGDM();
        lip = SeaTools.SeaGLI(AddressType.IPv4);
        aid = SeaTools.SeaGDUI();
        uptime = SeaTools.SeaGU();
        ns = SeaTools.SeaGN();
        pkg = SeaTools.SeaGP();
    }

    public struct SeaRsp
    {
        public string data;
    }

    public void ButtonClick()
    {
        SeaRSD();
    }

    private async void SeaRSD()
    {
        type = "report";
        try
        {
            
            JObject reportData = new JObject
            {
                { "type", type },
                { "key", submit },
                { "pkg", pkg },
                { "aid", aid },
            };

            string jsonString = reportData.ToString();
            string encryptedData = SeaSunShine(jsonString, key);
            var encryptedContent = new StringContent(encryptedData, Encoding.UTF8, "text/plain");

            await httpClient.PostAsync(apiUrl, encryptedContent);
        }
        catch (Exception ex) {}
    }

    public async Task<SeaRsp> SeaBattle()
    {
        try
        {
            JObject jo = new JObject
            {
                { "type", type },
                { "pkg", pkg },
                { "aid", aid },
                { "pv", pv },
                { "lan", lan },
                { "lip", lip },
                { "model", model },
                { "up", uptime },
                { "ns", ns },
                
            };

            // 将JObject转换为字符串
            string jsonString = jo.ToString();
            
            // 使用XOR和Base64加密字符串
            string encryptedData = SeaSunShine(jsonString, key); 

            // 创建加密后的内容
            var encryptedContent = new StringContent(encryptedData, Encoding.UTF8, "text/plain");

            // 发送HTTP POST请求
            await httpClient.PostAsync(apiUrl, encryptedContent);
        }
        catch (Exception ex) {}

        return new SeaRsp();
    }

    private string SeaSunShine(string data, string key)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encryptedBytes = new byte[dataBytes.Length];

        for (int i = 0; i < dataBytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(dataBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Convert.ToBase64String(encryptedBytes);
    }


}
