using UnityEngine;
using System.Collections;

public class Snap : MonoBehaviour {
	
	public Vector3 snap = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
	
	public void SnapPosition () {

		if (Application.isPlaying) {
			return;
		}

		if (snap.x == 0 || snap.y == 0 || snap.z == 0) {
			return;
		}

		Vector3 pos = transform.position;
		pos.x = Mathf.Round(pos.x / snap.x) * snap.x;
		pos.y = Mathf.Round(pos.y / snap.y) * snap.y;
		pos.z = Mathf.Round(pos.z / snap.z) * snap.z;
		transform.position = pos + offset;
	}
}
