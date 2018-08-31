using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour
{
    public int InitialSpawnWeight;
    private GameObject destructionPoint;

    /** 
     *  Initialization
     */
    void Start()
    {
        destructionPoint = GameObject.Find("DestructionPoint");
    }

    /** 
     *  Update is called once per frame. Destroy the element when it is behind the destruction point
     */
    void Update()
    {
        if (transform.position.x < destructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}