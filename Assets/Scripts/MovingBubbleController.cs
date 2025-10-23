using UnityEngine;

// 类 变量 函数

// 类 class
// 变量 函数

public class MovingBubbleController : MonoBehaviour
{
    private int value;
    private float value2;
    private string value3;

    private bool canMove = true;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector2.left);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector2.right);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.transform.tag == "topWall")
        {
            canMove = false;
        }
    }
}
