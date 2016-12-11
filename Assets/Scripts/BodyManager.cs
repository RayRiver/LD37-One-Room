using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    void Start()
    {
        Messenger.AddListener<BattleUnit>("CreateBody", OnCreateBody);
    }

    void OnCreateBody(BattleUnit unit)
    {
        var prefab = unit.GetBodyPrefab();

        if (prefab != null)
        {
            var go = Instantiate(prefab);
            go.transform.parent = transform;
            go.transform.position = unit.transform.position;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))); // random rotation;

            var fadeTime = unit.GetBodyFadeTime(); 
            if (fadeTime > 0)
            {
                var renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    StartCoroutine(BodyFade(renderer, fadeTime));
                }
            }
        }
    }

    IEnumerator BodyFade(Renderer renderer, float time)
    {
        yield return new WaitForSeconds(time);

        var waitForEndOfFrame = new WaitForEndOfFrame();
        float opacity = 1;
        while (true)
        {
            opacity -= 0.02f;

            var color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, opacity);
            if (renderer.material.color.a <= 0)
            {
                break;
            }
            yield return waitForEndOfFrame;
        }

        Destroy(renderer.gameObject);
    }
}
