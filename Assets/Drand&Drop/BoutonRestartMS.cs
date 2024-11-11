using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonRestartMS : MonoBehaviour, IPointerClickHandler
{
    public GameManager gameManager;
    public GameObject boutonFin;

    void Start()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Recommence");

        // Réinitialise les coeurs 
        gameManager.ResetHearts();

        // Réinitialise la position de tous les objets au même niveau que ce bouton dans la hiérarchie, qui sont "movable"
        ResetAndActivateTaggedSiblings();

        boutonFin.SetActive(false);


    }

    private void ResetAndActivateTaggedSiblings()
    {
        // Obtient le parent de ce bouton
        Transform parentTransform = transform.parent;

        // Vérifie si le parent existe
        if (parentTransform != null)
        {
            // Obtient le parent du parent (grand-parent)
            Transform grandParentTransform = parentTransform.parent;

            // Si le grand-parent existe
            if (grandParentTransform != null)
            {
                // Itère sur chaque enfant du grand-parent (oncles et tantes)
                foreach (Transform uncleOrAunt in grandParentTransform)
                {
                    // Vérifie le tag de l'objet
                    if (uncleOrAunt.CompareTag("OUI") || uncleOrAunt.CompareTag("NON"))
                    {
                        // Active l'objet
                        uncleOrAunt.gameObject.SetActive(true);

                        // Obtient le composant NewBehaviourScript attaché à l'objet
                        NewBehaviourScript script = uncleOrAunt.GetComponent<NewBehaviourScript>();

                        // Si le composant NewBehaviourScript existe
                        if (script != null)
                        {
                            // Réinitialise la position de l'objet
                            script.ResetPosition();
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Le parent du parent n'existe pas.");
            }
        }
        else
        {
            Debug.LogWarning("Le bouton n'a pas de parent.");
        }
    }
}
