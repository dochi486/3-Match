using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Food1 = 0,
    Food2,
    Food3,
    Food4,
    Food5,
    Food6,
}

public class Food : MonoBehaviour
{
    public FoodType Foodtype { get; set; }
    int index;
    public int Index { get; set; }
    Rigidbody rigid;
    bool isMoving;
    public bool IsMoving => isMoving;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, -100f, 0, ForceMode.Force);
        Foodtype = (FoodType)Random.Range(0, System.Enum.GetNames(typeof(FoodType)).Length);
        transform.name = Foodtype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Foodtype.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameManager.instance.pressedFood = transform;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.instance.releasedFood = transform;
        }
    }
}
