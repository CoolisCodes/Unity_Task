using System;
using UnityEngine;

[Serializable]
public class PlayerDecision
{
    public string firstKey;
    public string secondKey;
    public bool isCorrect = false;

    public LineRenderer lineRenderer;
    public Color correctColor;
    public Color incorrectColor;

    public void EvaluateDecision()
    {
        if (firstKey == secondKey)
        {
            lineRenderer.SetColors(correctColor, correctColor);
            isCorrect = true;

        }
        else
        {
            lineRenderer.SetColors(incorrectColor, incorrectColor);
            isCorrect = false;
        }
    }

    public PlayerDecision(LineRenderer lineRenderer, Color correctColor, Color incorrectColor)
    {
        this.lineRenderer = lineRenderer;
        this.correctColor = correctColor;
        this.incorrectColor = incorrectColor;
    }
}
