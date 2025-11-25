using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energy;
    bool lerpUp = true;
    float lerpSpeed = 1;

    // NEED TO FIX THIS AS LERP IS TOO FAST

    private void Start() => energy.value = 100;

    private void Update()
    {
        if (!RoundTimer.i.gamePlaying) return;

        if (lerpUp && energy.value != 100)
        {
            energy.value = Mathf.Lerp(energy.value, 100, lerpSpeed * Time.deltaTime);
        }
        else if (!lerpUp && energy.value != 0)
        {
            energy.value = Mathf.Lerp(energy.value, 0, lerpSpeed * Time.deltaTime);
        }


        if (PercentCorrectCounter.pcc.percentage > 75) lerpSpeed = 0.5f;
        else if (PercentCorrectCounter.pcc.percentage <= 75 && PercentCorrectCounter.pcc.percentage > 50) lerpSpeed = 1;
        else if (PercentCorrectCounter.pcc.percentage <= 50) lerpSpeed = 2;
        else lerpSpeed = 0.1f;
    }

    public void ShotEnemy(bool correct)
    {
        lerpUp = correct;
    }
}
