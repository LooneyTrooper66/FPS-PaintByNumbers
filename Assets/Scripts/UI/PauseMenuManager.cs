using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject tutorialMenu;
    public GameObject optionsMenu;


    void Start()
    {
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
    public void TutorialButton()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public void OptionsButton()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
