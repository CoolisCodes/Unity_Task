using UnityEngine;

/// <summary>
/// this class is responsible for keeping all the UI elements of the application and the position of the mouse
/// at any given time.
/// </summary>
public class GameUI : MonoBehaviour
{
    /// <summary>
    /// the backround panel of the Main Canvas
    /// </summary>
    private GameObject backroundPanel;

    /// <summary>
    /// the panel in wich the images will get generated into
    /// </summary>
    public GameObject imagesPanel;

    /// <summary>
    /// the panel in wich the words will get generated into
    /// </summary>
    public GameObject wordsPanel;

    /// <summary>
    /// the Rect Transform used to evalueate the position of the mouse
    /// </summary>
    public RectTransform rectTransform;

    /// <summary>
    /// the position of the mouse
    /// </summary>
    public Vector3 mouseLocalPos;


    private void Start()
    {
        backroundPanel = transform.Find("Backround Panel").gameObject;
        imagesPanel = backroundPanel.transform.Find("Images Panel").gameObject;
        wordsPanel = backroundPanel.transform.Find("Words Panel").gameObject;
    }

    void Update()
    {
        Vector2 localpoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, GetComponent<Canvas>().worldCamera, out localpoint);

        Vector2 normalizedPoint = Rect.PointToNormalized(rectTransform.rect, localpoint);

        mouseLocalPos = new Vector3(localpoint.x, localpoint.y, transform.position.z);
    }


}
