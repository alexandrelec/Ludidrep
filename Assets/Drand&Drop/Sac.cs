using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sac : MonoBehaviour, IDropHandler
{
    public GameManagerMS gameManager;
    public GameObject boutonSuite;
    public NewBehaviourScriptMS dragDropScript;
    private int dropCounter = 0;
    public GameManagerMS gameManagerreal;
    public Canvas popUpCanvas;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("on drop");
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            string objectName = droppedObject.name;
            Debug.Log("dropped object : " + objectName);
            string popUpName = "popUp" + objectName;
            GameObject popUp = droppedObject.transform.Find(popUpName)?.gameObject;
            gameManager.AddObjet(droppedObject);

            if (droppedObject.CompareTag("OUI"))
            {
                
                if (popUp != null)
                {
                    popUp.SetActive(true);
                    gameManagerreal.popupactif = true;

                    // Déplacer temporairement le pop-up en dehors du LayoutGroup pour le centrer
                    popUp.transform.SetParent(popUpCanvas.transform, false);

                    // Centrer le pop-up à l'écran
                    Vector3 centerScreenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                    popUp.transform.position = centerScreenPosition;
                }
                dropCounter++;
                droppedObject.SetActive(false);
            }

            
            Debug.Log("nombre d'objet : " + dropCounter);
            if (dropCounter == 5)
            {
                StartCoroutine(HandleFiveDrops(popUp));
            }

            if (droppedObject.CompareTag("NON"))
            {   
                SessionManagerMS.InstanceMS.AddErreur(droppedObject);
                Debug.Log("Loosing heart");
                if (popUp != null)
                {
                    popUp.SetActive(true);
                    gameManagerreal.popupactif = true;

                    // Déplacer temporairement le pop-up en dehors du LayoutGroup pour le centrer
                    popUp.transform.SetParent(popUpCanvas.transform, false);

                    // Centrer le pop-up à l'écran
                    Vector3 centerScreenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                    popUp.transform.position = centerScreenPosition;
                }

                droppedObject.GetComponent<NewBehaviourScriptMS>().ResetPosition();
                gameManager.LoseHeart(droppedObject);
                
            }
        }
    }

    private IEnumerator HandleFiveDrops(GameObject lastPopUp)
    {
        Debug.Log("jeu terminé");
        if (lastPopUp != null)
        {
            lastPopUp.SetActive(false);
        }

        boutonSuite.SetActive(true); // Activation du bouton de victoire

        yield return null;
    }
}
