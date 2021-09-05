using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the "clickable" UI elements of the application and stores the type and the name of each one
/// </summary>
public class AnimalUiElement : MonoBehaviour
{
    /// <summary>
    /// the name of the animal in the UI element
    /// </summary>
    public string key;

    /// <summary>
    /// the type of that UI element (Image or Word)
    /// </summary>
    public UiType uiType;

    /// <summary>
    /// this turns true if the UI element is already selected
    /// </summary>
    public bool taken = false;

    /// <summary>
    /// Using an Event trigger, when the user clicks the UI element it invokes the onUiElementClicked delegate
    /// passing the name, the position and the AnimalUI object
    /// </summary>
    public void UserClick()
    {
        GameManager.Instance.onUiElementClicked?.Invoke(key, transform.position, this);
    }
}

public enum UiType
{
    Image,
    Word
}
