using UnityEngine;

[CreateAssetMenu(menuName="FafaTools/Variables/Int")]
public class FafaInt : FafaVariable {

	public int value;

	public override object getVariable(){
		return value;
	}
	public override object getConstVariable(){
		return value;
	}
	public override void setVariable(object newValue){
		value = (int)newValue;
	}
}
