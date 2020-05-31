using UnityEngine;

public static class StaticRandom {
  public static int Range(int min, int max) {
    return Random.Range(min, max);
  }

  public static float Range(float min, float max) {
    return Random.Range(min, max);
  }
}