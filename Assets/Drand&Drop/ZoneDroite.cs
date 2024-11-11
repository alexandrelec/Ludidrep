using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoneDroite : MonoBehaviour, IDropHandler
{
    public GameManager gameManager;
    public GameObject popUpFootball;
    public NewBehaviourScript dragDropScript;
    public Canvas popUpCanvas;
    public Transform content;
    public Transform contentgauche;


    private void Start()
    {
        // Réassignez les références ici si nécessaire
        if (content == null)
        {
            content = GameObject.Find("Content").transform; // Assurez-vous que le nom est correct
        }

        if (popUpCanvas == null)
        {
            popUpCanvas = GameObject.Find("PopUpCanvas").GetComponent<Canvas>(); // Assurez-vous que le nom est correct
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        
        {
            // Récupérez le tag de l'objet déposé
                string objectTag = droppedObject.tag;


            if (droppedObject.CompareTag("Football") || droppedObject.CompareTag("Natation") || droppedObject.CompareTag("Tennis") || droppedObject.CompareTag("Ski1000") || droppedObject.CompareTag("Basket") || droppedObject.CompareTag("Equitation"))
            {
                // Générez le nom du pop-up en concaténant "popUp" avec le tag de l'objet déposé
                string popUpName = "popUp" + objectTag;

                // Recherchez le pop-up correspondant par son nom
                GameObject popUp = droppedObject.transform.Find(popUpName)?.gameObject;
                if (popUp != null)
                {
                    // Activer le pop-up
                    popUp.SetActive(true);

                    // Déplacer temporairement le pop-up en dehors du LayoutGroup pour le centrer
                    popUp.transform.SetParent(popUpCanvas.transform, false);

                    // Centrer le pop-up sur l'écran
                    RectTransform popUpRectTransform = popUp.GetComponent<RectTransform>();
                    if (popUpRectTransform != null)
                    {
                        // Définir la taille fixe du pop-up
                        float popUpWidth = 120f; // Remplacez cette valeur par la largeur souhaitée
                        float popUpHeight = 100f; // Remplacez cette valeur par la hauteur souhaitée

                        // Utiliser les ancrages pour centrer le pop-up
                        popUpRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        popUpRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        popUpRectTransform.pivot = new Vector2(0.5f, 0.5f);

                        // Définir la taille et la position
                        popUpRectTransform.sizeDelta = new Vector2(popUpWidth, popUpHeight);
                        popUpRectTransform.anchoredPosition = Vector2.zero;
                    }

                    // Optionnel : replacer le pop-up dans la hiérarchie si nécessaire
                    // popUp.transform.SetParent(droppedObject.transform);
                }
                
                droppedObject.transform.SetParent(content);
                gameManager.AddObjet(droppedObject);
                droppedObject.transform.localScale = Vector3.one;

                //gameManager.AugmenterNombre();



            }

            if (droppedObject.CompareTag("Plongée") || droppedObject.CompareTag("Ski2000") || droppedObject.CompareTag("Parachute"))
            {
                if (MainManager.Instance.currentidPatient > -1)
                {
                    // Générez le nom du pop-up en concaténant "popUp" avec le tag de l'objet déposé
                    string popUpName = "popUp" + objectTag + "fail";

                    // Recherchez le pop-up correspondant par son nom
                    GameObject popUp = droppedObject.transform.Find(popUpName)?.gameObject;

                    if (popUp != null)
                    {
                        // Activer le pop-up
                        popUp.SetActive(true);

                        // Déplacer temporairement le pop-up en dehors du LayoutGroup pour le centrer
                        popUp.transform.SetParent(popUpCanvas.transform, false);

                        // Centrer le pop-up sur l'écran
                        RectTransform popUpRectTransform = popUp.GetComponent<RectTransform>();
                        if (popUpRectTransform != null)
                        {
                            // Définir la taille fixe du pop-up
                            float popUpWidth = 120f; // Remplacez cette valeur par la largeur souhaitée
                            float popUpHeight = 100f; // Remplacez cette valeur par la hauteur souhaitée

                            // Utiliser les ancrages pour centrer le pop-up
                            popUpRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                            popUpRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                            popUpRectTransform.pivot = new Vector2(0.5f, 0.5f);

                            // Définir la taille et la position
                            popUpRectTransform.sizeDelta = new Vector2(popUpWidth, popUpHeight);
                            popUpRectTransform.anchoredPosition = Vector2.zero;
                        }

                        // Optionnel : replacer le pop-up dans la hiérarchie si nécessaire
                        // popUp.transform.SetParent(droppedObject.transform);
                        // Debug.Log("Loosing heart");
                        gameManager.LoseHeart(droppedObject);
                        droppedObject.transform.SetParent(contentgauche);
                        gameManager.AddObjet(droppedObject);
                        droppedObject.transform.localScale = Vector3.one;
                    }
                }
                else
                {
                    droppedObject.GetComponent<NewBehaviourScript>().ResetPosition();
                }

                
            }
            
        }
    }

}

public class MyScript : MonoBehaviour
{
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Debug.Log("This GameObject has a Rect Transform component with Anchors.");
        }
    }
}
