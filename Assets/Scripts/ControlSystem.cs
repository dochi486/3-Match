using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlSystem
{
    int row = 8;
    int column = 6;
    Vector2 _foodGap;
    public Vector2 FoodGap => _foodGap;
    public bool IsSwiping => moveSystem.IsSwping;

    MatchSystem matchSystem = new MatchSystem();
    [SerializeField] MoveSystem moveSystem = new MoveSystem();
    [SerializeField] GenerateSystsem generateSystsem = new GenerateSystsem();
    DestroySystem destroySystem = new DestroySystem();

    List<List<GameObject>> foodList = new List<List<GameObject>>();
    List<Food> destroyFoodList = new List<Food>();
    public int DestroyFoodCount => destroyFoodList.Count;


    public void Initialize(GameManager gameManager, Action<int> onDestroy)
    {
        moveSystem.Initialize(GetFoodInfo, IsMatched);
        generateSystsem.Initialize(out _foodGap.x, out _foodGap.y, row, column, GetFoodInfo);
        destroySystem.Initialize(destroyFoodList, onDestroy);
        matchSystem.Initialize(row, column, GetFoodInfo, destroySystem.AddToDestroy);
    }

    public void GenerateFoods()
    {
        generateSystsem.GenerateFoods(foodList);
    }

    public void IsMatchAndDestroy()
    {
        matchSystem.IsMatchAndDestroy();
        destroySystem.DestroyFoods();
    }

    public void OnCompleteDestroy(GameObject food, int index)
    {
        generateSystsem.OnCompleteDestroy(foodList, food, index);
    }

    Food GetFoodInfo(int x, int y)
    {
        if (foodList.Count > x && foodList[x].Count > y)
        {
            return foodList[x][y].GetComponent<Food>();
        }

        return null;
    }

    public bool IsMatched()
    {
        return matchSystem.IsMatchedHorizontal(MatchMode.Check) 
            || matchSystem.IsMatchedVertical(MatchMode.Check);
    }

    public bool IsMoving()
    {
        return moveSystem.IsMoving(row, column);
    }

    public void SwipingFood(Transform food1, Transform food2)
    {
        moveSystem.SwitchingFood(foodList, food1, food2);
    }
}
