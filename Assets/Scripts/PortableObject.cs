using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PortableObjectOrientation {
  Dragging,
  Inventory,
}

/// <summary>
/// Game controller for an underlying <c>PortableItem</c>.
/// </summary>
/// <seealso cref="PortableItem" />
public class PortableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  /// <summary>
  /// The underlying item.
  /// </summary>
  [HideInInspector]
  public PortableItem item;

  /// <summary>
  /// The game object's transform.
  /// </summary>
  private RectTransform rectTransform;

  /// <summary>
  /// The game object's canvas group.
  /// </summary>
  private CanvasGroup canvasGroup;

  /// <summary>
  /// The game object's image.
  /// </summary>
  private Image image;

  /// <summary>
  /// The item's orientation.
  /// </summary>
  /// <remarks>
  /// This is used for determining which sprite to render.
  /// </remarks>
  private PortableObjectOrientation objectOrientation = PortableObjectOrientation.Inventory;

  /// <summary>
  /// The cursor offset when initating a drag event.
  /// </summary>
  private Vector2 startDragPosition;

  /// <summary>
  /// The details of the underlying item.
  /// </summary>
  public PortableItemDetails details {
    get { return this.item.details; }
  }

  /// <summary>
  /// The orientation of the item.
  /// </summary>
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

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.canvasGroup = GetComponent<CanvasGroup>();
    this.image = GetComponent<Image>();
  }

  /// <inheritdoc />
  void Start() {
    this.image.sprite = this.item.details.inventorySprite;
    this.image.alphaHitTestMinimumThreshold = 0.1f;
  }

  // drag handler methods

  /// <inheritdoc />
  public void OnBeginDrag(PointerEventData eventData) {
    this.startDragPosition = this.rectTransform.anchoredPosition;
    this.transform.SetAsLastSibling();
    this.canvasGroup.blocksRaycasts = false;
    this.canvasGroup.alpha = 0.8f;
    this.image.sprite = this.details.draggingSprite;
  }

  /// <inheritdoc />
  /// <remarks>
  /// </remarks>
  public void OnDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition += eventData.delta;
  }

  /// <inheritdoc />
  /// <remarks>
  /// </remarks>
  public void OnEndDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition = this.startDragPosition;
    this.canvasGroup.blocksRaycasts = true;
    this.canvasGroup.alpha = 1f;
    this.image.sprite = this.details.inventorySprite;
  }
}
