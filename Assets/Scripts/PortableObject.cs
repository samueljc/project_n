using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PortableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  // TODO: pick a sprite relative to the PortableItem?
  public PortableItem item;

  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;

  private Vector2 startDragPosition;

  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();
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
