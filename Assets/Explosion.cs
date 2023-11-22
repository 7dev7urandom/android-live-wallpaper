using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float minForce = 125;
    public float maxForce = 500;
    public float radius = 10;
    public float upMod = 0.5f;
    public void Explode()
    {
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius, upMod);
            }
            StartCoroutine(Die(Random.Range(0.5f, 1.6f), t.gameObject));
        }
        Die(2f, gameObject);
    }

    public void Start()
    {
        Explode();
    }
    private IEnumerator Die(float waitTime, GameObject obj)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }
}
