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
    forever.Show(new DialogCue("you can't delete me"));
    forever.dismissed += () => {
      Debug.Log("dismissed");
      forever.Show(new DialogCue("HAHAHAHA"));
    };
  }
}
