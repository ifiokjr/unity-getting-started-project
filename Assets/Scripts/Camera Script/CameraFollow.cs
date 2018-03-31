using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private float followHeight = 8f;
	[SerializeField] private float followDistance = 6f;

	private Transform player;
	private float targetHeight;
	private float currentHeight;
	private float currentRotation;
	
	// This is the first to be called
	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// This is called second
	private void OnEnable()
	{
			
	}

	// Use this for initialization
	//  This is called third
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		targetHeight = player.position.y + followHeight;
		currentRotation = transform.eulerAngles.y;

		currentHeight = Mathf.Lerp(transform.position.y, targetHeight, 0.9f * Time.deltaTime);
		Quaternion euler = Quaternion.Euler(0f, currentRotation, 0f);

		Vector3 targetPosition = player.position - (euler * Vector3.forward) * followDistance;

		targetPosition.y = currentHeight;

		transform.position = targetPosition;
		transform.LookAt(player);
	}
}
