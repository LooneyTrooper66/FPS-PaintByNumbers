using UnityEngine;
using TMPro;
using System.Collections;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject beforeStart;
    public GameObject roundStart;
    public GameObject pauseMenu;
    public GameObject mainUI;
    public float roundLengthInSeconds;
    float timeLeft;
    public bool gamePlaying;
    public bool gameEnded;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        beforeStart.SetActive(true);
        roundStart.SetActive(false);
        pauseMenu.SetActive(false);
        mainUI.SetActive(false);
        gamePlaying = false;
        gameEnded = false;
    }

    public void StartButton() => StartCoroutine(GameCountdown());
    IEnumerator GameCountdown()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        beforeStart.SetActive(false);
        roundStart.SetActive(true);
        yield return new WaitForSecondsRealtime(3.3f);
        timeLeft = roundLengthInSeconds;
        roundStart.SetActive(false);
        mainUI.SetActive(true);
        gamePlaying = true;
    }

    private void Update()
    {
        // pause 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePlaying) { gamePlaying = false; Time.timeScale = 0; pauseMenu.SetActive(true); }
            else { gamePlaying = true; Time.timeScale = 1; pauseMenu.SetActive(false); }
        }

        if (gamePlaying)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer();
            if (timeLeft < 0)
            {
                gamePlaying = false; // need to add actual end screen
                gameEnded = true;
            }
        }
    }

    void UpdateTimer()
    {
        float minutes = (timeLeft / 60);
        int minutesI = (int)minutes;
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        seconds = Mathf.Abs(seconds); // make not negative
        timerText.text = string.Format("{0} : {1:00}", minutesI, seconds);
    }
}
