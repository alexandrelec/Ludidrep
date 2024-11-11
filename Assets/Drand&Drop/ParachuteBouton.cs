using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParachuteBouton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Equitation;
    public GameObject popUpEquitation;
    public GameObject popUpParachute;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Equitation.SetActive(true);
        popUpParachute.SetActive(false);
        popUpEquitation.SetActive(false);
    }
}
