using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlongéeButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Natation;
    public GameObject popUpPlongée;
    public GameObject popUpNatation;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Natation.SetActive(true);
        popUpNatation.SetActive(false);
        popUpPlongée.SetActive(false);
    }
}


