using UnityEngine;

public class VHSController : MonoBehaviour {
  public LocalizedText Title;

  public LocalizedText Studio;

  public LocalizedText Genre;

  public LocalizedText RentalPeriod;

  public LocalizedText Starring;

  public LocalizedText DirectedBy;

  public LocalizedText ProducedBy;

  public LocalizedText Synopsis;

  public LocalizedText Rating;

  public LocalizedText ReleaseYear;

  public LocalizedText Duration;

  public LocalizedText Color;

  private VHSDetails details;

  public VHSDetails Details {
    get {
      return this.details;
    }
    set {
      if (this.details != value) {

      }
    }
  }

  private void UpdateDetails(in VHSDetails details) {
    this.details = details;
    this.Title.SetText(this.details.Title);
    this.Studio.SetText(this.details.Studio);
    this.FormatDetail(this.RentalPeriod, this.details.RentalPeriod);
    this.FormatDetail(this.Starring, this.details.Stars);
  }

  private void FormatDetail(LocalizedText field, params object[] args) {
    field.SetMessage(field.Key, args);
  }
}