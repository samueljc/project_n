/// <summary>
/// Debug scene controller for checking how a movie's details render on a
/// case.
/// </summary>
class DebugMovieSceneController : UnityEngine.MonoBehaviour {
  public MovieDetails details;

  public MovieDetailsController vhsCase;
  
  private void Start() {
    vhsCase.Movie = details;
  }
}
