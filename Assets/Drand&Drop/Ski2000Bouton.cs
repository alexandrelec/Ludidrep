using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ski2000Bouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Parachute;
    public GameObject popUpparachute;
    public GameObject popUpSki2000;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Parachute.SetActive(true);
        popUpparachute.SetActive(false);
        popUpSki2000.SetActive(false);
    }
}
