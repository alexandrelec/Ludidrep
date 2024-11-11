using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ski1000Bouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Basket;
    public GameObject popUpBasket;
    public GameObject popUpSki1000;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Basket.SetActive(true);
        popUpBasket.SetActive(false);
        popUpSki1000.SetActive(false);
    }
}
