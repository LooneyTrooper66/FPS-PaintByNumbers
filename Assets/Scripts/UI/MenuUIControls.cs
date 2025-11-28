using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MenuUIControls : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject tutorialMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public Image fadeOut;
    public Slider rDSlider;
    public TMP_Text rdSliderVal;
    float duration = 1;


    private void Start()
    {
        fadeOut.enabled = false;
        startMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        rDSlider.value = PlayerPrefs.GetFloat("roundTimeInMins");
    }

    public void StartButton()
    {
        fadeOut.enabled = true;
        StartCoroutine(FadeScreen());
    }
    public void TutorialButton()
    {
        startMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
    public void OptionsButton()
    {
        startMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
    public void CreditsButton()
    {
        startMenu.SetActive(false);
        tutorialMenu.SetActive(false);
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
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

    }

    public void RoundDurationSlider()
    {
        PlayerPrefs.SetFloat("roundTimeInMins", rDSlider.value);
        rdSliderVal.text = rDSlider.value.ToString();
    }

    IEnumerator FadeScreen()
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            fadeOut.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
        SceneManager.LoadScene("Map1");
    }
}
