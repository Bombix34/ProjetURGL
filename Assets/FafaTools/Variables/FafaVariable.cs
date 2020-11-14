using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FafaVariable : ScriptableObject {
	public bool resetAtStart;
	[HideInInspector]
	public object localValue;
	public abstract object getVariable();
	public abstract object getConstVariable();
	public abstract void setVariable(object newValue);

	void OnEnable(){
		if(resetAtStart){}
			localValue = getConstVariable();
	}
}
