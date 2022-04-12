using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroyer : MonoBehaviour
{
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float accelerationConstant = 50f;
    private BallGenerator generator;
    private Rigidbody rb;
    public GameObject roof;
    // Start is called before the first frame update
    void Start()
    {
        generator = Camera.main.GetComponent<BallGenerator>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = Random.Range(minScale, maxScale);
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.material.color = Random.ColorHSV(0f, 1f, .8f, 1f, .6f, .8f);
        transform.localScale *= rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10f)
        {
            generator.GenerateBall();
            Destroy(gameObject);
        }
        //Vector3 vec = (Input.acceleration - Input.gyro.gravity);
        Vector3 vec = Input.gyro.userAcceleration;
        rb.AddForce(new Vector3(vec.x, 0, vec.y) * accelerationConstant, ForceMode.Acceleration);
    }

}
