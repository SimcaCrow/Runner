using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private GameObject player;
    private float playerSpeed;
    private float backgroundSpeed;
    private float backgroundXSize;
    private float cameraStartingPosition;

    /** 
     *  Initialization
     */
    void Start()
    {
        player = GameObject.Find("Player");
        playerSpeed = player.GetComponent<PlayerController>().startSpeed;
        backgroundSpeed = playerSpeed / 2;
        backgroundXSize = GameObject.Find("3rdBackground").GetComponent<Transform>().position.x - GameObject.Find("Background").GetComponent<Transform>().position.x;
        cameraStartingPosition = Camera.main.gameObject.transform.position.x;
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        UpdateBackground();
    }

    /** 
     *  Update the level background position. 
     */
    private void UpdateBackground()
    {
        if (!player) return;
        playerSpeed = player.GetComponent<PlayerController>().PlayerSpeed;
        //The speed of the background is 2 times less than that of the player
        backgroundSpeed = playerSpeed / 2;
        float newPosition = Mathf.Repeat((Camera.main.gameObject.transform.position.x - cameraStartingPosition) * backgroundSpeed / playerSpeed, backgroundXSize);
        transform.position = new Vector2(Camera.main.gameObject.transform.position.x, transform.position.y);
        transform.position = (Vector2)transform.position + Vector2.left * newPosition;
    }
}
