using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientBase : ScriptableObject
{
    public Sprite image;
    public GameObject objectPrefab;
    
    [Header("Stats")]
    public float bitterValue = 0;
    public float sweetValue = 0;
    public float sourValue = 0;
    public float aromaValue = 0;
    public float alcoholValue = 0;
}
