using System.Text;
using UnityEngine;

/// <summary>
/// Controller for managing movie case details.
/// </summary>
public class MovieDetailsController : MonoBehaviour {
  /// <summary>
  /// The title field.
  /// </summary>
  public LocalizedText Title;

  /// <summary>
  /// The studio field.
  /// </summary>
  public LocalizedText Studio;

  /// <summary>
  /// The genre field.
  /// </summary>
  public LocalizedText Genres;

  /// <summary>
  /// The rental period field.
  /// </summary>
  public LocalizedText RentalPeriod;

  /// <summary>
  /// The starring field.
  /// </summary>
  public LocalizedText Starring;

  /// <summary>
  /// The director field.
  /// </summary>
  public LocalizedText DirectedBy;

  /// <summary>
  /// The producer field.
  /// </summary>
  public LocalizedText ProducedBy;

  /// <summary>
  /// The synopsis field.
  /// </summary>
  public LocalizedText Synopsis;

  /// <summary>
  /// The MPAA rating field.
  /// </summary>
  public LocalizedText Rating;

  /// <summary>
  /// The release year field.
  /// </summary>
  public LocalizedText ReleaseYear;

  /// <summary>
  /// The duration field.
  /// </summary>
  public LocalizedText Duration;

  /// <summary>
  /// The color field - i.e. in color or in black and white.
  /// </summary>
  public LocalizedText Color;

  private MovieDetails details;

  /// <summary>
  /// The underlying movie details presented on this case.
  /// </summary>
  public MovieDetails Details {
    get {
      return this.details;
    }
    set {
      if (this.details != value) {
        UpdateDetails(value);
      }
    }
  }

  /// <summary>
  /// Update the VHS case details with the provided movie details. This should
  /// take care of all localization.
  /// </summary>
  /// <param name="details">The details to show on the case.</param>
  private void UpdateDetails(in MovieDetails details) {
    this.details = details;

    this.Title.SetMessage(this.details.Title);

    this.Studio.SetMessage(this.details.Studio);

    StringBuilder localizedGenres = new StringBuilder();
    for (int i = 0; i < this.details.Genres.Length; ++i) {
      localizedGenres.Append(LocalizationManager.GetText(this.details.Genres[i]));
      // separate the genres with a /
      if (i < this.details.Genres.Length - 1) {
        localizedGenres.Append(" / ");
      }
    }
    this.FormatDetail(this.Genres, localizedGenres);

    this.FormatDetail(this.RentalPeriod, this.details.RentalPeriod);

    StringBuilder localizedStars = new StringBuilder();
    for (int i = 0; i < this.details.Stars.Length; ++i) {
      localizedStars.Append(LocalizationManager.GetText(this.details.Stars[i]));
      // separate the names with a bullet
      if (i < this.details.Stars.Length - 1) {
        localizedStars.Append(" • ");
      }
    }
    this.FormatDetail(this.Starring, localizedStars.ToString());

    string localizedDirector = LocalizationManager.GetText(this.details.Director);
    this.FormatDetail(this.DirectedBy, localizedDirector);

    string localizedProducer = LocalizationManager.GetText(this.details.Producer);
    this.FormatDetail(this.ProducedBy, localizedProducer);

    this.Synopsis.SetMessage(this.details.Summary);

    string localizedRating = LocalizationManager.GetText(this.details.Rating);
    this.FormatDetail(this.Rating, localizedRating);

    this.ReleaseYear.SetText(this.details.Year.ToString());

    this.FormatDetail(this.Duration, this.details.Duration);

    if (this.details.Color) {
      this.Color.SetText(LocalizationManager.GetText("vhs/color"));
    } else {
      this.Color.SetText(LocalizationManager.GetText("vhs/black & white"));
    }
  }

  /// <summary>
  /// Format a single detail field using the field's key and the given
  /// arguments for localization. If the provided argument should be
  /// localized, do so before passing it in.
  /// </summary>
  /// <param name="field">The VHS field to format</param>
  /// <param name="args">The format args</param>
  private void FormatDetail(LocalizedText field, params object[] args) {
    field.SetMessage(field.Key, args);
  }
}