using System.Collections.Generic;
using UnityEngine;

public enum JournalSource {
  // Sources of entropy for shelves
  Store_Shelf_Types,
  Store_Shelf_TypeCount,
  Store_Shelf_TotalCount,
  // Sources of entropy for aisles
  Store_Aisle_Types,
  Store_Aisle_TypeCount,
  Store_Aisle_TotalCount,
}

public struct JournalEntry {
  public JournalSource source;
  public float score;

  public JournalEntry(JournalSource source, float score) {
    this.source = source;
    this.score = score;
  }
}

public class JournalDay {
  private List<JournalEntry> entries;
  private float totalEntropy;

  public float TotalEntropy {
    get { return this.totalEntropy; }
  }

  public JournalDay() {
    this.entries = new List<JournalEntry>();
    this.totalEntropy = 0f;
  }

  public void Add(JournalEntry entry) {
    this.entries.Add(entry);
    this.totalEntropy += entry.score;
  }

  public void Add(JournalSource source, float score) {
    Debug.LogFormat("Adding journal entry for {0} : {1}", source, score);
    this.Add(new JournalEntry(source, score));
  }
}

[CreateAssetMenu(fileName="New Journal", menuName="Scriptable Objects/Journal")]
public class Journal : ScriptableObject {
  private List<JournalDay> journal;

  public JournalDay Today {
    get { return this.journal[this.journal.Count - 1]; }
  }

  public void OnEnable() {
    // TODO: should I just have an Init function and call it from the
    // WorldState instead of using OnEnable?
    this.journal = new List<JournalDay>();
    this.NewDay();
    // TODO: preseed with some initial journal entries to help people know
    // how the scoring works
  }

  public void NewDay() {
    this.journal.Add(new JournalDay());
  }

  public void Add(JournalEntry entry) {
    this.Today.Add(entry);
  }

  public void Add(JournalSource source, float score) {
    this.Today.Add(source, score);
  }
}
