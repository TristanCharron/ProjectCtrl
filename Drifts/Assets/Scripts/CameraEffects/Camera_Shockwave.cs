using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shockwave : MonoBehaviour {
	[SerializeField]Shader shockwaveShader;
	// Use this for initialization
	void Start () 
	{
		Camera.main.SetReplacementShader(shockwaveShader, "RenderType");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
