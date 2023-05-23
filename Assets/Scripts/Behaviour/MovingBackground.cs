using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MovingBackground : MonoBehaviour
{
    [SerializeField] [Range(-1f, 1f)] private float _x = 0.5f;
    [SerializeField] [Range(-1f, 1f)] private float _y = 0f;

    private float save_x, save_y;

    void Start()
    {
        save_x = _x;
        save_y = _y;
    }
    
    void Update()
    {
        float offsetX = Time.time * _x;
        float offsetY = Time.time * _y;
        GetComponent<Renderer>().material.mainTextureOffset = new(offsetX, offsetY);
    }

    public void IncreaseSpeed() {
        _x *= 1.5f;
        _y *= 1.5f;
    }

    public void Reset() {
        _x = save_x;
        _y = save_y;
    }
}