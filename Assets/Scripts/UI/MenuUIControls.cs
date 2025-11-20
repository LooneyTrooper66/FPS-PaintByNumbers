using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIControls : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;


    private void Start()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Map1");
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
}
