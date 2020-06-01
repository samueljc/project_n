using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Numbers", menuName="Scriptable Objects/Numbers")]
public class Numbers : ScriptableObject {
  private List<int> veryGoodNumbers;
  private List<int> goodNumbers;
  private List<int> veryBadNumbers;

  public void OnEnable() {
    // TODO: should I just have an Init function and call it from the
    // WorldState instead of using OnEnable?
    InitializeNumbers();
  }

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

  private static void PrintNumbers(string fmt, List<int> l) {
    Debug.LogFormat(fmt, string.Join(", ", l));
  }
}