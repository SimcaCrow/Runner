using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilBlockController : MonoBehaviour
{
    private float xPosition;
    private bool playerBendDown;

    /** 
     *  Initialization
     */
    void Start()
    {
        xPosition = transform.position.x;
    }

    /** 
     *  Update is called once per frame. Move the block from left to right
     */
    void Update()
    {
        transform.position = new Vector2(xPosition + Mathf.Sin(Time.time), transform.position.y);
        CheckBendDown();
    }

    /** 
     *  Check is the player is bend down. Modify the hitbox of the block if he is
     */
    void CheckBendDown()
    {
        playerBendDown = Input.GetKey(KeyCode.S);
        if (playerBendDown)
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 0.5f);
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        }
    }
}
