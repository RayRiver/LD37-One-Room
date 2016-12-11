using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance {
		get {

			if (!_instance) {
				_instance = (T)FindObjectOfType(typeof(T));
			}

			return _instance;
		}
	}

	protected static T _instance;	
}