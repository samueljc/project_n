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
  public PortableItem item;

  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;
  private Image image;

  private Vector2 startDragPosition;

  public PortableItemDetails details {
    get { return this.item.details; }
  }

  void Awake() {
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();
    image = GetComponent<Image>();
  }

  void Start() {
    image.sprite = this.item.details.forwardSprite;
  }

  // drag handler methods
  public void OnBeginDrag(PointerEventData eventData) {
    this.startDragPosition = this.rectTransform.anchoredPosition;
    canvasGroup.blocksRaycasts = false;
    canvasGroup.alpha = 0.8f;
  }

  public void OnDrag(PointerEventData eventData) {
    rectTransform.anchoredPosition += eventData.delta;
  }

  public void OnEndDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition = this.startDragPosition;
    canvasGroup.blocksRaycasts = true;
    canvasGroup.alpha = 1f;
  }
}
