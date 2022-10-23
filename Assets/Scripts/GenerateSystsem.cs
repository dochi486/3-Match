using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerateSystsem 
{
    float xGap;
    float yGap;
    [SerializeField] public Transform foodParent;
    [SerializeField] Food foodPrefab;
    float foodScaleX;
    int row;
    int column;
    Func<int, int, Food> getFood;

    public void Initialize(out float xGap, out float yGap, int row, int column, Func<int,int,Food> getFood)
    {
        foodScaleX = foodPrefab.transform.localScale.x;
        var boxColliderSize = foodPrefab.GetComponent<BoxCollider>().size;
        xGap = boxColliderSize.x * foodScaleX + 0.01f;
        yGap = boxColliderSize.y + 0.01f;

        this.xGap = xGap;
        this.yGap = yGap;
        this.row = row;
        this.column = column;
        this.getFood = getFood;
    }

    public void GenerateFoods(List<List<GameObject>> foodList)
    {
        foodList.Clear() ;
        for (int x = 0; x < column; x++)
        {
            foodList.Add(new List<GameObject>(row));

            for (int y = 0; y < row; y++)
            {
                foodList[x].Add(CreateAnimal(x, x * xGap, y ));
            }
        }
    }

    GameObject CreateAnimal(int x, float posX, float posY)
    {
        var pos = new Vector3(posX, posY);
        var newGo = GameObject.Instantiate(foodPrefab, pos, Quaternion.identity, foodParent);
        newGo.GetComponent<Food>().Index = x;
        return newGo.gameObject;
    }

    public void OnCompleteDestroy(List<List<GameObject>> foodList, GameObject food, int index)
    {
        foodList[index].Remove(food);
        Reborn(foodList, index);
    }

    void Reborn(List<List<GameObject>> foodList, int index)
    {
        var newY = getFood?.Invoke(index, foodList[index].Count - 1).transform.position.y;
        foodList[index].Add(CreateAnimal(index, index * xGap, (float)newY + (yGap * 2)));
    }

}
