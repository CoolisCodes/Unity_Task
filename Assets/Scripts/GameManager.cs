using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public List<Animal> animals = new List<Animal>();

    public List<PlayerDecision> playerDecisions = new List<PlayerDecision>();

    private PlayerDecision currentPlayerDecision;

    public Dictionary<string, Texture2D> picturesAndAnimals = new Dictionary<string, Texture2D>();

    public GameObject animalImagePrefab;
    public GameObject animalNamePrefab;
    public GameObject linePrebab;

    public LineRenderer createdLine;
    public UiType lastClickedElementType;

    public GameUI gameUI;

    public Color correctColor;
    public Color incorrectColor;

    public Action<string, Vector3, UiType> onUiElementClicked;

    public bool lineInstantiated = false;

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void OnEnable()
    {
        onUiElementClicked += HandleLine;
    }

    private void OnDisable()
    {
        onUiElementClicked -= HandleLine;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            StartCoroutine(SetImages(animals));
        }
    }

    public void HandleLine(string key, Vector3 poition, UiType uiType)
    {
        if (!lineInstantiated)
        {
            Debug.Log($"the user first clicked on {key}");



            lastClickedElementType = uiType;

            GameObject lineRenderer = Instantiate(linePrebab, gameUI.transform);

            createdLine = lineRenderer.GetComponent<LineRenderer>();

            createdLine.SetPosition(0, new Vector3(poition.x, poition.y, -20));

            currentPlayerDecision = new PlayerDecision(createdLine, correctColor, incorrectColor);

            currentPlayerDecision.firstKey = key;

            lineInstantiated = true;
        }
        else if (lineInstantiated && lastClickedElementType != uiType)
        {
            Debug.Log($"the user last clicked on {key}");

            createdLine.SetPosition(1, new Vector3(poition.x, poition.y, -20));

            currentPlayerDecision.secondKey = key;

            currentPlayerDecision.EvaluateDecision();

            playerDecisions.Add(currentPlayerDecision);

            lineInstantiated = false;
        }
    }

    private void Update()
    {
        if (lineInstantiated && createdLine) createdLine.SetPosition(1, gameUI.mouseLocalPos);
    }

    public void ShuffleDictionary()
    {
        System.Random rand = new System.Random();

        picturesAndAnimals = picturesAndAnimals.OrderBy(x => rand.Next())
          .ToDictionary(item => item.Key, item => item.Value);
    }

    public void GenerateImages()
    {
        foreach (string key in picturesAndAnimals.Keys)
        {
            Texture2D animalImage = picturesAndAnimals[key];

            GameObject image = Instantiate(animalImagePrefab, gameUI.imagesPanel.transform);

            image.GetComponent<RawImage>().texture = animalImage;

            image.GetComponent<AnimalUiElement>().key = key;
        }
    }

    public void GenerateWords()
    {
        foreach (string key in picturesAndAnimals.Keys)
        {
            string animalName = key;

            GameObject text = Instantiate(animalNamePrefab, gameUI.wordsPanel.transform);

            text.GetComponent<Text>().text = animalName;

            text.GetComponent<AnimalUiElement>().key = key;
        }
    }

    IEnumerator SetImages(List<Animal> animals)
    {
        foreach (Animal animal in animals)
        {
            WWW www = new WWW(animal.pictureURL);
            yield return www;

            picturesAndAnimals.Add(animal.animalName, www.texture);

            www.Dispose();
        }

        GenerateImages();
        ShuffleDictionary();
        GenerateWords();
    }
}
