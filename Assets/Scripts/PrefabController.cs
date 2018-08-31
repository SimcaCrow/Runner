using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TypeOfPlatform { leftArm, rightArm, jump, bendDown, neutral };

public class PrefabController : MonoBehaviour
{
    public TypeOfPlatform myType;
    public int width;
    public int height;
    public int InitialSpawnWeight;
    private int weight;
    public int Weight
    {
        get { return weight; }
        set { weight = value; }
    }
    public GameObject[] possibleNeighbours;
    private GameObject destructionPoint;

    /** 
     *  Initialization
     */
    void Start()
    {
        Weight = InitialSpawnWeight;
        destructionPoint = GameObject.Find("DestructionPoint");
        possibleNeighbours = possibleNeighbours.OrderBy(i => i.GetComponent<PrefabController>().InitialSpawnWeight).ToArray();
    }

    /** 
     *  Update is called once per frame. Destroy the prefab when it is behind the destruction point
     */
    void Update()
    {
        if (transform.position.x < destructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
