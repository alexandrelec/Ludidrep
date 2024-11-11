using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NatationBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Tennis;
    public GameObject popUpNatation;
    public GameObject popUpTennis;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Tennis.SetActive(true);
        popUpTennis.SetActive(false);
        popUpNatation.SetActive(false);
    }
}
