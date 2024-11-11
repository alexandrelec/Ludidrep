using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonListButton : MonoBehaviour
{
    [SerializeField]
    private Text mytext;
    public int patientId; // Identifiant du patient associ� � ce bouton
   public void SetText(string textString)
    {
        mytext.text = textString;
    }

    

    // M�thode appel�e lors du clic sur le bouton
    public void OnButtonClick()
    {

        Debug.Log("Patient ID: " + patientId);
        // Faites ce que vous voulez avec l'identifiant du patient lorsque le bouton est cliqu�
        MainManager.Instance.currentidPatient = patientId;
        Debug.Log("current ID :" + MainManager.Instance.currentidPatient);
        
        SceneManager.LoadScene("Patient");
       
    }
}
