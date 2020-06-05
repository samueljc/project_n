using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The numbers used for determining entropy throughout the game.
/// </summary>
[CreateAssetMenu(fileName="New Entropy Numbers", menuName="Scriptable Objects/Entropy Numbers")]
public class EntropyNumbers : ScriptableObject {
  [NonSerialized]
  private List<int> veryGoodNumbers;
  [NonSerialized]
  private List<int> goodNumbers;
  [NonSerialized]
  private List<int> veryBadNumbers;

  /// <inheritdoc />
  void OnEnable() {
    // TODO: should I just have an Init function and call it from the
    // WorldState instead of using OnEnable?
    InitializeNumbers();
  }

  /// <summary>
  /// Generate the unweighted score of a a given number based on the existing
  /// entropy numbers.
  /// </summary>
  /// <param name="number">The number to evaluate.</param>
  /// <returns>The unweighted score for the number.</returns>
  public float Score(int number) {
    if (number == 0) {
      return 0f;
    } else if (veryGoodNumbers.Contains(number)) {
      return 5f;
    } else if (goodNumbers.Contains(number)) {
      return 2f;
    } else if (veryBadNumbers.Contains(number)) {
      return -10f;
    } else {
      return -1f;
    }
  }

  /// <summary>
  /// Creates a new, randomly generated collection of numbers for scoring the
  /// game's entropy.
  /// </summary>
  private void InitializeNumbers() {
    // TODO: make the range generic?
    int range = 15;
    List<int> numbers = new List<int>(range);
    for (int i = 1; i <= range; ++i) {
      numbers.Add(i);
    }

    // Find our good numbers
    this.goodNumbers = new List<int>(3);
    int rangeEnd = numbers.Count;
    int rangeStart = range * 8 / 15;
    int idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.goodNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    rangeEnd = rangeStart;
    rangeStart = range * 3 / 15;
    idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.goodNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    rangeEnd = rangeStart;
    rangeStart = 0;
    idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.goodNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    // Find our very good numbers
    this.veryGoodNumbers = new List<int>(2);
    rangeEnd = numbers.Count;
    rangeStart = numbers.Count * 2 / 5;
    idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.veryGoodNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    rangeEnd = rangeStart;
    rangeStart = 0;
    idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.veryGoodNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    // Find our very bad numbers
    this.veryBadNumbers = new List<int>(1);
    rangeEnd = numbers.Count;
    rangeStart = 0;
    idx = StaticRandom.Range(rangeStart, rangeEnd);
    this.veryBadNumbers.Add(numbers[idx]);
    numbers.RemoveAt(idx);

    PrintNumbers("Very good: {0}", this.veryGoodNumbers);
    PrintNumbers("Good: {0}", this.goodNumbers);
    PrintNumbers("Very bad: {0}", this.veryBadNumbers);
  }

  /// <summary>
  /// Debug method for printing a list of numbers.
  /// </summary>
  /// <param name="fmt">The format string.</param>
  /// <param name="l">The list to be printed.</param>
  private static void PrintNumbers(string fmt, List<int> l) {
    Debug.LogFormat(fmt, string.Join(", ", l));
  }
}