using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "New Upgrade", fileName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    [TextArea] public string description;
    public int cost;
}
