using UnityEngine;

[CreateAssetMenu(menuName="FafaTools/Variables/Float")]
public class FafaFloat : FafaVariable {

	public float value;

	public override object getVariable(){
		if(resetAtStart){
			return localValue;
		}	else	{
			return value;
		}
	}

	public override object getConstVariable(){
		return value;
	}

	public override void setVariable(object newValue){
		if(resetAtStart){
			localValue = newValue;
		}	else	{
			value = (float)newValue;
		}
	}
}
