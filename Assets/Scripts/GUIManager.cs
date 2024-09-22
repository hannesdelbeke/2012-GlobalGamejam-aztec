using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//ShowBalloon("TESTING", CharacterController2D.CHARACTER_STATE.RHINO, 5.0f);
		
		if( balloonSkin.label.contentOffset == Vector2.zero )
			balloonSkin.label.contentOffset = new Vector2(200, 40);
	}
	
	//public enum BALLOON_TYPE { NONE, RHINO, RABBIT, FISH, ANT }
	
	// Update is called once per frame
	void Update () 
	{
		if( showBalloon )
		{
			timeoutCounter += Time.deltaTime;
			
			if( timeoutCounter > timeoutLimit )
				HideCurrentBalloon();
		}
	}
	
	protected string text = "";
	protected CharacterController2D.CHARACTER_STATE type = CharacterController2D.CHARACTER_STATE.NONE;
	protected bool showBalloon = false;
	
	protected float timeoutCounter = 0.0f;
	protected float timeoutLimit = 0.0f;
	
	public Texture2D fishBG = null;
	public Texture2D rabbitBG = null;
	public Texture2D antBG = null;
	public Texture2D rhinoBG = null;
	
	public void HideCurrentBalloon()
	{
		showBalloon = false;
	}
	
	public void ShowBalloon(string text, CharacterController2D.CHARACTER_STATE type, float duration )
	{
		showBalloon = true;
		
		this.text = text;
		this.type = type;
		
		timeoutCounter = 0.0f;
		timeoutLimit = duration;
		
		
		Texture2D tex = null;
		
		if( type == CharacterController2D.CHARACTER_STATE.ANT )
			tex = antBG;
		else if( type == CharacterController2D.CHARACTER_STATE.FISH )
			tex = fishBG;
		else if( type == CharacterController2D.CHARACTER_STATE.RABBIT )
			tex = rabbitBG;
		else if( type == CharacterController2D.CHARACTER_STATE.RHINO )
			tex = rhinoBG;
		
		balloonSkin.label.normal.background = tex;
	}
	
	public GUISkin balloonSkin = null;
	
	public bool onTop = true; // otherwhise: show on bottom;
	
	void OnGUI()
	{
		if( !showBalloon )
			return;
		
		int height = 150;//100;
		int width = 600;//400;
		int startX = Screen.width / 2 - width/2;
		int startY = 10;
		
		if( !onTop )
			startY = Screen.height - 150 - 10;
		
		GUI.skin = balloonSkin;
		GUI.Label( new Rect(startX, startY, width, height), text );
	}
}
