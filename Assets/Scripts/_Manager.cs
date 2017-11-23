using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class _Manager : MonoBehaviour {
	static long time;

	public void ChangeScene (string scene) {
		if(scene == "Level 1")
			time = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
		else if (scene == "End") {
			time = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond - time;
			//time /= 1000;
		}

		SceneManager.LoadScene(scene);
	}

	private void OnGUI() {
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.color = Color.black;

		GUI.Label(new Rect(0, 0, 200, 200), "Time: " + time.ToString());
	}
}
