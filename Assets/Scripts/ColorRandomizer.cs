using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour {
    
    public Color[] colors;
    public Renderer[] affectedRenderers;
    
	void Start ()
    {
        Color selectedColor = colors[Random.Range(0, colors.Length)];
        foreach (Renderer renderer in affectedRenderers)
        {
            renderer.material = new Material(renderer.material);    // Make sure to have a private copy of the material
            renderer.material.color = selectedColor;
        }
	}
}
