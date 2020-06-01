using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Portable Object is an interactable wrapper around a portable item that
 * supports being clicked and dragged into other inventories.
 */
public class PortableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  // TODO: pick a sprite relative to the PortableItem?
  [HideInInspector] public PortableItem item;

  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;
  private Image image;

  private Vector2 startDragPosition;

  public PortableItemDetails details {
    get { return this.item.details; }
  }

  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.canvasGroup = GetComponent<CanvasGroup>();
    this.image = GetComponent<Image>();
  }

  void Start() {
    this.image.sprite = this.item.details.forwardSprite;
    this.image.alphaHitTestMinimumThreshold = 0.1f;
  }

  // drag handler methods
  public void OnBeginDrag(PointerEventData eventData) {
    this.startDragPosition = this.rectTransform.anchoredPosition;
    this.transform.SetAsLastSibling();
    this.canvasGroup.blocksRaycasts = false;
    this.canvasGroup.alpha = 0.8f;
  }

  public void OnDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition += eventData.delta;
  }

  public void OnEndDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition = this.startDragPosition;
    this.canvasGroup.blocksRaycasts = true;
    this.canvasGroup.alpha = 1f;
  }
}
