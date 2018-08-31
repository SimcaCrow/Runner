using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    private Vector3 playerOldPosition;
    private float cameraTranslation;

    /** 
     *  Initialization
     */
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerOldPosition = player.transform.position;
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        UpdateCamera();
    }

    /** 
     *  Update the camera position. The camera follows the player
     */
    private void UpdateCamera()
    {
        cameraTranslation = player.transform.position.x - playerOldPosition.x;
        transform.position = new Vector3(transform.position.x + cameraTranslation, transform.position.y, transform.position.z);
        playerOldPosition = player.transform.position;
    }
}
