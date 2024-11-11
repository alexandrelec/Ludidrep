using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScriptMS : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    public GameManagerMS gameManager;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 initialPosition; // Sauvegarde de la position initiale de l'objet
    private CanvasGroup canvasGroup; // Ajout du CanvasGroup

    public void Awake()
    {
        Debug.Log("Awakened Object");
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Sauvegarde de la position initiale
        canvasGroup = GetComponent<CanvasGroup>(); // Récupère le CanvasGroup
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); // Ajoute un CanvasGroup s'il n'existe pas
        }
    }

    public void Update()
    {
        // Bloquer les interactions si un popup est actif
        if (gameManager.popupactif || gameManager.Objetdansliste(this.gameObject))
        {
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canvasGroup.blocksRaycasts) return; // Ne pas commencer le glisser-déposer si les interactions sont bloquées

        if (gameManager.Objetdansliste(this.gameObject))
        {
            Debug.Log("L'objet est bloqué et ne peut pas être déplacé.");
            return; // Ne pas commencer le glisser-déposer si l'objet est bloqué
        }
        else
        {
            Debug.Log("OnBeginDrag " + this.gameObject.name);
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canvasGroup.blocksRaycasts) return; // Ne pas glisser si les interactions sont bloquées

        Debug.Log("OnDrag");
        if (isDragging)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canvasGroup.blocksRaycasts) return; // Ne pas commencer le glisser-déposer si les interactions sont bloquées

        if (gameManager.Objetdansliste(this.gameObject))
        {
            Debug.Log("L'objet est bloqué et ne peut pas être déplacé.");
            return; // Ne pas commencer le glisser-déposer si l'objet est bloqué
        }
        else
        {
            Debug.Log("OnPointerDown");
            if (!isDragging)
            {
                isDragging = true;
            }
        }
    }

    public void StartDragging()
    {
        if (!canvasGroup.blocksRaycasts) return; // Ne pas commencer le glisser-déposer si les interactions sont bloquées

        if (gameManager.Objetdansliste(this.gameObject))
        {
            Debug.Log("L'objet est bloqué et ne peut pas être déplacé.");
            return; // Ne pas commencer le glisser-déposer si l'objet est bloqué
        }
        else
        {
            if (!isDragging)
            {
                Debug.Log("Début du déplacement de l'objet");
                isDragging = true;
            }
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = initialPosition; // Réinitialiser la position de l'objet à sa position initiale
    }
}
