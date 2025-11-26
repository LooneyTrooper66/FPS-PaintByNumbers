using UnityEngine;
using TMPro;
using System.Collections;

public class RoundTimer : MonoBehaviour
{
    public static RoundTimer i;

    public TMP_Text timerText;
    public GameObject gameEnd;
    public GameObject beforeStart;
    public GameObject roundStart;
    public GameObject pauseMenu;
    public GameObject mainUI;
    public float roundLengthInSeconds;
    float timeLeft;
    public bool gamePlaying;
    public bool gameEnded;
    public Animator textAnim;
    bool animPlaying;

    public GameObject gameEndCanvas;
    public string gameEndText;


    void Awake()
    {
        if (i != null) Destroy(this);
        else i = this;
        animPlaying = false;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        gameEnd.SetActive(false);
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
            if (gameEnded) return;
            if (gamePlaying) { gamePlaying = false; Time.timeScale = 0; pauseMenu.SetActive(true); }
            else { gamePlaying = true; Time.timeScale = 1; pauseMenu.SetActive(false); }
        }

        if (gamePlaying)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer();
            if (timeLeft < 0)
            {
                gamePlaying = false; 
                gameEnded = true;
                TimerEndedScreen(gameEndText);
            }
            else if (timeLeft <= 11 && !animPlaying) { animPlaying = true; textAnim.SetTrigger("5SecsLeft"); }
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

    public void TimerEndedScreen(string titleText)
    {
        pauseMenu.SetActive(false);
        mainUI.SetActive(false);
        TMP_Text[] texts = gameEndCanvas.GetComponent<Canvas>().GetComponentsInChildren<TMP_Text>();
        texts[0].text = titleText;
        texts[1].text = ($"Lets see your stats!\n\n% of paint in correct areas:\n{PercentCorrectCounter.pcc.percentText}\n\nNumber of tiles filled:\n{FloorFillCheck.ffc.floorTilesCompleted}/48 completed\n\nTotal enemies killed:\n{PercentCorrectCounter.pcc.numPaint.ToString()}\n");
        gameEndCanvas.SetActive(true);
    }
}
