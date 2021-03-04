using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Boom());
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
