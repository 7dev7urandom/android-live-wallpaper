using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class BallGenerator : MonoBehaviour
{
    public Rigidbody ballPrefab;
    public Transform dropPosition;
    public int numberOfBalls = 5;
    public float timeToDropBalls = 5f;
    public float pushPower = 10f;
    public float randomVariation = 2f;
    public Transform platform;
    public Image image;
    private Rigidbody rb;
    private float timeBetweenBalls;
    private float timeElapsed = 0f;
    private int ballsDropped = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenBalls = timeToDropBalls / numberOfBalls;
        Input.gyro.enabled = true;

        string path = Application.persistentDataPath + "/backImage.jpg";
        Debug.Log(path);
        if(File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log(bytes);
            Debug.Log(texture);
            Debug.Log(sprite);
            image.sprite = sprite;
        } else
        {
            Debug.Log("path doesn't exist");
        }

        rb = platform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        int ballIndex = (int) Math.Floor(timeElapsed / timeBetweenBalls);
        if(ballIndex > ballsDropped && ballIndex <= numberOfBalls)
        {
            GenerateBall();
        }

        platform.rotation = Quaternion.Lerp(platform.rotation, Quaternion.Euler(Mathf.Clamp(Input.gyro.gravity.y * 90, -35f, 35f), 0f, Mathf.Clamp(Input.gyro.gravity.x * -90, -35f, 35f)), 0.5f);
        //rb.AddForce((Input.acceleration - Input.gyro.gravity) * accelerationConstant);
    }
    public void GenerateBall()
    {
        Rigidbody obj = Instantiate(ballPrefab, dropPosition);
        obj.velocity = UnityEngine.Random.insideUnitSphere * randomVariation + dropPosition.transform.forward * pushPower;
        ballsDropped++;
    }
}
