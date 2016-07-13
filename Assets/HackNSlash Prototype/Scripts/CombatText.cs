using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {


	static int positionCounter = 0;

    // player
    public Color DamageColor = new Color(70, 119, 6); // Color.red;
    public Color HealColor = new Color(70, 19, 6); // Color.green;
    public Color MoneyColor = new Color(228, 182, 51); // Color.yellow;
	public Color FaithColor = new Color(91, 2, 16); // Color.white;
	public Color FightingSpiritColor = new Color(255, 0, 255); // Color.magenta;
    public Color RandomStuffColor = new Color(91, 20, 34); // Color.blue;
    public Color SpokenColor = new Color(91, 2, 16); // Color.white;


    public Color NPCDamageColor = new Color(70, 119, 6);

    public Color EnemyDamageColor = new Color(10, 15, 10);
    public float OffsetY = 2.0f;
    public float OffsetX = 0.0f;
    public float OffsetZ = 0.0f;

    public float displayTime = 2.0f;
    public float OffsetGainedX = 0.0f;
    public float OffsetGainedY = 2.0f;

    public GUIText displayedText;

    private float startTime;
    ////GameLogic gamelogic;

    public enum TextType {
        // Friends 
        Damage,
        Heal,
        Money,
		Faith,
		FightingSpirit,
        RandomStuff,
        // NPCs (Friend)
        NPCDamage,
        // Enemies
        EnemyDamage,
        EnemyHeal, // not used
        EnemyMoney, // not used
        EnemyFaith, // not used
        EnemyRandomStuff, // not used
        Spoken
    }

    void Awake() {
        ////gamelogic = GameObject.Find("_GameLogic").GetComponent<GameLogic>();

    }


    public void ShowText(TextType textType, string value)
    {
        ////int positionCounter = gamelogic.positionCounter;

        //displayedText = this.gameObject.GetComponent<GUIText> ();

        Color usedColor = Color.white;
		Debug.Log("show text");
        int fontSize = 24;

		displayedText.fontStyle = FontStyle.Bold;

        switch (textType)
        {

            case TextType.Damage:
                usedColor = DamageColor;
                break;

            case TextType.Faith:
                usedColor = FaithColor;
                break;

			case TextType.FightingSpirit:
				usedColor = FightingSpiritColor;
				break;
		
			case TextType.Heal:
                usedColor = HealColor;
                break;

            case TextType.Money:
                usedColor = MoneyColor;
                break;

            case TextType.RandomStuff:
                usedColor = RandomStuffColor;
                break;

            case TextType.EnemyDamage:
                fontSize = 15;
                usedColor = EnemyDamageColor;
				displayedText.fontStyle = FontStyle.Normal;
                break;

            case TextType.NPCDamage:
				fontSize = 18;
				displayedText.fontStyle = FontStyle.Normal;
                usedColor = NPCDamageColor;
                break;

            case TextType.Spoken:
				fontSize = 20;
				displayedText.fontStyle = FontStyle.Normal;
                usedColor = SpokenColor;
                break;

            default:

                break;

        }

        if (textType != TextType.Spoken)
        {
            displayedText.text = value;
            displayedText.color = usedColor;
            displayedText.fontSize = fontSize;
            startTime = Time.time;
            StartCoroutine(handleCombatText());

			/*
            if (positionCounter > 6)
            {
                this.transform.position += new Vector3(15 / Screen.width, 0, 0);
            }
            else if (positionCounter < 3)
            {
                this.transform.position -= new Vector3(15 / Screen.width, 0, 0);

            }
            if (positionCounter == 0 || positionCounter == 3 || positionCounter == 6)
            {
                this.transform.position += new Vector3(0, 10 / Screen.height, 0);
            }
            else if (positionCounter == 2 || positionCounter == 5 || positionCounter == 8)
            {
                this.transform.position -= new Vector3(0, 10 / Screen.height, 0);
            }
            positionCounter = positionCounter + 1;
            if (positionCounter > 8)
            {
                positionCounter = 0;
            }*/
			
            ////gamelogic.positionCounter = positionCounter;

        }
        else
        {
            displayedText.text = value;
            displayedText.color = usedColor;
            displayedText.fontSize = fontSize;
            startTime = Time.time;
            this.transform.position += new Vector3(0, 0, 0);
            displayTime = value.Length / 4;
            StartCoroutine(handleSpokenText(displayTime));
        }
}

	IEnumerator handleCombatText() {
		yield return new WaitForSeconds(0.05f);
		if (Time.time - startTime >= displayTime) {
			this.displayedText.text = "";
			//Destroy (displayedText);
			Destroy (this.gameObject);
		} else {
			this.transform.position += new Vector3( OffsetGainedX / Screen.width, OffsetGainedY / Screen.height, 0);
			StartCoroutine(handleCombatText());
		}
}

    IEnumerator handleSpokenText(float time)
    {
        yield return new WaitForSeconds(time);
            this.displayedText.text = "";
            //Destroy (displayedText);
            Destroy(this.gameObject);
    }

}