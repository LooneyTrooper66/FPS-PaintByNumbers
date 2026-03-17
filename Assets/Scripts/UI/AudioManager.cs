using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterS;
    public Slider musicS;
    public Slider sfxS;


    private void Start()
    {
        masterS.value = PlayerPrefs.GetFloat("MasterVol", 1);
        mixer.SetFloat("MasterVol", Mathf.Log10(masterS.value) * 20);
        musicS.value = PlayerPrefs.GetFloat("MusicVol", 1);
        mixer.SetFloat("MusicVol", Mathf.Log10(musicS.value) * 20);
        sfxS.value = PlayerPrefs.GetFloat("SFXVol", 1);
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxS.value) * 20);
    }

    public void OnMasterChange()
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(masterS.value) * 20);
        PlayerPrefs.SetFloat("MasterVol", masterS.value);
    }
    public void OnMusicChange()
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(musicS.value) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicS.value);
    }
    public void OnSFXChange()
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxS.value) * 20);
        PlayerPrefs.SetFloat("SFXVol", sfxS.value);
    }
}
