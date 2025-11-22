using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuUIControls : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public Image fadeOut;
    float duration = 1;


    private void Start()
    {
        fadeOut.enabled = false;
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void StartButton()
    {
        fadeOut.enabled = true;
        StartCoroutine(FadeScreen());
    }
    public void OptionsButton()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
    public void CreditsButton()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void BackButton()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

    }

    IEnumerator FadeScreen()
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            fadeOut.color = new Color(0, 0, 0, newAlpha);
            Debug.Log(fadeOut.color);
            yield return null;
        }
        SceneManager.LoadScene("Map1");
    }
}
