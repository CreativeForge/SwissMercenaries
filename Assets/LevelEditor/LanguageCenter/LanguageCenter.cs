using UnityEngine;
using System.Collections;

namespace GameLab.LanguageCenter
{

	public enum LanguageType {
		EN,
		DE,
		DECH
	}

public class LanguageCenter : MonoBehaviour {

		// language elements
		ArrayList arrLanguageElements = new ArrayList () ;

		void Start( ) {

			/*
			AddLanguageElement( LanguageType.EN, "menu.claim", "THE HACKNSLASH FOR ALL OF US!" );
			AddLanguageElement( LanguageType.EN, "menu.start", "PLAY" );
			
			AddLanguageElement( LanguageType.DE, "menu.claim", "GEMETZEL FUER UNS ALLE!" );
			AddLanguageElement( LanguageType.DE, "menu.start", "START" );

			// Some Tests
			Debug.Log ("GetLanguage('menu.claim'): "+GetLanguage (LanguageType.EN, "menu.claim"));

			*/



		}

		// "abc" or
		void Text( string textOrAt ) {


		}


		// load all entries for the following language
		// (from json file)
		public void LoadLanguage ( LanguageElement lanElement ) {

			// load a language set
			// EN/DE/DECH


		}

		public void AddLanguageElement( LanguageType lanType, string key, string translation ) {

			LanguageElement le = new LanguageElement ();
			le.languageType = lanType;
			le.key = key; 
			le.translation = translation;
			arrLanguageElements.Add (le);
		}


				public LanguageElement GetLanguageElement(  LanguageType languageType, string key ) {

					// Debug.Log ("GetLanguageElement(  "+ languageType.ToString () +", "+ key+" )");

					for (int i=0; i<arrLanguageElements.Count; i++) {
						LanguageElement lanElement = (LanguageElement) arrLanguageElements[i];
						if ( lanElement.languageType == languageType ) {
							// Debug.Log (i+". ("+lanElement.languageType.ToString ()+"/"+lanElement.key.ToLower ()+") "+lanElement.translation);
							if (lanElement.key.ToLower ().Equals ( key.ToLower () )) {
								return lanElement;
							}
						}
					}

					return null;

				}

			public string GetLanguage( LanguageType languageType, string key ) {

				LanguageElement lanElement = GetLanguageElement ( languageType, key );

				if (lanElement == null) {
					Debug.LogError ("GetLanguage( "+languageType+", "+ key +" ) NOT FOUND");
					return ""; // return null
				} else {
					return lanElement.translation;
				}

			}


		public string GetLanguageOr(LanguageType languageType, string key, string alternative ) {

			LanguageElement lanElement = GetLanguageElement ( languageType, key );
			
			if (lanElement == null) {
				return alternative; // return null
			} else {
				return lanElement.translation;
			}
			
		}


	}

}

