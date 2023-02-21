using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public struct WifiInfo
{
    public string SSID;
    public int level;
}

public class WifiScanner : MonoBehaviour
{
    public UIDebugManager dbg;

    private AndroidJavaObject wifiManager;

    private List<WifiInfo> wifiInfoList = new();

    private const int REQUEST_CODE = 1;
    private string[] permissions = { "android.permission.ACCESS_WIFI_STATE" };

    void Start()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("requestPermissions", permissions, REQUEST_CODE);
        wifiManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "wifi");
    }

    void Update()
    {
        if (dbg.IsOn)
        {
            DispScan();
        }
    }

    public void UpdateWifiInfoList()
    {
        var scanResults = wifiManager.Call<AndroidJavaObject>("getScanResults");
        var list = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(scanResults.GetRawObject());

        wifiInfoList = new();
        foreach (var wifi in list)
        {
            var wifiInfo = new WifiInfo();
            wifiInfo.SSID = wifi.Call<string>("getWifiSsid");
            wifiInfo.level = wifi.Get<int>("level");
            wifiInfoList.Add(wifiInfo);
        }
    }

    public string GetWifiInfoString()
    {
        string wifiInfoString = "";
        foreach (var wifiInfo in wifiInfoList)
        {
            wifiInfoString += wifiInfo.SSID + " " + wifiInfo.level + "dB\n";
        }
        return wifiInfoString;
    }

    public void DispScan()
    {
        UpdateWifiInfoList();
        dbg.SetMainDebug(GetWifiInfoString());
    }
}
