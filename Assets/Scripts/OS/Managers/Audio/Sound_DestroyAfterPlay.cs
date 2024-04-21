using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_DestroyAfterPlay : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterPlay());
    }

    IEnumerator DestroyAfterPlay()
    {
        yield return new WaitForSeconds(this.gameObject.GetComponent<AudioSource>().clip.length);
        Destroy(this.gameObject);
    }
}
