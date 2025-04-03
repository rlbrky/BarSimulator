using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksManager : MonoBehaviour
{
    [Header("Settings")] 
    public float shakeErrorMargin = 3f; // 3 seconds default error margin for shaking.
    public float overallErrorMargin = 3f; // 3 points for each stats error margin.
    public float aromaErrorMargin = 100f;
    
    [Header("References")] 
    private DrinkBase _currentDrink;
    
    // Required stuff
    private float _minShakeTime = 0f;
    private float _maxShakeTime = 0f;
    
    [Header("Overall Values")]
    private float _bitterValue = 0;
    private float _sweetValue = 0;
    private float _sourValue = 0;
    private float _aromaValue = 0;
    private float _alcoholValue = 0;

    // Controls
    private float _currentShakeTimer = 0f;
    
    public void Shake()
    {
        _currentShakeTimer += Time.deltaTime;
        if (_currentShakeTimer >= _minShakeTime && _currentShakeTimer <= _maxShakeTime)
        {
            CompleteDrink();
            // Optionally reset the shake timer if needed:
            _currentShakeTimer = 0f;
        }
    }

    public void StartDrink(DrinkBase drink)
    {
        _currentDrink = drink;
        _minShakeTime = drink.requiredShakeTime - shakeErrorMargin;
        _maxShakeTime = drink.requiredShakeTime + shakeErrorMargin;
    }

    public void AddIngredient(IngredientBase ingredient)
    {
        _bitterValue += ingredient.bitterValue;
        _sweetValue += ingredient.sweetValue;
        _sourValue += ingredient.sourValue;
        _aromaValue += ingredient.aromaValue;
        _alcoholValue += ingredient.alcoholValue;
    }

    private bool IsWithinMargin(float value, float target, float margin)
    {
        return value >= target - margin && value <= target + margin;
    }

    private void CompleteDrink()
    {
        if (_currentDrink != null 
            && IsWithinMargin(_bitterValue, _currentDrink.bitterValue, overallErrorMargin)
            && IsWithinMargin(_sweetValue, _currentDrink.sweetValue, overallErrorMargin)
            && IsWithinMargin(_sourValue, _currentDrink.sourValue, overallErrorMargin)
            && IsWithinMargin(_alcoholValue, _currentDrink.alcoholValue, overallErrorMargin)
            && IsWithinMargin(_aromaValue, _currentDrink.aromaValue, aromaErrorMargin))
        {
            // Reset drink values
            _bitterValue = 0;
            _sweetValue = 0;
            _sourValue = 0;
            _aromaValue = 0;
            _alcoholValue = 0;
            
            // Spawn result
            Instantiate(_currentDrink.objectPrefab, PlayerController.instance.holdPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("You failed to craft the drink you wanted to.");
        }
    }
}