using UnityEngine;
using System.Collections;
using System;

public class PlayerStorage {

	GameObject playerInstance;

	float health = 100f;
	float faith = 0f;
	int money = 0;
	string weapon = "sword";
	int levelMastered = 0;

	Hashtable attributes;

	// Constructor(s)
	public PlayerStorage(GameObject playerInstance) {

		this.attributes = new Hashtable();
	}

	public PlayerStorage(GameObject playerInstance, float health, float faith, int money) {

		this.health = health;
		this.faith = faith;
		this.money = money;
		this.playerInstance = playerInstance;
		this.attributes = new Hashtable();

	}

	public PlayerStorage(GameObject playerInstance, int levelMastered) {

		this.levelMastered = levelMastered;
		this.playerInstance = playerInstance;
		this.attributes = new Hashtable();

	}

	public PlayerStorage(GameObject playerInstance, float health, float faith, int money, string weapon, int levelMastered) {

		this.health = health;
		this.faith = faith;
		this.money = money;
		this.weapon = weapon;
		this.levelMastered = levelMastered;
		this.playerInstance = playerInstance;
		this.attributes = new Hashtable();

	}

	public void AddAttribute(string attrName, object attrObject) {

		attributes.Add(attrName, attrObject);

	}

	public T GetAttribute<T>(string attrName) {

		if(attributes.ContainsKey(attrName)) {

			try {

				// Return attribute
				return (T)attributes[attrName];

			} catch(InvalidCastException e) {

				Debug.LogError("[PlayerStorage] Cannot access attribute. Invalid type definition!");
				return default(T);
			
			}

		}

		Debug.LogError("[PlayerStorage] Attribute does not exist!");
		return default(T);

	}

	public bool AttributeExists(string attrName) {

		if(attributes.ContainsKey(attrName)) {

			return true;

		}

		return false;

	}

	// Player instance
	public GameObject Player {

		set { playerInstance = value; }

	}

	// Properties
	public float Health {

		get { return health; }

		set { health = Mathf.Clamp(value, 0f, 100f); }

	}

	public float Faith {
		
		get { return faith; }

		set { faith = Mathf.Clamp(value, 0f, 100f); }

	}

	public int Money {

		get { return money; }

		set { money = value;}

	}

}
