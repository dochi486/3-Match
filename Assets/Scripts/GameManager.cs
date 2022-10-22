using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GameObject foodGo;
    Transform foodParent;
    readonly string foodGoString = "Food";
    readonly string touchEffectString = "TouchEffect";
    GameObject touchEffectGo;
    List<List<GameObject>> foodList;
    List<Food> destroyAnimals = new List<Food>();
    int row = 8;
    int column = 6;
    float foodScaleX;
    float xGap;
    float yGap;

    public Transform pressedFood;
    public Transform releasedFood;

    GameObject touchedEffect;

    bool firstTouch;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        foodParent = GameObject.Find("FoodParent").transform;
        foodGo = (GameObject)Resources.Load(foodGoString);
        touchEffectGo = (GameObject)Resources.Load(touchEffectString);

        foodScaleX = foodGo.transform.localScale.x;
        var boxColliderSize = foodGo.GetComponent<BoxCollider>().size;
        xGap = boxColliderSize.x * foodScaleX + 0.01f;
        yGap = boxColliderSize.y + 0.01f;
        GenerateAnimals();

        yield return new WaitForSeconds(1);

    }

    private void Update()
    {
        Drag();
    }

    void GenerateAnimals()
    {
        foodList = new List<List<GameObject>>(column);
        for (int x = 0; x < column; x++)
        {
            foodList.Add(new List<GameObject>(row));

            for (int y = 0; y < row; y++)
            {
                foodList[x].Add(CreateAnimal(x, x , y ));
            }
        }
    }

    GameObject CreateAnimal(int x, float posX, float posY)
    {
        var pos = new Vector3(posX, posY);
        var newGo = Instantiate(foodGo, pos, Quaternion.identity, foodParent);
        newGo.GetComponent<Food>().Index = x;
        return newGo;
    }

    void SwipeFood(Transform food1, Transform food2)
    {
        var food1Index = food1.GetComponent<Food>().Index;
        var food2Index = food2.GetComponent<Food>().Index;
        int food1Y = foodList[food1Index].IndexOf(food1.gameObject);
        int food2Y = foodList[food2Index].IndexOf(food2.gameObject);

        var temp = foodList[food1Index][food1Y];
        foodList[food1Index][food1Y] = foodList[food2Index][food2Y];
        foodList[food2Index][food2Y] = temp;
        GetFoodInfo(food1Index, food1Y).Index = food1Index;
        GetFoodInfo(food2Index, food2Y).Index = food2Index;
    }

    Food GetFoodInfo(int x, int y)
    {
        if(foodList.Count > x && foodList[x].Count > y)
        {
            return foodList[x][y].GetComponent<Food>();
        }

        return null;
    }

    void Drag()
    {
        if (Input.GetMouseButton(0))
        {
            if (pressedFood)
            {
                touchedEffect = Instantiate(touchEffectGo, pressedFood.position, Quaternion.identity);
            }
        }
    }

}
