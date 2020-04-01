using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    private float lifetime = 3;
    private Material mat;
    private Color originalColor;
    private float fadePercent;
    private float deathTime;
    private bool fading;
    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + lifetime;
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        while(true)
        {
            yield return new WaitForSeconds(.2f);
            if(fading)
            {
                fadePercent += Time.deltaTime;
                mat.color = Color.Lerp(originalColor, Color.clear, fadePercent);
                
                if (fadePercent >= 1)
                {
                    Destroy(gameObject);
                }
            }
            else

            if (Time.time > deathTime)
            {
                fading = true;
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Ground")
        {
            GetComponent<Rigidbody>().Sleep();
        }
    }
}
