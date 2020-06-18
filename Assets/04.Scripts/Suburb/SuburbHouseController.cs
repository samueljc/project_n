using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for a single house in the suburb.
/// </summary>
class SuburbHouseController : MonoBehaviour {
  /// <summary>
  /// The state of this house.
  /// </summary>
  [SerializeField]
  private SuburbHouseState state;

  /// <summary>
  /// The <c>PortableItemController</c> prefab to use when instantiating
  /// objects.
  /// </summary>
  [SerializeField]
  private PortableItemController prefab;

  /// <summary>
  /// The factory to generate weeds.
  /// </summary>
  [SerializeField]
  private PortableItemFactory weedFactory;

  /// <summary>
  /// The trashcan to dispose of weeds.
  /// </summary>
  [SerializeField]
  private TrashCanController trashCan;

  /// <summary>
  /// The lawn area that weeds can spawn in.
  /// </summary>
  [SerializeField]
  private RectTransform lawn;

  /// <summary>
  /// The number of lawn children in the last cycle.
  /// </summary>
  private int previousLawnChildCount;

  /// <inheritdoc />
  void Update() {
    if (this.previousLawnChildCount < this.lawn.childCount) {
      this.OrderLawn();
    }
    this.previousLawnChildCount = this.lawn.childCount;
  }

  /// <inheritdoc />
  void Start() {
    for (int i = 0; i < this.state.weeds; ++i) {
      PortableItem weed = this.weedFactory.CreateRandomItem();
      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.lawn);
      obj.item = weed;
      // TODO: I think we could probably extract some of this out into the
      // PortableItemController class as we generally want to share the current
      // sprite's pivot and size.
      RectTransform weedTransform = obj.transform as RectTransform;
      // Set the anchor to the bottom left of the container.
      weedTransform.anchorMin = Vector2.zero;
      weedTransform.anchorMax = Vector2.zero;
      // Set the size to the sprite size.
      weedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weed.inventorySprite.rect.width);
      weedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, weed.inventorySprite.rect.height);
      // Set the pivot to the sprite pivot. Need to divide by the sprite size
      // as the sprite pivot is in pixels but we need it as a fraction.
      weedTransform.pivot = weed.inventorySprite.pivot / weed.inventorySprite.rect.size;
      // Move the object.
      weedTransform.anchoredPosition = new Vector2(
        StaticRandom.Range(weedTransform.pivot.x, lawn.rect.width - (weedTransform.rect.width - weedTransform.pivot.x)),
        StaticRandom.Range(weedTransform.pivot.y, lawn.rect.height - (weedTransform.rect.height - weedTransform.pivot.y))
      );
    }
  }

  /// <inheritdoc />
  void OnEnable() {
    this.trashCan.AddDestroyedHandler(this.RemoveWeed);
  }

  /// <inheritdoc />
  void OnDisable() {
    this.trashCan.RemoveDestroyedHandler(this.RemoveWeed);
  }

  /// <summary>
  /// Orders the lawn elements so that they're drawn correctly.
  /// </summary>
  private void OrderLawn() {
    List<RectTransform> transforms = new List<RectTransform>(this.state.weeds);
    foreach (RectTransform transform in this.lawn) {
      transforms.Add(transform);
    }
    // Sort the items by y so that the layering is correct and then move them
    // to be the last sibling to enforce the draw order.
    transforms.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));
    foreach (RectTransform transform in transforms) {
      transform.SetAsLastSibling();
    }
  }

  /// <summary>
  /// Remove a weed from the house state.
  /// </summary>
  /// <param name="item">The removed weed.</param>
  private void RemoveWeed(PortableItem item) {
    this.state.weeds -= 1;
  }
}