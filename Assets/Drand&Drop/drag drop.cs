using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform movementArea; // La zone de déplacement
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 initialPosition; // Sauvegarde de la position initiale de l'objet
    public GameManager gameManager;
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
        if (gameManager.Objetdansliste(this.gameObject))
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
        if (!canvasGroup.blocksRaycasts) return;

        if (gameManager.Objetdansliste(this.gameObject))
        {
            Debug.Log("L'objet est bloqué et ne peut pas être déplacé.");
            return; // Ne pas commencer le glisser-déposer si l'objet est bloqué
        }
        else
        {
            Debug.Log("OnBeginDrag" + this.gameObject.name);
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canvasGroup.blocksRaycasts) return;

        if (isDragging)
        {
            // Calculer la nouvelle position
            Vector2 newPosition = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
            // Limiter la nouvelle position aux limites de la zone de déplacement
            rectTransform.anchoredPosition = ClampToArea(newPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canvasGroup.blocksRaycasts) return;

        if (gameManager.Objetdansliste(this.gameObject))
        {
            Debug.Log("L'objet est bloqué et ne peut pas être déplacé.");
            return; // Ne pas commencer le glisser-déposer si l'objet est bloqué
        }
        else
        {
            Debug.Log("OnPointerDown");
            // Réinitialiser l'état de déplacement si l'objet n'est pas déjà en train d'être déplacé
            if (!isDragging)
            {
                isDragging = true;
            }
        }
    }

    public void StartDragging()
    {
        if (!canvasGroup.blocksRaycasts) return;

        if (!isDragging)
        {
            Debug.Log("Début du déplacement de l'objet");
            isDragging = true;
        }
    }

    public void ResetPosition()
    {
        // Réinitialiser la position de l'objet à sa position initiale
        rectTransform.anchoredPosition = initialPosition;
    }

    private Vector2 ClampToArea(Vector2 position)
    {
        // Convertir les positions en coordonnées locales du parent
        Vector2 minPosition = movementArea.TransformPoint(movementArea.rect.min);
        Vector2 maxPosition = movementArea.TransformPoint(movementArea.rect.max);

        Vector2 localMin = rectTransform.parent.InverseTransformPoint(minPosition);
        Vector2 localMax = rectTransform.parent.InverseTransformPoint(maxPosition);

        // Calculer les limites pour l'objet dans la zone de déplacement
        float objectWidth = rectTransform.rect.width;
        float objectHeight = rectTransform.rect.height;

        // Calculer les limites pour l'objet dans la zone de déplacement en coordonnées locales
        float minX = localMin.x + objectWidth / 2;
        float maxX = localMax.x - objectWidth / 2;
        float minY = localMin.y + objectHeight / 2;
        float maxY = localMax.y - objectHeight / 2;

        // Restreindre la position de l'objet aux limites calculées
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
}
