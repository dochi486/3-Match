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
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, -100f, 0, ForceMode.Force);
        Foodtype = (FoodType)Random.Range(0, System.Enum.GetNames(typeof(FoodType)).Length);
        transform.name = Foodtype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Foodtype.ToString());

        StartCheckState();
    }

    void StartCheckState()
    {
        StartCoroutine(CheckStateCo());

        IEnumerator CheckStateCo()
        {
            while(isAlive)
            {
                CheckMoving();
                CheckSwiping();
                CheckUpForce();
                yield return null;
            }
        }
    }

    void CheckMoving()
    {
        isMoving = rigid.velocity.sqrMagnitude > 0.1f;
    }

    void CheckSwiping()
    {
        rigid.useGravity = !GameManager.instance.IsSwiping;
    }

    void CheckUpForce()
    {
        if(rigid.velocity.y > 0)
        {
            rigid.Sleep();
        }
    }

    public void DestroyFood()
    {
        StartCoroutine(Co());

        IEnumerator Co()
        {
            animator.Play("Destroy");

            yield return new WaitForSeconds(0.4f);

            GameManager.instance.OnCompleteDestroy(gameObject, Index);
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        GameManager.instance.OnMouseDown(transform);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.instance.OnMouseOver(transform);
        }
    }
}
