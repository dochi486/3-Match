using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MatchMode
{
    Check,
    CheckAndDestroy
}

public class GameManager : Singleton<GameManager>
{
    public static GameManager instance;
    GameObject foodGo;
    Transform foodParent;
    readonly string foodGoString = "Food";
    readonly string touchEffectString = "TouchEffect";
    GameObject touchEffectGo;
    List<List<GameObject>> foodList;
    List<Food> destroyFoods = new List<Food>();
    int row = 8;
    int column = 6;
    float foodScaleX;
    public float xGap => controlSystem.FoodGap.x;
    float yGap => controlSystem.FoodGap.y;

    [SerializeField]public ControlSystem controlSystem = new ControlSystem();
    [SerializeField]public UserInputSystem userInputSystem = new UserInputSystem();

    public Transform pressedFood;
    public Transform releasedFood;

    GameObject touchedEffect;

    bool firstTouch;

    bool m_isSwiping = false;
    public bool IsSwiping => m_isSwiping;
    bool isPlaying = false;

    private void Awake()
    {
        instance = this;
        controlSystem.Initialize(this, OnDestroyFood);
        userInputSystem.Initialize(controlSystem.IsMoving, () => controlSystem.FoodGap,
            controlSystem.SwipingFood, () => controlSystem.IsSwiping);
    }

    private void OnDestroyFood(int count)
    {
        userInputSystem.ClearTouchInfo();
    }

    IEnumerator Start()
    {
        foodParent = GameObject.Find("FoodParent").transform;
        foodGo = (GameObject)Resources.Load(foodGoString);
        touchEffectGo = (GameObject)Resources.Load(touchEffectString);


        OnStartPlayGame();

        yield return new WaitForSeconds(1);

        while (isPlaying)
        {
            while (controlSystem.IsMoving() == false)
            {
                controlSystem.IsMatchAndDestroy();

                yield return userInputSystem.WaitUserInput();
            }
            yield return null;
        }
    }

    void OnStartPlayGame()
    {
        controlSystem.GenerateFoods();
        isPlaying = true;

    }

    private void Update()
    {
        userInputSystem.Swipe();
    }

    public void OnCompleteDestroy(GameObject food, int index)
    {
        controlSystem.OnCompleteDestroy(food, index);
    }

    public void OnMouseDown(Transform targetFood)
    {
        userInputSystem.OnMouseDown(targetFood);
    }

    public void OnMouseOver(Transform targetFood)
    {
        userInputSystem.OnMouseOver(targetFood);
    }
}
