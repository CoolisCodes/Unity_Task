using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalUiElement : MonoBehaviour
{
    public string key;

    public UiType uiType;

    public void UserClick()
    {
        GameManager.Instance.onUiElementClicked?.Invoke(key, transform.position, uiType);
    }
}

public enum UiType
{
    Image,
    Word
}
