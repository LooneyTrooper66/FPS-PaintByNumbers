using UnityEngine;
using TMPro;

public class PercentCorrectCounter : MonoBehaviour
{
    public static PercentCorrectCounter pcc;
    public EnergyBar bar;
    [SerializeField] TMP_Text percent;
    public int numPaint = 0;
    public int numCorrect = 0;
    public decimal percentage = 100;


    void Awake()
    {
        if (pcc != null) Destroy(this);
        else pcc = this;
    }

    private void FixedUpdate()
    {
        if (numPaint > 0)
        {
            percentage = decimal.Divide(numCorrect, numPaint) * 100;
            percent.text = $"{decimal.Round(percentage, 2)}% correct";
        }
    }

    public void EnemyShot(bool correct)
    {
        bar.ShotEnemy(correct);
    }
}
