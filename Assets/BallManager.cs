using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float accelerationConstant = 50f;
    private BallGenerator generator;
    private Rigidbody rb;
    public Texture2D[] emojiImages;
    public GameObject exploder;
    private int emojiIndex = 0;
    private Color ballColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        generator = Camera.main.GetComponent<BallGenerator>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = Random.Range(minScale, maxScale);
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if(PlayerPrefs.GetString("useEmojis") == "true" )
        {
            ballColor = Random.ColorHSV(0f, 1f, .8f, 1f, .6f, .8f);
            mr.material.color = ballColor;
        } else
        {
            emojiIndex = Random.Range(0, emojiImages.Length);
            mr.material.SetTexture("_MainTex", emojiImages[emojiIndex]);
        }
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

    public void Explode()
    {
        var gObj = Instantiate(exploder, transform.position, transform.rotation);
        bool emojis = PlayerPrefs.GetString("useEmojis") == "true";
        foreach (Transform t in gObj.transform)
        {
            if(true)
                t.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", emojiImages[emojiIndex]);
            else
                t.GetComponent<MeshRenderer>().material.color = ballColor;
        }
        gObj.SetActive(true);
        gameObject.SetActive(false);
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        // Wait 2 seconds and die
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        generator.GenerateBall();
    }
}
