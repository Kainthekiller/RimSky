using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LylekGames.FarmAnimals {
	public class PlayAnimationOnClick : MonoBehaviour {

		public Animation[] animator;
		public string myAnimName;

		public void PlayAnimation() {
			foreach (Animation anim in animator) {
				anim.Play (myAnimName);
				anim.wrapMode = WrapMode.Loop;
			}
		}
	}
}
