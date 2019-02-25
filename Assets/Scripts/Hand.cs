using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Hand : MonoBehaviour
{
    private IBlockView currentBlockView;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

	        RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentBlockView = hit.collider.gameObject.GetComponent<IBlockView>();
            }
	    }

	    if (Input.GetMouseButtonUp(0))
	    {
	        currentBlockView?.TryToConnect();

            currentBlockView = null;
	    }

	    if (currentBlockView != null)
	    {
            var plane = new Plane(Vector3.forward, Vector3.zero);
	        float distance;
	        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out distance))
            {
                currentBlockView.Position = ray.GetPoint(distance);
            }
	    }
	}
}
