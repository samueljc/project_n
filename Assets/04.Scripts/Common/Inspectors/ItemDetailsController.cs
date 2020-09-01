using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Common item inspector.
/// </summary>
public class ItemDetailsController : MonoBehaviour {
  /// <summary>
  /// The graphical representation of the item.
  /// </summary>
  public Image Image;

  /// <summary>
  /// The name of the item.
  /// </summary>
  public LocalizedText Name;

  /// <summary>
  /// The description of the item.
  /// </summary>
  public LocalizedText Description;

  /// <summary>
  /// The portable item this is representing.
  /// </summary>
  private PortableItem item;

  /// <summary>
  /// The underlying item.
  /// </summary>
  public PortableItem Item {
    get { return this.item; }
    set {
      if (this.item != value) {
        UpdateDetails(this.item);
      }
    }
  }

  /// <summary>
  /// Update the detail fields with a new item.
  /// </summary>
  /// <param name="item">The new item to use.</param>
  private void UpdateDetails(in PortableItem item) {
    this.item = item;

    this.Image.sprite = this.item.worldSprite;

    this.Name.SetMessage(this.item.name);

    this.Description.SetMessage(this.item.description);
  }
}
