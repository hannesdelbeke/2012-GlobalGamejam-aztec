using UnityEngine;
using System;
using System.Collections;
using Sifteo;

public class SifteoApp : Sifteo.BaseApp 
{
	// how to connect:
	// 1. press play button in unity
	// 2. press play button in SiftDev (after loading a generic sifteo template app)
	// 3. ...
	// 4. Profit!
	
	override public void Setup () 
	{
		Debug.Log("SifteoApp SETUP entered");
	}
	
	override public void Tick () 
	{
		//Debug.Log("Sifteo tick");
	}
}