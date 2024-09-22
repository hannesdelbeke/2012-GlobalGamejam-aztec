using UnityEngine;
using System.Collections;
using System.Threading;
using Aniballs.Sifteo;

// http://answers.unity3d.com/questions/47732/about-multiple-thread.html

public class SifteoThread
{

	public SifteoThread()
	{
	}
	
	
	public int count = 0;
	public GameControls app = null;
	
	public void run()
	{
		// start the sifteo main loop
		
		Debug.Log("Creating the SifteoApp (1/3)");
		app = new GameControls();
		Debug.Log("SifteoApp created... about to actually call run() (2/3)");
		app.Run();
		Debug.Log("SifteoApp.Run() returned (3/3)");
	}
}

public class SifteoThreadComponent : MonoBehaviour 
{
	SifteoThread thread = null;
	
	void Start () 
	{	
		thread = new SifteoThread();
		
		Thread newThrd = new Thread(new ThreadStart(thread.run));
		
		newThrd.Start();
	}
	
	void Update () 
	{
		if( thread.app == null )
			return;
		
		// here you can just use thread.app.globalVar
		
		
	}
	
	void OnGUI()
	{
		if( thread.app == null )
			return;
		
	}

}
