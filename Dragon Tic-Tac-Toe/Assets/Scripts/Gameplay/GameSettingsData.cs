using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "GameData/GameSettingsData", order = 1)]
public class GameSettingsData : ScriptableObject
{
    [Header("Value")]
    public int round = 1;
    public int time = 0;
    public int size = 3;

    [Header("Game Settings")]
    public int[] rounds = new int[4] { 5, 10, 30, 0 };
    public int[] times = new int[4] { 1, 3, 5, 7 };
    public int minSize = 3;
    public int maxSize = 9;
}
