using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vars : MonoBehaviour {

	private bool idle = true;
	private bool onjourney = false;
	private bool infight = false;
	private bool cuttingwood = false;
	private bool ismining = false;
	private bool dead = false;

	public bool Idle {
		get{ return this.idle; }
		set{ 
			this.idle = value; 
		}
	}

	public bool onJourney {
		get{ return this.onjourney; }
		set{ 
			this.onjourney = value; 
			if (value) {
				this.Idle = false;
			} else {
				this.Idle = true;

			}
		}
	}

	public bool inFight {
		get{ return this.infight; }
		set{ this.infight = value; }
	}

	public bool cuttingWood {
		get{ return this.cuttingwood; }
		set{ this.cuttingwood = value; }
	}

	public bool isMining {
		get{ return this.ismining; }
		set{ this.ismining = value; }
	}

	public bool Dead {
		get{ return this.dead; }
		set{ 
			this.dead = value;
			if (value) {
				this.onJourney = false;
				this.inFight = false;
			}
		}
	}
}
