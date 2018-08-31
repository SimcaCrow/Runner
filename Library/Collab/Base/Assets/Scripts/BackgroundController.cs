using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public float backgroundSpeed;

	private Vector2 startPosition;

	// Initialization
	void Start () {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float newPosition = Mathf.Repeat(Time.time * backgroundSpeed, 19.2f);
        transform.position = new Vector2(Camera.main.gameObject.transform.position.x, transform.position.y);
        transform.position = (Vector2)transform.position + Vector2.left * newPosition;
    }
}
