using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem 
{
    Func<int, int, Food> getFood;
    Action<Food, Food, Food> addToDestroyFoods;
    int row;
    int column;

    public void Initialize(int row, int column, Func<int, int, Food> getFood, Action<Food, Food,Food> addToDestroyFoods)
    {
        this.row = row;
        this.column = column;
        this.getFood = getFood;
        this.addToDestroyFoods = addToDestroyFoods;
    }

    public void IsMatchAndDestroy()
    {
        IsMatchedVertical(MatchMode.CheckAndDestroy);
        IsMatchedHorizontal(MatchMode.CheckAndDestroy);
    }

    public bool IsMatchedVertical(MatchMode matchMode)
    {
        Food first, second, third;

        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row - 2; y++)
            {
                first = getFood?.Invoke(x, y);
                second = getFood?.Invoke(x, y + 1);
                third = getFood?.Invoke(x, y + 2);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchMode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            addToDestroyFoods?.Invoke(first, second, third);
                            break;

                    }
                }
            }
        }

        return false;
    }

    public bool IsMatchedHorizontal(MatchMode matchMode)
    {
        Food first, second, third;

        for (int x = 0; x < column -2; x++)
        {
            for (int y = 0; y < row ; y++)
            {
                first = getFood?.Invoke(x, y);
                second = getFood?.Invoke(x + 1, y);
                third = getFood?.Invoke(x + 2, y);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchMode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            addToDestroyFoods?.Invoke(first, second, third);
                            break;
                    }
                }
            }
        }

        return false;
    }
}
