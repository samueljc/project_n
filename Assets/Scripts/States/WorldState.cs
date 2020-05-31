using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldState : MonoBehaviour {
  private List<int> veryGoodNumbers;
  private List<int> goodNumbers;
  private List<int> badNumbers;
  private List<int> veryBadNumbers;

  [HideInInspector]
  public StoreState storeState;

  public void Awake() {
    DontDestroyOnLoad(this.gameObject);
    InitializeNumbers();
    this.storeState = new StoreState();
  }

  public void Start() {
    this.Repopulate();
  }

  public void Repopulate() {
    this.storeState.Repopulate();
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

    // Remaining numbers are bad
    this.badNumbers = numbers;

    PrintNumbers("Very good: {0}", this.veryGoodNumbers);
    PrintNumbers("Good: {0}", this.goodNumbers);
    PrintNumbers("Bad: {0}", this.badNumbers);
    PrintNumbers("Very bad: {0}", this.veryBadNumbers);
  }

  private static void PrintNumbers(string fmt, List<int> l) {
    Debug.LogFormat(fmt, string.Join(", ", l));
  }
}
