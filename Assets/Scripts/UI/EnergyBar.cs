using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energy;
    public Animator energyAnim;
    bool barPusling = false;
    bool lerpUp = true;
    float lerpSpeed = 2;

    [Header("Game Over")]
    public string gameOverText;


    private void Start() => energy.value = 100;

    private void Update()
    {
        if (!RoundTimer.i.gamePlaying) return;

        // lerp energy bar
        LerpBar();
        CheckLerpDirection();  // need to get some playtests to see how fair this is and wether it needs changing

        // animate lerp bar
        BarPusle();

        // dead check
        if (energy.value <= 0) EnergyDrained();
    }

    void LerpBar()
    {
        if (lerpUp && energy.value != 100)
        {
            energy.value = Mathf.MoveTowards(energy.value, 100, lerpSpeed * Time.deltaTime);
        }
        else if (!lerpUp && energy.value != 0)
        {
            energy.value = Mathf.MoveTowards(energy.value, 0, lerpSpeed * Time.deltaTime);
        }
    }

    void CheckLerpDirection()
    {
        if (PercentCorrectCounter.pcc.percentage <= 75 && lerpUp) lerpUp = false;
        else if (PercentCorrectCounter.pcc.percentage > 75 && !lerpUp) lerpUp = true;
    }

    void BarPusle()
    {
        if (energy.value <= 25 && !barPusling) { energyAnim.SetBool("below25", true); barPusling = true; }
        else if (energy.value > 25 && barPusling) { energyAnim.SetBool("below25", false); barPusling = false; }
    }

    void EnergyDrained()
    {
        RoundTimer.i.gamePlaying = false;
        RoundTimer.i.gameEnded = true;
        RoundTimer.i.TimerEndedScreen(gameOverText);
    }
}
