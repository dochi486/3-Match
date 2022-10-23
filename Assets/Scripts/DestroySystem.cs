using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySystem
{
    List<Food> destroyFoods;
    Action<int> onDestroyFood;

    public void Initialize(List<Food> destroyFoods, Action<int> onDestroyFood)
    {
        this.destroyFoods = destroyFoods;
        this.onDestroyFood = onDestroyFood;
    }

    public void AddToDestroy(Food first, Food second, Food third)
    {
        _AddToDestroyFood(first, second, third);
    }

    void _AddToDestroyFood(params Food[] foods)
    {
        foreach (var item in foods)
        {
            if (destroyFoods.Contains(item) == false)
                destroyFoods.Add(item);
        }
    }

    public void DestroyFoods()
    {
        var count = destroyFoods.Count;
        if(count > 0)
        {
            onDestroyFood?.Invoke(count);
            destroyFoods.ForEach((x) => x.DestroyFood());
            destroyFoods.Clear();
        }
    }
}
