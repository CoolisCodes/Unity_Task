using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

/// <summary>
/// The Game Manager class is responsible for handling the Data, UI and the decisions that the player made.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// static instance fo the class, to access it form anywhere
    /// </summary>
    private static GameManager instance;

    /// <summary>
    /// list of all the available animals
    /// </summary>
    public List<Animal> animals = new List<Animal>();

    /// <summary>
    /// list of all the decisions of the player
    /// </summary>
    public List<PlayerDecision> playerDecisions = new List<PlayerDecision>();

    /// <summary>
    /// a private instance of a player decision to be handled by the class
    /// </summary>
    private PlayerDecision currentPlayerDecision;

    /// <summary>
    /// a dicitionary containing the names and the pictures of all the available animals
    /// </summary>
    public Dictionary<string, Texture2D> picturesAndAnimals = new Dictionary<string, Texture2D>();

    /// <summary>
    /// prefab UI element representing the Picture of an animal;
    /// </summary>
    public GameObject animalImagePrefab;

    /// <summary>
    /// prefab UI element representing the Name of an animal;
    /// </summary>
    public GameObject animalNamePrefab;

    /// <summary>
    /// prefab UI element representing the line that is drawn by the user.
    /// </summary>
    public GameObject linePrebab;

    /// <summary>
    /// a private instance of a line to be handled by the class
    /// </summary>
    private LineRenderer createdLine;

    /// <summary>
    /// the UI type of the last clicked element.
    /// </summary>
    private AnimalUiElement lastClickedElement;

    /// <summary>
    /// The app UI
    /// </summary>
    public GameUI gameUI;

    /// <summary>
    /// the color that the created line will turn to if the decision is correct
    /// </summary>
    public Color correctColor;

    /// <summary>
    /// the color that the created line will turn to if the decision is incorrect
    /// </summary>
    public Color incorrectColor;

    /// <summary>
    /// this degelate gets invoked each time a UI element is clicked
    /// </summary>
    public Action<string, Vector3, AnimalUiElement> onUiElementClicked;

    /// <summary>
    /// turns true when the line is instantiated and false when the player decision is finalized
    /// </summary>
    private bool lineInstantiated = false;

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
            DontDestroyOnLoad(this);
            StartCoroutine(SetImages(animals));
        }
    }

    /// <summary>
    /// This method is assigned to the onUiElementClicked delegate
    /// </summary>
    /// <param name="key"> the name of the animal on the clicked UI element </param>
    /// <param name="poition"> the position of the clicked UI element </param>
    /// <param name="uiType"> the type of the clicked Ui element </param>
    public void HandleLine(string key, Vector3 poition, AnimalUiElement animalUiElement)
    {
        if (!animalUiElement.taken)
        {
            if (!lineInstantiated)
            {
                Debug.Log($"the user first clicked on {key}");

                lastClickedElement = animalUiElement;

                GameObject lineRenderer = Instantiate(linePrebab, gameUI.transform);

                createdLine = lineRenderer.GetComponent<LineRenderer>();

                createdLine.SetPosition(0, new Vector3(poition.x, poition.y, -20));

                currentPlayerDecision = new PlayerDecision(createdLine, correctColor, incorrectColor);

                currentPlayerDecision.firstKey = key;

                lineInstantiated = true;
            }
            else if (lineInstantiated && lastClickedElement.uiType != animalUiElement.uiType)
            {
                Debug.Log($"the user last clicked on {key}");

                createdLine.SetPosition(1, new Vector3(poition.x, poition.y, -20));

                currentPlayerDecision.secondKey = key;

                currentPlayerDecision.EvaluateDecision();

                playerDecisions.Add(currentPlayerDecision);

                lineInstantiated = false;
            }

            animalUiElement.taken = true;
        }



    }

    /// <summary>
    /// In the Update method once the line is instantiated, the secont position follows the position of the mouse.
    /// </summary>
    private void Update()
    {
        if (lineInstantiated && createdLine) createdLine.SetPosition(1, gameUI.mouseLocalPos);
    }

    /// <summary>
    /// Shuffling the picturesAndAnimals dictionary with Linq magic
    /// </summary>
    public void ShuffleDictionary()
    {
        System.Random rand = new System.Random();

        picturesAndAnimals = picturesAndAnimals.OrderBy(x => rand.Next())
          .ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// Getting the links of a given animal list and proceeds to download them.
    /// Once everything is downloaded the appropriate UI elements are generated.
    /// </summary>
    /// <param name="animals"> a list of animals to be downloaded </param>
    /// <returns></returns>
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

    /// <summary>
    /// Generating a picture of an animal on the Images Panel
    /// </summary>
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

    /// <summary>
    /// Generating a text element with the name of the animal in the Words Panel
    /// </summary>
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


}
