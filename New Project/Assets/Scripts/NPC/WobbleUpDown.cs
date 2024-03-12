using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleUpDown : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float magniute;
    private float startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition.y;
    }

    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, SineAmount() + startPosition);
    }

    public float SineAmount()
    {
        return magniute * Mathf.Sin(Time.fixedTime * speed);
    }
}
