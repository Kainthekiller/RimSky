using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames.FarmAnimals;

namespace LylekGames.FarmAnimals {
	public class NextCharacter : MonoBehaviour {

		public GameObject[] myCharacters;
		private static int i = 0;

		public void Next() {
			i += 1;
			if (i >= myCharacters.Length) {
				i = 0;
			}
			foreach (GameObject character in myCharacters) {
				character.SetActive (false);
			}
			myCharacters [i].SetActive (true);
		}
		public void Previous() {
			i -= 1;
			if (i < 0) {
				i = myCharacters.Length - 1;
			}
			foreach (GameObject character in myCharacters) {
				character.SetActive (false);
			}
			myCharacters [i].SetActive (true);
		}
	}
}
