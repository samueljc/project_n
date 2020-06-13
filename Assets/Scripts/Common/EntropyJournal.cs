using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The source of a journal entry.
/// </summary>
/// <remarks>
/// This is used in conjunction with the score to generate a string.
/// </remarks>
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

/// <summary>
/// A single entry in the journal.
/// </summary>
public struct JournalEntry {
  /// <summary>
  /// The source of the entry.
  /// </summary>
  public JournalSource source;

  /// <summary>
  /// The entropy score of this entry.
  /// </summary>
  public float score;

  /// <summary>
  /// Create a new journal entry.
  /// </summary>
  /// <param name="source">The source of the entry.</param>
  /// <param name="score">The entropy score of this entry.</param>
  public JournalEntry(JournalSource source, float score) {
    this.source = source;
    this.score = score;
  }
}

/// <summary>
/// A single day in the journal.
/// </summary>
public class JournalDay {
  /// <summary>
  /// List of entries for this day.
  /// </summary>
  private List<JournalEntry> entries;

  /// <summary>
  /// Total entropy for this day.
  /// </summary>
  private float totalEntropy;

  /// <summary>
  /// The total entropy of all events thus far for the day.
  /// </summary>
  public float entropy {
    get { return this.totalEntropy; }
  }

  /// <summary>
  /// Create a new, empty journal day.
  /// </summary>
  public JournalDay() {
    this.entries = new List<JournalEntry>();
    this.totalEntropy = 0f;
  }

  /// <summary>
  /// Add an journal entry to the day.
  /// </summary>
  /// <param name="entry">The entry to add to the journal.</param>
  public void Add(JournalEntry entry) {
    this.entries.Add(entry);
    this.totalEntropy += entry.score;
  }

  /// <summary>
  /// Add an journal entry to the day.
  /// </summary>
  /// <param name="source">The source of the entry.</param>
  /// <param name="score">The entropy score of the entry.</param>
  public void Add(JournalSource source, float score) {
    Debug.LogFormat("Adding journal entry for {0} : {1}", source, score);
    this.Add(new JournalEntry(source, score));
  }
}

/// <summary>
/// A list of days representing events that happened and the total change in
/// the world entropy.
/// </summary>
[CreateAssetMenu(fileName="New Entropy Journal", menuName="Scriptable Objects/Entropy Journal")]
public class EntropyJournal : ScriptableObject {
  /// <summary>
  /// The list of days.
  /// </summary>
  [NonSerialized]
  private List<JournalDay> journal;

  /// <summary>
  /// The current day in the journal.
  /// </summary>
  public JournalDay today {
    get { return this.journal[this.journal.Count - 1]; }
  }

  /// <inheritdoc />
  void OnEnable() {
    // TODO: should I just have an Init function and call it from the
    // WorldState instead of using OnEnable?
    this.journal = new List<JournalDay>();
    this.NewDay();
    // TODO: preseed with some initial journal entries to help people know
    // how the scoring works
  }

  /// <summary>
  /// Start a new day in the journal.
  /// </summary>
  public void NewDay() {
    this.journal.Add(new JournalDay());
  }

  /// <summary>
  /// Add an entry to today's journal entry.
  /// </summary>
  /// <param name="entry">The entry to add to the journal.</param>
  public void Add(JournalEntry entry) {
    this.today.Add(entry);
  }

  /// <summary>
  /// Add an entry to today's journal entry.
  /// </summary>
  /// <param name="source">The source of the entry.</param>
  /// <param name="score">The entropy score of the entry.</param>
  public void Add(JournalSource source, float score) {
    this.today.Add(source, score);
  }
}
