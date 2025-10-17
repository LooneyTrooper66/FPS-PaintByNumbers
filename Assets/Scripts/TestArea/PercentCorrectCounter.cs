using UnityEngine;
using TMPro;

public class PercentCorrectCounter : MonoBehaviour
{
    [SerializeField] TMP_Text percent;
    public int numPaint = 0;
    public int numCorrect = 0;


    private void FixedUpdate()
    {
        if (numPaint > 0)
        {
            decimal percentage = decimal.Divide(numCorrect, numPaint) * 100;
            percent.text = $"{decimal.Round(percentage, 2)}% correct";
        }
    }
}
