using UnityEngine;

public class PlayUISound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip beep;


    public void OnClickPlaySFX()
    {
        source.PlayOneShot(beep);
    }
}
