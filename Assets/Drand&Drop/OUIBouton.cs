using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OUIButton : MonoBehaviour, IPointerClickHandler
{
    public GameManagerMS gameManager;
    // Start is called before the first frame update
    void Start()
    {
    
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        transform.parent.gameObject.SetActive(false);
        gameManager.popupactif = false;
    }
}


