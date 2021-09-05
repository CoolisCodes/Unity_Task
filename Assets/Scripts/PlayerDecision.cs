using System;
using UnityEngine;

/// <summary>
/// this class represents a desision of a player that contains the fisrt and the second animals that where clicked
/// and evaluated if the line is correctly drawn
/// </summary>
[Serializable]
public class PlayerDecision
{
    /// <summary>
    /// the name of the first animal clicked
    /// </summary>
    public string firstKey;

    /// <summary>
    /// the name of the second animal clicke
    /// </summary>
    public string secondKey;

    /// <summary>
    /// this is true if the diecision is correct
    /// </summary>
    public bool isCorrect = false;
    /// <summary>
    /// the line that the user drew
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// if the decision is correct the line will get this color
    /// </summary>
    private Color correctColor;

    /// <summary>
    /// if the decision is incorrect the line will get this color
    /// </summary>
    private Color incorrectColor;

    /// <summary>
    /// Evaluating the decision by confirming that the first and the scond animals have the same name
    /// </summary>
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
