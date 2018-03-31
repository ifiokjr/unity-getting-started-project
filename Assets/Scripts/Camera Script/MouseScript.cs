using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
	[SerializeField] private Texture2D cursorTexture;
	[SerializeField] private GameObject mousePoint;

	private CursorMode mode = CursorMode.ForceSoftware;
	private Vector2 hotSpot = Vector2.zero;

	private GameObject instantiatedMouse;
	
	// Use this for initialization
	private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		Cursor.SetCursor(cursorTexture, hotSpot, mode);

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider is TerrainCollider)
				{
					Vector3 temp = hit.point;
					temp.y = 0.25f;
					if (instantiatedMouse != null)
					{
						Destroy(instantiatedMouse);
					}
					instantiatedMouse = Instantiate(mousePoint, temp, Quaternion.identity);
				}
			}
			
		}
	}
}
