using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCoinController : MonoBehaviour
{
    private float yPosition;

    /** 
     *  Initialization
     */
    void Start()
    {
        yPosition = transform.position.y;
    }

    /** 
     *  Update is called once per frame. Move the coin up and down
     */
    void Update()
    {
        transform.position = new Vector2(transform.position.x, yPosition + Mathf.Sin(Time.time * 2) / 12);
    }
}
