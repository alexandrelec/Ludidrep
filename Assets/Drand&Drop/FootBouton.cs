using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FootBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Plongée;
    public GameObject popUpFootball;
    public GameObject popUpPlongée;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Plongée.SetActive(true);
        popUpPlongée.SetActive(false);
        popUpFootball.SetActive(false);
    }
}


