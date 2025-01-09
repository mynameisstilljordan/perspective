using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Lofelt.NiceVibrations;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Sprite[] _vibrationButtonSprites;
    [SerializeField] Button _vibrationButton;
    [SerializeField] Sprite[] _soundButtonSprites;
    [SerializeField] Button _soundButton;
    [SerializeField] Button _playButton;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("vibration", 1) == 1) _vibrationButton.image.sprite = _vibrationButtonSprites[0];
        else _vibrationButton.image.sprite = _vibrationButtonSprites[1];

        if (PlayerPrefs.GetInt("sound", 1) == 1) _soundButton.image.sprite = _soundButtonSprites[0];
        else _soundButton.image.sprite = _soundButtonSprites[1];

        _vibrationButton.onClick.AddListener(OnVibrationButtonPressed);
        _soundButton.onClick.AddListener(OnSoundButtonPressed);
        _playButton.onClick.AddListener(OnPlayButtonPressed);

        if (GlobalGameHandlerScript._adCounter == 2) { //if 
            GlobalGameHandlerScript.ShowAd(); //show an ad
        }
    }

    //when the settings button is pressed
    void OnVibrationButtonPressed() {
        if (PlayerPrefs.GetInt("vibration", 1) == 1) PlayerPrefs.SetInt("vibration", 0); //if vibration was enabled, disable it
        else PlayerPrefs.SetInt("vibration", 1); //if vibration was disabled, enable it

        if (PlayerPrefs.GetInt("vibration", 1) == 1) {
            _vibrationButton.image.sprite = _vibrationButtonSprites[0];
            HapticController.hapticsEnabled = true;
        }
        else {
            _vibrationButton.image.sprite = _vibrationButtonSprites[1];
            HapticController.hapticsEnabled = false;
        }

        PlayButtonSound();
    }

    void OnSoundButtonPressed() {
        if (PlayerPrefs.GetInt("sound", 1) == 1) PlayerPrefs.SetInt("sound", 0);
        else PlayerPrefs.SetInt("sound", 1);

        if (PlayerPrefs.GetInt("sound", 1) == 1) _soundButton.image.sprite = _soundButtonSprites[0];
        else _soundButton.image.sprite = _soundButtonSprites[1];

        PlayButtonSound();
    }

    void OnPlayButtonPressed() {
        PlayButtonSound();
        SceneManager.LoadScene("ingame");
    }

    void PlayButtonSound() {
        SoundManager.PlaySound("button");
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
}
