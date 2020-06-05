using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PortableObjectOrientation {
  Dragging,
  Inventory,
}

/**
 * Portable Object is an interactable wrapper around a portable item that
 * supports being clicked and dragged into other inventories.
 */
public class PortableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  [HideInInspector]
  public PortableItem item;

  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;
  private Image image;
  private PortableObjectOrientation objectOrientation = PortableObjectOrientation.Inventory;

  private Vector2 startDragPosition;

  public PortableItemDetails details {
    get { return this.item.details; }
  }

  public PortableObjectOrientation orientation {
    get { return this.objectOrientation; }
    set {
      this.objectOrientation = orientation;
      switch (this.objectOrientation) {
        case PortableObjectOrientation.Inventory:
          this.image.sprite = this.details.inventorySprite;
          break;
        case PortableObjectOrientation.Dragging:
          this.image.sprite = this.details.draggingSprite;
          break;
      }
    }
  }

  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.canvasGroup = GetComponent<CanvasGroup>();
    this.image = GetComponent<Image>();
  }

  void Start() {
    this.image.sprite = this.item.details.inventorySprite;
    this.image.alphaHitTestMinimumThreshold = 0.1f;
  }

  // drag handler methods
  public void OnBeginDrag(PointerEventData eventData) {
    this.startDragPosition = this.rectTransform.anchoredPosition;
    this.transform.SetAsLastSibling();
    this.canvasGroup.blocksRaycasts = false;
    this.canvasGroup.alpha = 0.8f;
    this.image.sprite = this.details.draggingSprite;
  }

  public void OnDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition += eventData.delta;
  }

  public void OnEndDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition = this.startDragPosition;
    this.canvasGroup.blocksRaycasts = true;
    this.canvasGroup.alpha = 1f;
    this.image.sprite = this.details.inventorySprite;
  }
}
