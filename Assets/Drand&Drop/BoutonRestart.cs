using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BoutonRestart : MonoBehaviour, IPointerClickHandler
{
    public GameObject Football;
    public GameObject popUpFootball;
    public GameManager gameManager;
    public GameObject boutonFin;

    void Start()
    {
    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Recommence");

        //// Désactive le pop-up Football
        //popUpFootball.SetActive(false);

        //// Réinitialise les cœurs
        //gameManager.ResetHearts();

        //// Désactive le bouton de fin
        //boutonFin.SetActive(false);

        //// Active l'objet Football
        //Football.SetActive(true);

        //// Obtient le composant NewBehaviourScript attaché à l'objet Football
        //NewBehaviourScript footballScript = Football.GetComponent<NewBehaviourScript>();

        //// Vérifie si le composant existe avant d'appeler la méthode resetPosition()
        //if (footballScript != null)
        //{
        //    // Appelle la méthode resetPosition() sur le composant NewBehaviourScript
        //    footballScript.ResetPosition();
        //}
        //else
        //{
        //    // Affiche un avertissement dans la console si le composant n'est pas trouvé
        //    Debug.LogWarning("Le composant NewBehaviourScript n'est pas attaché à l'objet Football.");
        //}
        SceneManager.LoadScene("Quel Sport");
    }
}
