using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopItemScript : MonoBehaviour {

	public string itemName;
	public string description;
	public Sprite image;
	List<ShopItemCaseScript> cases = new List<ShopItemCaseScript>();
	public float chance;
	public float price;

	// Use this for initialization
	void Start () {
		foreach(Transform tT in transform){
			ShopItemCaseScript iCS = tT.GetComponent<ShopItemCaseScript>();
			Debug.Log("iCS: "+iCS);
			cases.Add(iCS);
		}
		Debug.Log("cases.count: "+cases.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float TotalChance(){
		float totalChance = 0;
		foreach(ShopItemCaseScript iC in cases){
			totalChance += iC.chance;
		}
		return totalChance;
	}

	public ShopItemCaseScript Consume(){
		float rand = Random.Range(0, TotalChance());
		ShopItemCaseScript chosenCase = null;
		float counter = 0;
		foreach(ShopItemCaseScript iC in cases){
			float counterNew = counter + iC.chance;
			Debug.Log("iC: "+iC+" counterNew: "+counterNew);
			if(counter<rand && rand<counterNew){
				chosenCase = iC;

				Debug.Log("before break: "+chosenCase);
				break;
			}else{
				counter = counterNew;
			}
		}
		Debug.Log("ChosenCase: "+chosenCase+" title:"+chosenCase.caseName);
		Destroy(gameObject);
		return chosenCase;
	}
}
