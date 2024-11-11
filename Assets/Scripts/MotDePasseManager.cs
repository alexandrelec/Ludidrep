using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MotDePasseManager : MonoBehaviour
{
    public InputField motdepasse;
    public Text errormessage;
    public GameObject popupinfos;
    public Button togglePasswordButton;
    public Text togglePasswordButtonText; // Text component on the button to indicate the visibility state
    public Sprite visibleSprite; // Sprite for visible state
    public Sprite hiddenSprite;  // Sprite for hidden state
    private Image toggleButtonImage; // Image component of the button

    private bool isPasswordVisible = false;

    void Start()
    {
        // Configure le champ de mot de passe pour masquer les caractères
        motdepasse.contentType = InputField.ContentType.Password;
        motdepasse.ForceLabelUpdate(); // Met à jour l'affichage pour prendre en compte le changement de contentType

        // Configure le bouton pour afficher/masquer le mot de passe
        toggleButtonImage = togglePasswordButton.GetComponent<Image>();
        togglePasswordButton.onClick.AddListener(TogglePasswordVisibility);
        UpdateToggleButton();
    }

    public void Valider()
    {
        if (motdepasse.text.Trim().ToLower() == "ludidrep")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            errormessage.text = "Mot de passe incorrect \n Indice : ludi....";
            motdepasse.text = "";
        }
    }

    public void Infos()
    {
        popupinfos.SetActive(true);
    }

    public void FermerInfos()
    {
        popupinfos.SetActive(false);
    }

    private void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;
        motdepasse.contentType = isPasswordVisible ? InputField.ContentType.Standard : InputField.ContentType.Password;
        motdepasse.ForceLabelUpdate(); // Met à jour l'affichage pour prendre en compte le changement de contentType
        UpdateToggleButton();
    }

    private void UpdateToggleButton()
    {
        togglePasswordButtonText.text = isPasswordVisible ? "Masquer" : "Afficher";
        toggleButtonImage.sprite = isPasswordVisible ? visibleSprite : hiddenSprite;
    }
}
