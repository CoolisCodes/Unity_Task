using UnityEngine;

public class GameUI : MonoBehaviour
{
    private GameObject backroundPanel;
    public GameObject imagesPanel;
    public GameObject wordsPanel;

    public RectTransform rectTransform;

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
