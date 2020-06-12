using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suburb : MonoBehaviour {
  public DialogBox normal;
  public DialogBox autoAdvance;
  public DialogBox slow;
  public DialogBox multiple;
  public DialogBox forever;

  // Start is called before the first frame update
  void Start() {
    normal.Show(new DialogCue("Standard dialog"));
    autoAdvance.Show(new DialogCue("Auto-advance dialog", 1f));
    slow.Show(new DialogCue("Slow dialog", float.PositiveInfinity, 2f));
    multiple.Show(
      new DialogCue("Part 1", 2f),
      new DialogCue("Part 2", 2f, 1f),
      new DialogCue("Part 3", 2f)
    );
    DialogCue cue = new DialogCue("you can't delete me");
    DialogCue laughCue = new DialogCue("HAHAHAHA");
    cue.dismissed = () => {
      forever.Show(laughCue);
    };
    laughCue.dismissed = () => {
      forever.Show(laughCue);
    };
    forever.Show(cue);
  }
}
