using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor (typeof(Snap))]
[CanEditMultipleObjects]

public class SnapCustomEditor : Editor {

	void OnSceneGUI() {
		if (target != null) {
			((Snap)target).SnapPosition();
		}
	}
}
