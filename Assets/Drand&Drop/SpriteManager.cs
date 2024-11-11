using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{    
     // Liste des sprites représentant les différents états des cœurs
    public List<Sprite> heartSprites;

    // Propriété pour accéder à la liste des sprites de cœur
    public List<Sprite> HeartSprites
    {
        get { return heartSprites; }
    }
}

