using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class GlobalGameHandlerScript : MonoBehaviour {
    /// <summary>Static reference to the instance of GlobalGameHandler</summary>
    public static GlobalGameHandlerScript Instance;

    /// <summary>Awake is called when the script instance is being loaded.</summary>
    void Awake() {
        // If the instance reference has not been set, yet, 
        if (Instance == null) {
            // Set this instance as the instance reference.
            Instance = this;
        }
        else if (Instance != this) {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }

        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }

    public static int _adCounter;
    string _npaValue;

    private void Start() {
        _adCounter = 0;
        _npaValue = PlayerPrefs.GetString("npa", "0"); //default npa of 0

        Advertisements.Instance.Initialize(); //initialize ads

        if (PlayerPrefs.GetInt("vibration", 1) == 0) HapticController.hapticsEnabled = false; //disable haptics
        else HapticController.hapticsEnabled = true; //enable haptics
    }

    public static void IncrementAdCounter() {
        _adCounter++; //increment the ad counter
    }

    public static void ShowAd() {
        _adCounter = 0; //reset the adCounter
        if (Advertisements.Instance.IsInterstitialAvailable()) Advertisements.Instance.ShowInterstitial(); //show the ad
    }
}
