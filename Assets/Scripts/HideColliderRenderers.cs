using UnityEngine;
using System.Collections;

public class HideColliderRenderers : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//return;
		
		foreach( Transform child in this.transform )
		{
			child.GetComponent<MeshRenderer>().enabled = false;
			
			Vector3 scale = child.transform.localScale;
			scale.z = 20;
			child.transform.localScale = scale;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
