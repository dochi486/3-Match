using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MoveSystem 
{
    public bool IsSwping => _isSwiping;
    bool _isSwiping = false;

    Func<int, int, Food> getFood;
    Func<bool> isMatched;
    [SerializeField] float tweenMoveTime = 0.3f;

    public void Initialize(Func<int,int,Food> getFood, Func<bool> isMatched)
    {
        this.getFood = getFood;
        this.isMatched = isMatched;
    }

    public bool IsMoving(int row, int column)
    {
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (getFood?.Invoke(x, y).IsMoving == true)
                    return true;
            }
        }
        return false;
    }

    public void SwitchingFood (List<List<GameObject>> foodList, Transform food1, Transform food2)
    {
        SwipeFood(foodList, food1, food2);

        var isMatching = isMatched?.Invoke();
        if(isMatching == false)
        {
            SwipeFood(foodList, food1, food2);
        }

        MoveFoodPosition(food1, food2, (bool)isMatching);
        MoveFoodPosition(food2, food1, (bool)isMatching);
    }

    void SwipeFood(List<List<GameObject>> foodList, Transform food1, Transform food2)
    {
        _isSwiping = true;
        var food1Index = food1.GetComponent<Food>().Index;
        var food2Index = food2.GetComponent<Food>().Index;
        int food1Y = foodList[food1Index].IndexOf(food1.gameObject);
        int food2Y = foodList[food2Index].IndexOf(food2.gameObject);

        var temp = foodList[food1Index][food1Y];
        foodList[food1Index][food1Y] = foodList[food2Index][food2Y];
        foodList[food2Index][food2Y] = temp;
        getFood.Invoke(food1Index, food1Y).Index = food1Index;
        getFood.Invoke(food2Index, food2Y).Index = food2Index;
    }

    void MoveFoodPosition(Transform transform, Transform target, bool isMatched)
    {
        transform.DOMove(target.position, tweenMoveTime)
            .SetLoops(isMatched == true ? 1:2, LoopType.Yoyo)
            .SetEase(Ease.OutBounce)
            .SetLink(transform.gameObject)
            .OnComplete(() => _isSwiping = false);
    }
}
