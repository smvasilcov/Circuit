using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteVisualizer : MonoBehaviour
{
    public GameObject Visualizer;

    public IBlock _block;

	// Use this for initialization
	void Start ()
	{
	    var block = GetComponent<IBlock>();

	    if (block == null)
	    {
            return;
	    }

        block.ExuteStarted += BlockOnExuteStarted;
	    block.ExecuteEnded += BlockOnExecuteEnded;
	}

    private void BlockOnExecuteEnded()
    {
        Renderer rend = Visualizer.GetComponent<Renderer>();

        var material = new Material(Shader.Find("Standard"));

        rend.material = material;
        rend.material.SetColor("_Color", Color.white);
    }

    private void BlockOnExuteStarted()
    {
        Renderer rend = Visualizer.GetComponent<Renderer>();

        var material = new Material(Shader.Find("Standard"));

        rend.material = material;
        rend.material.SetColor("_Color", Color.green);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
