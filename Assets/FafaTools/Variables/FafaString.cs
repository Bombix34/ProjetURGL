using UnityEngine;

[CreateAssetMenu(menuName="FafaTools/Variables/String")]
public class FafaString : FafaVariable {

	public string value;

	public override object getVariable(){
		return value;
	}
	public override object getConstVariable(){
		return value;
	}
	public override void setVariable(object newValue){
		value = (string)newValue;
	}
}
