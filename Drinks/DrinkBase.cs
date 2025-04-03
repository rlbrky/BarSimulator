using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TempratureCategory
{
    Cold,
    Hot,
}


[CreateAssetMenu(fileName = "New Drink", menuName = "Drink")]
public class DrinkBase : ScriptableObject
{
    public Sprite image;
    public GameObject objectPrefab;
    
    [Header("Stats")]
    public float bitterValue = 0;
    public float sweetValue = 0;
    public float sourValue = 0;
    public float aromaValue = 0;
    public float alcoholValue = 0;
    public float requiredShakeTime = 0;

    [Header("Order Related")] 
    public TempratureCategory category;
}