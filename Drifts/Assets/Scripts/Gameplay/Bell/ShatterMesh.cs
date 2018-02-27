using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShatterMesh : MonoBehaviour {

	public enum ShatterDirection{
		ImpactDirection,
		RandomDirection,
		AwayFromCenter
	}

	[SerializeField]Rigidbody[] meshs;

	[SerializeField]float verticalForce = 1;
	[SerializeField]float randomDirForce = 2;
	[SerializeField]float impactDirForce = 15;

	[SerializeField]float angularSpeed;
	[SerializeField]float delayDelete = 2;

	//You plug the mesh renderer and you apply a color to the shared material
	[SerializeField]MeshRenderer meshR;



	public void ColorMesh(Color color)
	{
		meshR.sharedMaterial.SetColor("_Color",color);
	}

	public void Shatter (Vector3 directionShatter) 
	{
		foreach(Rigidbody rb in meshs)
		{
			Vector3 dir = rb.position - transform.position;
       
			//rb.AddForce(Random.onUnitSphere * randomDirForce + Vector3.up * verticalForce,ForceMode.VelocityChange);
			rb.AddForce(directionShatter * impactDirForce + dir * randomDirForce + Vector3.up * verticalForce,ForceMode.VelocityChange);

			
			rb.angularVelocity = Random.onUnitSphere * angularSpeed;
			Destroy(rb.gameObject,delayDelete);
		}
		Destroy(gameObject,delayDelete);
	}
	public void Shatter () 
	{
		Shatter(Vector3.zero);
	}
}
