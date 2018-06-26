using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

	private bool enabled = false;

	public bool Enabled
	{
		get {return enabled; }
		private set { enabled = value;}
	}

	public virtual void TurnOn()
	{
		enabled = true;

	}
	public virtual void TurnOff()
	{
		enabled = false;

	}


}
