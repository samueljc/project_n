using UnityEngine;

/// <summary>
/// Controls the inspection of a portable item.
/// </summary>
public class Inspector : MonoBehaviour {
  /// <summary>
  /// The portable item event handler for inspect events.
  /// </summary>
  [SerializeField]
  private PortableItemEvent inspectEvent;

  /// <summary>
  /// The inspector canvas.
  /// </summary>
  [SerializeField]
  private GameObject canvas;

  /// <summary>
  /// Generic item details view.
  /// </summary>
  private ItemDetailsController itemDetails;

  /// <summary>
  /// The specific movie details view.
  /// </summary>
  private MovieDetailsController movieDetails;

  /// <inheritdoc />
  private void Awake() {
    this.movieDetails = this.canvas.GetComponentInChildren<MovieDetailsController>();
    this.itemDetails = this.canvas.GetComponentInChildren<ItemDetailsController>();
  }

  /// <inheritdoc />
  private void OnEnable() {
    this.inspectEvent.AddHandler(this.Show);
  }

  /// <inheritdoc />
  private void OnDisable() {
    this.inspectEvent.RemoveHandler(this.Show);
  }

  /// <summary>
  /// Show the given item.
  /// </summary>
  /// <param name="item">The item to show.</param>
  public void Show(PortableItem item) {
    this.canvas.SetActive(true);
    this.movieDetails.gameObject.SetActive(false);
    this.itemDetails.gameObject.SetActive(false);

    if (item?.details is MovieDetails movie) {
      this.movieDetails.gameObject.SetActive(true);
      this.movieDetails.Movie = movie;
    } else {
      this.itemDetails.gameObject.SetActive(true);
      this.itemDetails.Item = item;
    }
  }

  /// <summary>
  /// Hide the insector view.
  /// </summary>
  public void Hide() {
    this.canvas.SetActive(false);
  }
}