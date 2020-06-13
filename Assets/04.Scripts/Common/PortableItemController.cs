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
public class PortableItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
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
  /// The root canvas this object is attached to.
  /// </summary>
  private Canvas root;

  /// <summary>
  /// The game object's image.
  /// </summary>
  private Image image;

  /// <summary>
  /// The transform of the canvas to use when dragging.
  /// </summary>
  private Transform draggingCanvas;

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
  /// The transform of the parent object when initiating a drag event.
  /// </summary>
  private Transform startDragParent;

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
    // Get the root canvas for determining our scale factor.
    this.root = GetComponentInParent<Canvas>().rootCanvas;
    // TODO: How can I do this without a tag and without needing to share too
    // much information between portable objects and their containers?
    GameObject canvas = GameObject.FindGameObjectWithTag("DraggingCanvas");
    this.draggingCanvas = canvas.transform;
  }

  /// <inheritdoc />
  void Start() {
    this.image.sprite = this.item.details.inventorySprite;
    this.image.alphaHitTestMinimumThreshold = 0.1f;
  }

  // drag handler methods

  /// <inheritdoc />
  /// <remarks>
  /// When we start dragging we want to record some information so we can move
  /// this object back if necessary, then we need to put it on the dragging
  /// canvas to make sure it's above everything else.
  /// </remarks>
  public void OnBeginDrag(PointerEventData eventData) {
    this.startDragPosition = this.rectTransform.anchoredPosition;
    this.startDragParent = this.transform.parent;

    this.transform.SetParent(this.draggingCanvas);
    this.transform.SetAsLastSibling();

    this.canvasGroup.alpha = 0.8f;

    this.image.sprite = this.details.draggingSprite;
  }

  /// <inheritdoc />
  /// <remarks>
  /// While dragging we want to continuously update the position relative to
  /// the pointer.
  /// </remarks>
  public void OnDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition += eventData.delta / this.root.scaleFactor;
  }

  /// <inheritdoc />
  /// <remarks>
  /// After dragging, we want to reset this object to its original position.
  /// If it was dropped into a new inventory that inventory will take it, so
  /// we don't need to do special handling for that here.
  /// </remarks>
  public void OnEndDrag(PointerEventData eventData) {
    this.transform.SetParent(this.startDragParent);
    this.rectTransform.anchoredPosition = this.startDragPosition;

    this.canvasGroup.alpha = 1f;

    this.image.sprite = this.details.inventorySprite;
  }
}
