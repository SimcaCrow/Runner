using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackgroundBlockController : MonoBehaviour
{
    private GameObject destructionPoint;

    /** 
     *  Initialization
     */
    void Start()
    {
        destructionPoint = GameObject.Find("DestructionPoint");
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        if (transform.position.x < destructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}