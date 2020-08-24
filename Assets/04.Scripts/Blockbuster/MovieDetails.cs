using UnityEngine;

/// <summary>
/// A portable item with details about the movie.
/// </summary>
[CreateAssetMenu(fileName="New Movie Details", menuName="Scriptable Objects/Movie Details")]
public class MovieDetails : PortableItemDetails {
  [SerializeField]
  private string title;

  /// <summary>
  /// The title of the film.
  /// </summary>
  public string Title { get { return this.title; } }

  [SerializeField]
  private string studio;

  /// <summary>
  /// The studio that produced the film.
  /// </summary>
  public string Studio { get { return this.studio; } }

  [SerializeField]
  private string[] genres;

  /// <summary>
  /// The genre that the film falls in.
  /// </summary>
  public string[] Genres { get { return this.genres; } }

  [SerializeField]
  private int rentalPeriod;

  /// <summary>
  /// The rental period of the film.
  /// </summary>
  public int RentalPeriod { get { return this.rentalPeriod; } }

  [SerializeField]
  private string[] stars;

  /// <summary>
  /// The stars of the film.
  /// </summary>
  public string[] Stars { get { return this.stars; } }

  [SerializeField]
  private string director;

  /// <summary>
  /// The director of the film.
  /// </summary>
  public string Director { get { return this.director; } }

  [SerializeField]
  private string producer;

  /// <summary>
  /// The producer of the film.
  /// </summary>
  public string Producer { get { return this.producer; } }

  [SerializeField]
  private string summary;

  /// <summary>
  /// The summary of the film.
  /// </summary>
  public string Summary { get { return this.summary; } }

  [SerializeField]
  private string rating;

  /// <summary>
  /// The film's MPAA rating - e.g. PG-13, R, PG, etc.
  /// </summary>
  public string Rating { get { return this.rating; } }

  [SerializeField]
  private int year;

  /// <summary>
  /// The year the film was released.
  /// </summary>
  public int Year { get { return this.year; } }

  [SerializeField]
  private int duration;

  /// <summary>
  /// The duration of the film.
  /// </summary>
  public int Duration { get { return this.duration; } }

  [SerializeField]
  private bool color = true;

  /// <summary>
  /// Whether the film was shot in color or not.
  /// </summary>
  public bool Color { get { return this.color; } }
}
