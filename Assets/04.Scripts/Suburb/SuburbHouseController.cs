using System;
using UnityEngine;

/// <summary>
/// A controller for a single house in the suburb.
/// </summary>
class SuburbHouseController : MonoBehaviour {
  /// <summary>
  /// The state of this house.
  /// </summary>
  [SerializeField]
  private SuburbHouseState state;
}