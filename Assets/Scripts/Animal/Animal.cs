using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents an Animal and stores its name and a URL to download it from
/// </summary>
[Serializable]
public class Animal
{
    /// <summary>
    /// the name of the Animal
    /// </summary>
    public string animalName;

    /// <summary>
    /// The URL to downlad the Picture of that animal
    /// </summary>
    [TextArea]
    public string pictureURL;
}
