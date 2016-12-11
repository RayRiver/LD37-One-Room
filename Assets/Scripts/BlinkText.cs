using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    void Start()
    {
        var text = GetComponent<Text>();
        if (text != null)
        {
            StartCoroutine(StartBlink(text));
        }
    }

    IEnumerator StartBlink(Text text)
    {
        var waitForOneSecond = new WaitForSeconds(1);
        var waitForEndOfFrame = new WaitForEndOfFrame();

        yield return waitForOneSecond;

        while (true)
        {
            float deltaPerFrame = 0.016f;

            // fade out;
            while (true)
            {
                var c = text.color;
                var a = c.a - deltaPerFrame;
                a = Mathf.Clamp01(a);
                text.color = new Color(c.r, c.g, c.b, a);
                if (a <= 0)
                {
                    break;
                }
                yield return waitForEndOfFrame;
            }

            // fade in;
            while (true)
            {
                var c = text.color;
                var a = c.a + deltaPerFrame;
                a = Mathf.Clamp01(a);
                text.color = new Color(c.r, c.g, c.b, a);
                if (a >= 1)
                {
                    break;
                }
                yield return waitForEndOfFrame;
            }

            yield return waitForOneSecond;
        }
    }
}
