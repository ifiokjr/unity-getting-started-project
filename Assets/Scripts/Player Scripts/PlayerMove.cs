using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	private Animator animator;
	[SerializeField]
	private CharacterController characterController;
	private CollisionFlags collisionFlags = CollisionFlags.None;

	private const float MoveSpeed = 5f;
	private bool canMove = false;

	private bool finishedMovement = true;
	private Vector3 targetPosition = Vector3.zero;
	private Vector3 playerMove = Vector3.zero;

	private float playerToPointDistance;

	private const float Gravity = 9.8f;
	private float height;

	private void Awake()
	{
		animator = GetComponent<Animator> ();
		characterController = GetComponent<CharacterController> ();
	}
	
	void Update () {
		CalculateHeight();
//		MoveThePlayer();
//		characterController.Move(playerMove);
		CheckIfFinishedMovement();
	}

	private bool IsGrounded()
	{
		return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
	}

	private void CalculateHeight()
	{
		if (IsGrounded())
		{
			height = 0f;
		}
		else
		{
			height -= Gravity * Mathf.Pow(Time.deltaTime, 2f);
		}
	}

	private void CheckIfFinishedMovement()
	{
		if (!finishedMovement)
		{
			if (!animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stand") &&
			    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
			{
				finishedMovement = true;
			}
		}
		else
		{
			MoveThePlayer();
			playerMove.y = height;
			collisionFlags = characterController.Move(playerMove);
		}
	}

	private void MoveThePlayer()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Point in screen pixels" + Input.mousePosition);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider is TerrainCollider)
				{
					playerToPointDistance = Vector3.Distance(transform.position, hit.point);
					Debug.Log("Point in world" + hit.point);
					if (playerToPointDistance >= 1.0f)
					{
						canMove = true;
						targetPosition = hit.point;
					}
				}
			}
		}
		if (canMove)
		{
			animator.SetFloat("Walk", 1.0f);
			Vector3 tempTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(tempTargetPosition - transform.position), 15.0f * Time.deltaTime);

			playerMove = transform.forward * MoveSpeed * Time.deltaTime;

			if (Vector3.Distance(transform.position, targetPosition) <= 0.5)
			{
				canMove = false;
			}
		}
		else
		{
			playerMove.Set(0f, 0f, 0f);
			animator.SetFloat("Walk", 0f);
		}
	}
}
