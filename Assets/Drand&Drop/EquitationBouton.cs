using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquitationBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject popUpEquitation;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Debug.Log("Réussite");
        popUpEquitation.SetActive(false);
        gameManager.Success();
    }
}
