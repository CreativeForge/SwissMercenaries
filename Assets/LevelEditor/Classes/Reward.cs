using UnityEngine;
using System.Collections;

[System.Serializable]
public class Reward {

		public string rewardName = ""; //  name

		public string rewardTitle = ""; //  title

		public string rewardAbstract = "";

		public bool rewarding = false; // rewarding = true / punishing = false

		public string rewardType = "gold" ; // gold|singleton

		// type: gold / score

		// gold / prices ...
		public int goldMinPrice = 10; 

		// type: artefacts
		public int artefactCounted = 0;

		// visuals 
		public Texture2D textureObj; // 2d texture
		public GameObject gameObj; // 3d object > for the own garden!
		
}


