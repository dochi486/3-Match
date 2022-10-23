using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInputSystem
{
    Func<bool> isMoving;
    Func<Vector2> getGap;
    Action<Transform, Transform> swipingFood;
    Func<bool> isSwiping;

    [SerializeField] GameObject touchEffectPrefab;
    string layerName = "Food";
    LayerMask foodLayer;
    GameObject touchedEffect;
    Transform pressedFood;
    Transform releasedFood;


    bool firstTouch = false;
    bool isTouchable = false;

    public void Initialize(Func<bool> isMoving, Func<Vector2> getGap, Action<Transform, Transform> swipingFood, Func<bool> isSwiping)
    {
        this.isMoving = isMoving;
        this.getGap = getGap;
        this.swipingFood = swipingFood;
        this.isSwiping = isSwiping;

        foodLayer = 1 << LayerMask.NameToLayer(layerName);
    }

    public void OnStartPlayGame()
    {
        isTouchable = true;
    }

    private float GetMaxDistance()
    {
        return Mathf.Max((float)(getGap?.Invoke().x), (float)(getGap?.Invoke().y)) + 0.01f;
    }

    public IEnumerator WaitUserInput()
    {
        isTouchable = true;
        yield return new WaitForSeconds(1);
        isTouchable = false;
    }

    public void Swipe()
    {
        if (Input.GetMouseButton(0))
        {
            if (pressedFood)
            {
                if (firstTouch)
                {
                    touchedEffect = GameObject.Instantiate(touchEffectPrefab, pressedFood.position, Quaternion.identity);
                    firstTouch = false;
                }
            }
        }
        else if (pressedFood != null && releasedFood != null &&
            pressedFood != releasedFood
            && Vector3.Distance(pressedFood.position, releasedFood.position) <= GetMaxDistance()
            && isSwiping?.Invoke() == false)
        {
            firstTouch = true;
            swipingFood?.Invoke(pressedFood, releasedFood);
        }
        else
        {
            ClearTouchInfo();
        }
    }


    public void ClearTouchInfo()
    {
        firstTouch = true;
        pressedFood = null;
        releasedFood = null;
        GameObject.Destroy(touchedEffect);
    }

    public void OnMouseDown(Transform targetFood)
    {
        pressedFood = targetFood;
    }

    public void OnMouseOver(Transform targetFood)
    {
        releasedFood = targetFood;
    }
}
