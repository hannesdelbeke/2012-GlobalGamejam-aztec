using UnityEngine;
using System.Collections;

public class SimonSaysGraphics : MonoBehaviour 
{
	public GameObject transparantBG = null;
	public GameObject animalSphere = null;
	
	public SimonSaysSequence sequence = null;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if( Input.GetKeyDown(KeyCode.S) )
		{
			sequence.Length = 6;
			
			BeginMinigame(CharacterController2D.CHARACTER_STATE.RHINO);
		}
		*/
	}
	
	//public enum ANIMAL_TYPE { NONE, RHINO, ANT, FISH, RABBIT }
	
	public Texture2D left = null;
	public Texture2D right = null;
	public Texture2D bottom = null;
	public Texture2D top = null;
	public Texture2D none = null;
	
	public Texture2D rhino = null;
	public Texture2D ant = null;
	public Texture2D fish = null;
	public Texture2D rabbit = null;
	
	public Transform animalHeadPlane = null;
	
	public void BeginMinigame(CharacterController2D.CHARACTER_STATE type)
	{
		StartCoroutine( BeginMinigameRoutine(type) );
	}
	
	protected CharacterController2D.CHARACTER_STATE currentType = CharacterController2D.CHARACTER_STATE.NONE;
	
	protected void ChangeHeadGraphics(CharacterController2D.CHARACTER_STATE type)
	{
		Texture2D tex = null;
		
		if( type == CharacterController2D.CHARACTER_STATE.ANT )
			tex = ant;
		else if( type == CharacterController2D.CHARACTER_STATE.FISH )
			tex = fish;
		else if( type == CharacterController2D.CHARACTER_STATE.RABBIT )
			tex = rabbit;
		else if( type == CharacterController2D.CHARACTER_STATE.RHINO )
			tex = rhino;
		
		animalHeadPlane.GetComponent<MeshRenderer>().material.mainTexture = tex;
	}
	
	protected IEnumerator BeginMinigameRoutine(CharacterController2D.CHARACTER_STATE type)
	{
		currentType = type;
		ChangeHeadGraphics(type);
		
		audio.PlayOneShot(this.audio.clip);
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().HideCurrentBalloon();
		
		// disable the character from moving while in this minigame
		GameObject.Find("Character").GetComponent<CharacterController2D>().enabled = false;
		GameObject.Find("Character").GetComponent<Rigidbody>().velocity = Vector3.zero;
		
		this.transform.position = new Vector3(Camera.main.transform.position.x + 4, Camera.main.transform.position.y, -24);
		
		// fade in the background
		if( transparantBG != null )
			transparantBG.transform.GetComponent<MeshRenderer>().enabled = true;
		
		animalSphere.animation["FallAndBounce"].speed = 1;
		animalSphere.animation["FallAndBounce"].time = 0.0f;
		animalSphere.animation.Play("FallAndBounce");
		
		yield return new WaitForSeconds( animalSphere.animation["FallAndBounce"].length );
		
		sequence.StartNewRound();
		
		//ShowDirection("top");
		//animalSphere.transform.position = new Vector3()
		
		yield return null;
	}
	
	public void EndMinigame(bool success)
	{
		StartCoroutine(EndMinigameRoutine(success));
	}
	
	protected IEnumerator EndMinigameRoutine(bool success)
	{
		ShowDirection("none");
		
		// give the player the control over the character back
		//GameObject.Find("Character").GetComponent<Rigidbody>().enabled = true;
		
		if( success )
			GameObject.Find("Character").GetComponent<CharacterController2D>().SwitchToState(currentType);
		else
			GameObject.Find("Character").GetComponent<CharacterController2D>().StateSwitchFailed(currentType);
		
		// reverse animation to make sphere disappear
		animalSphere.animation.Stop();
		animalSphere.animation["FallAndBounce"].speed = -1.0f;
		animalSphere.animation["FallAndBounce"].time = animalSphere.animation["FallAndBounce"].length;
		animalSphere.animation.Play("FallAndBounce");
		
		yield return new WaitForSeconds( animalSphere.animation["FallAndBounce"].length );
		
		if( transparantBG != null )
			transparantBG.transform.GetComponent<MeshRenderer>().enabled = false;
		
		GameObject.Find("Character").GetComponent<CharacterController2D>().enabled = true;
		
		
		yield return null;
	}
	
	public void ShowDirection(string direction)
	{
		Material sphereMat = animalSphere.GetComponent<MeshRenderer>().material;
		
		if( direction == "left" )
			sphereMat.mainTexture = left;
		else if( direction == "right" )
			sphereMat.mainTexture = right;
		else if( direction == "down" )
			sphereMat.mainTexture = bottom;
		else if( direction == "up" )
			sphereMat.mainTexture = top;
		else
			sphereMat.mainTexture = none;
	}
}
