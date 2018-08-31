using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSawController : MonoBehaviour
{
    private bool playerBendDown;

    /** 
     *  Update is called once per frame. Rotate the sprite of the saw
     */
    void Update()
    {
        transform.Rotate(Vector3.forward * +2f);
        CheckBendDown();
    }

    /** 
     *  Check is the player is bend down. Modify the hitbox of the saw if he is
     */
    void CheckBendDown()
    {
        playerBendDown = Input.GetKey(KeyCode.S);
        if (playerBendDown)
        {
            gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
        }
        else
        {
            gameObject.GetComponent<CircleCollider2D>().radius = 0.5f;
        }
    }
}
