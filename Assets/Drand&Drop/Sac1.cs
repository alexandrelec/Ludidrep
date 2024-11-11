using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sac1 : MonoBehaviour, IDropHandler
{
    public GameManagerMS gameManager;
    
    public NewBehaviourScriptMS dragDropScript;
    private int dropCounter = 0;
    public Canvas popUpCanvas;
    public GameManagerMS gameManagerreal;

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
            if (dropCounter == 7)
            {
                StartCoroutine(HandleSevenDrops(popUp));
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

    private IEnumerator HandleSevenDrops(GameObject lastPopUp)
    {
        Debug.Log("jeu terminé");
        if (lastPopUp != null)
        {
            lastPopUp.SetActive(false);
        }

        gameManagerreal.Success(); // Activation du bouton de victoire
        
        yield return null;
    }
}
