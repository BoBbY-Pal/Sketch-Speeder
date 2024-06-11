using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Sketch_Speeder.Managers;
using Sketch_Speeder.Utils;

namespace Sketch_Speeder.Advertising
{
    public class AdManager : Singleton<AdManager>
    {
#if UNITY_ANDROID && !UNITY_EDITOR
   
    private readonly string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";   // test id
    private readonly string rewardAdUnitId = "ca-app-pub-3940256099942544/5224354917";  // test id
    priate readonly string appOpenAdUnitId =  "ca-app-pub-3940256099942544/3419835294" // test id
    private readonly string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111"    ;
#else
        private readonly string interstitialAdUnitId = "unused";
        private readonly string rewardAdUnitId = "unused";
        private readonly string appOpenAdUnitId = "unused";
        private readonly string bannerAdUnitId = "unused";        
#endif

        [SerializeField] private Transform _rewardedAdsBtn;
        
        private AppOpenAd appOpenAd;
        private BannerView _bannerView;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
        private DateTime _appOpenAdExpireTime; 

        private void Awake()
        {
            // Use the AppStateEventNotifier to listen to application open/close events.
            // This is used to launch the loaded ad when we open the app.
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        private void OnDestroy()
        {
            // Always unlisten to events when complete.
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        }
        
        private void Start()
        {
            MobileAds.Initialize(initStatus =>
            {
                Debug.Log("Admob initialized");
                LoadAppOpenAd();
                LoadBannerAd();
                ShowBannerAd();
                LoadInterstitialAd();
                LoadRewardedAd();
            });
        }
        
        public bool IsAppOpenAdAvailable
        {
            get
            {
                return appOpenAd != null
                       && DateTime.Now < _appOpenAdExpireTime;
            }
        }
        
#region REWARDED ADS
        public void ShowRewardedAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    // TODO: Reward the user.
                    CurrencyManager.Instance.AddCoins(5,_rewardedAdsBtn.localPosition);
                    UiManager.Instance.ActivatePopUp("REWARD CREDITED!", "5 Coins", 0);
                    Debug.LogWarning("REWARD CREDITED!");
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }

        /// <summary>
        /// Loads the rewarded ad.
        /// </summary>
        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(rewardAdUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                    RegisterEventHandlers(_rewardedAd);
                });
        }
        
        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedAd();
            };
        }
#endregion

#region INTERSTITIAL ADS
        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public void ShowInterstitialAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }
        
        /// <summary>
        /// Loads the interstitial ad.
        /// </summary>
        public void LoadInterstitialAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(interstitialAdUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                    RegisterEventHandlers(_interstitialAd);
                });
        }
        
        private void RegisterEventHandlers(InterstitialAd interstitialAd)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad full screen content closed.");
                
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
        }
        #endregion
        
#region BANNER ADS        
        /// <summary>
        /// Shows the ad.
        /// </summary>
        public void ShowBannerAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Showing banner view.");
                _bannerView.Show();
            }
        }

        /// <summary>
        /// Creates a 320x50 banner view at top of the screen.
        /// </summary>
        public void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(bannerAdUnitId, AdSize.MediumRectangle, AdPosition.BottomLeft);
        }
        
        /// <summary>
        /// Creates the banner view and loads a banner ad.
        /// </summary>
        public void LoadBannerAd()
        {
            // create an instance of a banner view first.
            if(_bannerView == null)
            {
                CreateBannerView();
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
            ListenToAdEvents();
        }
        
        /// <summary>
        /// listen to events the banner view may raise.
        /// </summary>
        private void ListenToAdEvents()
        {
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += () =>
            {
                Debug.Log("Banner view loaded an ad with response : "
                          + _bannerView.GetResponseInfo());
            };
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                Debug.LogError("Banner view failed to load an ad with error : "
                               + error);
            };
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Banner view paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += () =>
            {
                Debug.Log("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Banner view full screen content closed.");
            };
        }
#endregion

#region APPOPEN ADS
        /// <summary>
        /// Loads the app open ad.
        /// </summary>
        public void LoadAppOpenAd()
        {
            // Clean up the old ad before loading a new one.
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }
            
            Debug.Log("Loading the app open ad.");
            var adRequest = new AdRequest();
            
            // send the request to load the ad.
            AppOpenAd.Load(appOpenAdUnitId, adRequest, (AppOpenAd ad, LoadAdError adError) =>
            {
                if (adError != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + adError);
                    return;
                }
                
                Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
                
                // App open ads can be preloaded for up to 4 hours.
                _appOpenAdExpireTime = DateTime.Now + TimeSpan.FromHours(4);
                
                appOpenAd = ad;
                RegisterEventHandlers(ad);
                
            });
        }
        

        /// <summary>
        /// Shows the app open ad.
        /// </summary>
        public void ShowAppOpenAd()
        {
            if (appOpenAd != null && appOpenAd.CanShowAd())
            {
                Debug.Log("Showing app open ad.");
                appOpenAd.Show();
            }
            else
            {
                Debug.LogError("App open ad is not ready yet.");
            }
        }
        
        private void RegisterEventHandlers(AppOpenAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("App open ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("App open ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("App open ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("App open ad full screen content opened.");
                
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("App open ad full screen content closed.");
                LoadAppOpenAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("App open ad failed to open full screen content " +
                               "with error : " + error);
                LoadAppOpenAd();
            };
        }
        
        private void OnAppStateChanged(AppState state)
        {
            Debug.Log("App State changed to : "+ state);

            // if the app is Foregrounded and the ad is available, show it.
            if (state == AppState.Foreground)
            {
                if (IsAppOpenAdAvailable)
                {
                    ShowAppOpenAd();
                }
            }
        }
#endregion        
    }
}