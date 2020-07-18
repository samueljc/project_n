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

  /// <summary>
  /// A layout engine for placing objects with no overlap within the bounds
  /// of a collider.
  /// </summary>
  private RandomColliderLayout layout;

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.layout = GetComponent<RandomColliderLayout>();
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
    // Clear existing weeds.
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      GameObject child = this.rectTransform.GetChild(i).gameObject;
      if (LawnController.IsWeed(child)) {
        Destroy(child);
      }
    }

    // Add new weeds.
    for (int weedIndex = 0; weedIndex < this.lawn.Capacity; ++weedIndex) {
      PortableItem weed = this.lawn[weedIndex];
      if (weed == null) {
        continue;
      }

      Vector2 position;
      if (layout.FindPosition(this.rectTransform, out position)) {
        PortableItemController obj = GameObject.Instantiate(this.prefab, this.rectTransform);
        obj.Initialize(weed);
        // Set the anchor to the center because rect.min, rect.max, and the
        // collider are all relative to the center.
        RectTransform itemTransform = obj.transform as RectTransform;
        itemTransform.anchorMin.Set(0.5f, 0.5f);
        itemTransform.anchorMax.Set(0.5f, 0.5f);
        itemTransform.anchoredPosition = position;
      } else {
        // Couldn't place it after 100 attempts; clear it from the inventory so
        // we don't count it in the score.
        this.lawn[weedIndex] = null;
      }
    }
    this.OrderLawn();
  }

  /// <summary>
  /// Orders the lawn elements so that they're drawn correctly.
  /// </summary>
  private void OrderLawn() {
    List<RectTransform> transforms = new List<RectTransform>(this.lawn.Capacity);
    foreach (RectTransform transform in this.rectTransform) {
      if (LawnController.IsWeed(transform.gameObject)) {
        transforms.Add(transform);
      }
    }
    // Sort the items by y so that the layering is correct and then move them
    // to be the last sibling to enforce the draw order.
    transforms.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));
    foreach (RectTransform transform in transforms) {
      transform.SetAsLastSibling();
    }
  }

  /// <summary>
  /// Check if the given game object is a weed.
  /// </summary>
  /// <param name="obj">The item in question.</param>
  /// <returns>True if the object is a weed, false otherwise.</returns>
  private static bool IsWeed(GameObject obj) {
    return obj != null && obj.GetComponent<PortableItemController>() != null;
  }
}
