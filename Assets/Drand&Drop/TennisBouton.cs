using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TennisBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Ski1000;
    public GameObject popUpski1000;
    public GameObject popUpTennis;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Ski1000.SetActive(true);
        popUpTennis.SetActive(false);
        popUpski1000.SetActive(false);
    }
}
