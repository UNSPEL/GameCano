using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _feetPos;
    [SerializeField] private float _zDisable = -15f;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _speedUp = 0.05f;
    [SerializeField] private bool _isOnStart;

    public void SetSpeed(float speed) {
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSystem.Instance.IsGameRunning) {
            return;
        }

        if (_isOnStart) {
            MoveUp();
        } else {
            MoveToPlayer();
        }

        if (transform.position.z <= _zDisable) {
            gameObject.SetActive(false);
        }
    }

    void MoveUp() {
        if (!CheckAbove()) {
            transform.position = new(transform.position.x, transform.position.y + _speedUp * Time.fixedDeltaTime, transform.position.z);
        }
    }

    void MoveToPlayer() {
        transform.position = new(transform.position.x, transform.position.y, transform.position.z - _speed * Time.fixedDeltaTime);
    }

    private bool CheckAbove() {
        if (Mathf.Abs(_feetPos.position.y - GameSystem.Instance.Surface.transform.position.y) <= 0.1f) {
            _isOnStart = false;
        }
        return !_isOnStart;
    }

    void OnEnable()
    {
        _isOnStart = true;
    }
}
