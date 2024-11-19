using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start() 
    {
        StartCoroutine(Coroutine());
        Invoke("AutoDestory", 3f);

        var a = Coroutine();
    }

    IEnumerator Coroutine()
    {
        int sec = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(++sec);
        }
    }

    void AutoDestory()
    {
        Destroy(gameObject);
    }
}
