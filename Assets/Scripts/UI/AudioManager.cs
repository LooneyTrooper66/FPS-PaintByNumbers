using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class AudioManager : MonoBehaviour
{
    AudioManager m;

    public AudioMixer mixer;
    public Slider masterS;
    public TMP_Text masterT;
    public Slider musicS;
    public Slider sfxS;
    public TMP_Text sfxT;


    private void Awake()
    {
        if (m != null) Destroy(this);
        else m = this;
    }

    public void OnMasterChange()
    {
        mixer.SetFloat("MasterVol", masterS.value);
    }
    public void OnMusicChange()
    {
        mixer.SetFloat("MusicVol", musicS.value);
    }
    public void OnSFXChange()
    {
        mixer.SetFloat("SFXVol", sfxS.value);
    }
}
