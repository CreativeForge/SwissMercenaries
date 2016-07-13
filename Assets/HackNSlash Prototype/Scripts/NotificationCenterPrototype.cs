using UnityEngine;
using System.Collections;

public class NotificationCenterPrototype : MonoBehaviour {

	public GameObject FloatingCombatTextPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void UpdateHealthEnemy(float inValue, Vector3 inPos){

		if(inValue<0)
			ShowCombatText(""+inValue,inPos,CombatText.TextType.EnemyDamage);
		else
			ShowCombatText("+"+inValue,inPos,CombatText.TextType.EnemyHeal);	
	}
	public void UpdateHealthPlayer(float inValue, Vector3 inPos){

		if(inValue<0)
			ShowCombatText(""+inValue,inPos,CombatText.TextType.Damage);
		else
			ShowCombatText("+"+inValue,inPos,CombatText.TextType.Heal);	
	}

	public void ShowCombatText(string value, Vector3 position, CombatText.TextType textType){
		Vector3 screenPos = Camera.main.WorldToScreenPoint (position + new Vector3 (FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetX,
			FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetY,
			FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetZ));
		//screenPos = Camera.main.WorldToScreenPoint(position);
		screenPos.x = screenPos.x / Screen.width;
		screenPos.y = screenPos.y / Screen.height;

		/* this is useless
			// some randomness
			screenPos.x = screenPos.x ; // + UnityEngine.Random.Range(-10,10);
			screenPos.y = screenPos.y ; // + UnityEngine.Random.Range(0,20);
      */

		//screenPos.x =screenPos.x/ Screen.width +0.5f;
		//screenPos.y =screenPos.y/ Screen.height+0.5f;
		GameObject temp;
		CombatText combatText;
		temp = Instantiate (FloatingCombatTextPrefab, screenPos, Quaternion.identity) as GameObject;
		combatText = temp.gameObject.GetComponent<CombatText> ();
		combatText.ShowText (textType,value);
	}
}
