using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {

	bool isShowingShop = false;

	public List<GameObject> itemPrefabs;
	List<ShopItemScript> itemsShowed = new List<ShopItemScript>();

	public GameObject shopCanvasGO;
	public Text[] titleTexts;
	public Text[] subTexts;
	public Image[] itemImages;

	// Use this for initialization
	void Awake () {
		shopCanvasGO.SetActive(false);
		shopCanvasGO.transform.SetParent(null);
	}
	
	// Update is called once per frame
	void Update () {
		if(IsShowingShop){
			if(Input.GetKeyDown(KeyCode.A)){
				ConsumeSelectItem(0);
				
			}else if(Input.GetKeyDown(KeyCode.B)){
				ConsumeSelectItem(1);

			}else if(Input.GetKeyDown(KeyCode.X)){ // exit shop
				IsShowingShop = false;

			}else if(Input.GetKeyDown(KeyCode.Y)){
				ConsumeSelectItem(2);
			}
		}
	}

	public void PlayerEnteredTrigger(){
		IsShowingShop = true;
	}

	bool IsShowingShop{
		get{
			return isShowingShop;
		}
		set{
			isShowingShop = value;
			shopCanvasGO.SetActive(isShowingShop);
			InGameController.i.InShop(value);
		
			if(isShowingShop){
				Set3RandomItems();

			}
		}
	}

	void Set3RandomItems(){
		itemsShowed = new List<ShopItemScript>();
		List<GameObject> tempItemsLeft = new List<GameObject>(itemPrefabs);
		//Debug.Log("tempItemsLeft count: "+tempItemsLeft.Count);
		//Debug.Log("itemPrefabs count: "+itemPrefabs.Count);
		for(int i=0; i<3; i++){
			int rand = Random.Range(0, tempItemsLeft.Count);
			//Debug.Log("rand: "+rand);
			GameObject tItemPrfab = tempItemsLeft[rand];

			GameObject tItemGO = Instantiate(tItemPrfab) as GameObject;
			ShopItemScript tItemS = tItemGO.GetComponent<ShopItemScript>();
			//Debug.Log("item: "+tItemS);
			itemsShowed.Add(tItemS);
			titleTexts[i].text = tItemS.itemName + " ("+ tItemS.price+" S.)";
			subTexts[i].text = tItemS.description;
			itemImages[i].sprite = tItemS.image;

			tempItemsLeft.Remove(tItemPrfab);
		}



	}
	void ConsumeSelectItem(int inIndex){
		IsShowingShop = false;

		ShopItemScript tchosenItem = itemsShowed[inIndex];

		if(InGameController.i.playerS.Money<tchosenItem.price){
			InGameController.i.ShowInGameMessage("You dont have enough money to buy this.\n(Press M to cheat and earn 100 Schilling)", true, 2);
			return;
		}
		InGameController.i.playerS.Money -= Mathf.RoundToInt( tchosenItem.price);
		//Debug.Log("chose: "+inIndex+" item: "+tchosenItem);
		ShopItemCaseScript tCase = tchosenItem.Consume();
		//Debug.Log("chosn case: "+tCase);

		ShowConsumeInfos(tchosenItem.itemName, tCase);

		InGameController.i.playerS.dS.Health += tCase.healthChange;
		InGameController.i.playerS.Money +=  Mathf.RoundToInt(tCase.moneyChange);
		InGameController.i.playerS.Faith += tCase.faithChange;

	}

	void ShowConsumeInfos(string inChosenItemName, ShopItemCaseScript inCase){
		InGameController.i.ShowInGameMessage(""+inChosenItemName.ToUpper()+"\n"+inCase.description, true, 5);
	}
}
