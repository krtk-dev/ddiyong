using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerCtrl : MonoBehaviour
{
    private BannerView bannerView;
    private AdRequest adRequest;

    public void Start()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-1799117611799052~9523343449";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
        MobileAds.Initialize(appId);
        if (PlayerPrefs.GetString("ADS") != "true")
        {
            RequestBanner();
        }
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1799117611799052/3121586622";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Center);


        //adRequest = new AdRequest.Builder().Build();
        AdRequest.Builder builder = new AdRequest.Builder();
        adRequest = builder.AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("3AD6F0C8ED87640D").Build();

        // Load the banner with the request.
        bannerView.LoadAd(adRequest);
        bannerView.Hide();
    }
    public void BannerLoad()
    {
        if (PlayerPrefs.GetString("ADS") != "ture" && bannerView != null) bannerView.Show();
    }
    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            //bannerView.Destroy();
        }
        else if(PlayerPrefs.GetString("ADS") != "true")
        {
            RequestBanner();
        }
    }
    public void DeleteBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            bannerView.Destroy();
        }
    }
}
