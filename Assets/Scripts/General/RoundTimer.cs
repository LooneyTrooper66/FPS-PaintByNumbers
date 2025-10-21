using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject pauseMenu;
    public float roundLengthInSeconds;
    float timeLeft;
    public bool gamePlaying;


    private void Start()
    {
        timeLeft = roundLengthInSeconds;
        pauseMenu.SetActive(false);
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
