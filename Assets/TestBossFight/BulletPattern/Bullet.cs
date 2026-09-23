using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletTimer;
    public float BulletSpeed;
    public Rigidbody2D rb;
    public enum PatternType {Straight, Sine, Circular};
    public PatternType TypeOfPattern;

    public float time;

    [Header("Sine Wave Settings")]
    public float sineSpeed = 2f; 
    public float sineAmplitude = 2f;

    [Header("Circular Wave Settings")]
    public float radius = 2f;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, BulletTimer);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        switch (TypeOfPattern)
        {
            case PatternType.Straight:
                StraightPattern();
                break;
            case PatternType.Sine:
                SinePattern();
                break;
            case PatternType.Circular: 
                CircularPattern(time);
                break;
        }
    }
    void StraightPattern()
    {
        rb.velocity = transform.up * BulletSpeed;
    }
    void SinePattern()
    {
        Vector2 MoveForward = transform.up * BulletSpeed;
        Vector2 MoveRightLeft = transform.right * Mathf.Sin(Time.time * sineSpeed) * sineAmplitude;

        rb.velocity = MoveForward + MoveRightLeft;
    }
    void CircularPattern(float time)
    {
        Vector2 MoveUp = transform.up * Mathf.Cos(time * sineSpeed) * radius * time;
        Vector2 MoveRight = transform.right * Mathf.Sin(time * sineSpeed) * radius * time;

        rb.velocity = (MoveUp + MoveRight) * BulletSpeed;
    }
}
