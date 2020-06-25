using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for a single house in the suburb.
/// </summary>
class LawnController : MonoBehaviour {
  /// <summary>
  /// The state of the lawn.
  /// </summary>
  [SerializeField]
  private Inventory lawn;

  /// <summary>
  /// The <c>PortableItemController</c> prefab to use when instantiating
  /// objects.
  /// </summary>
  [SerializeField]
  private PortableItemController prefab;

  /// <summary>
  /// The lawn area that weeds can spawn in.
  /// </summary>
  private RectTransform rectTransform;

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
  }

  /// <inheritdoc />
  void OnEnable() {
    this.lawn.AddChangedHandler(this.OrderLawn);
  }

  /// <inheritdoc />
  void OnDisable() {
    this.lawn.RemoveChangedHandler(this.OrderLawn);
  }

  /// <inheritdoc />
  void Start() {
    foreach (PortableItem weed in this.lawn) {
      if (weed == null) {
        continue;
      }
      PortableItemController obj = GameObject.Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.Initialize(weed);
      this.PositionWeed(obj.gameObject);
    }
  }

  /// <summary>
  /// Place the weed in the bounds of the yard.
  /// </summary>
  /// <param name="weed">The weed to position.</param>
  private void PositionWeed(GameObject weed) {
    RectTransform transform = weed.transform as RectTransform;
    // Set the anchor to the bottom left of the container.
    transform.anchorMin = Vector2.zero;
    transform.anchorMax = Vector2.zero;
    // Move the object.
    transform.anchoredPosition = new Vector2(
      StaticRandom.Range(transform.pivot.x, rectTransform.rect.width - (transform.rect.width - transform.pivot.x)),
      StaticRandom.Range(transform.pivot.y, rectTransform.rect.height - (transform.rect.height - transform.pivot.y))
    );
  }

  /// <summary>
  /// Orders the lawn elements so that they're drawn correctly.
  /// </summary>
  private void OrderLawn() {
    List<RectTransform> transforms = new List<RectTransform>(this.lawn.Capacity);
    foreach (RectTransform transform in this.rectTransform) {
      transforms.Add(transform);
    }
    // Sort the items by y so that the layering is correct and then move them
    // to be the last sibling to enforce the draw order.
    transforms.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));
    foreach (RectTransform transform in transforms) {
      transform.SetAsLastSibling();
    }
  }
}
