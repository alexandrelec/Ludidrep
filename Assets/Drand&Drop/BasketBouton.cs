using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasketBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Ski2000;
    public GameObject popUpBasket;
    public GameObject popUpski2000;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Ski2000.SetActive(true);
        popUpski2000.SetActive(false);
        popUpBasket.SetActive(false);
    }
}
