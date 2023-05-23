using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static Action<GameObject> OnPlayerDied;

    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;
    [SerializeField] private float _swapTime = 0.7f;
    [SerializeField, Range(10f, 45f)] private float _angleRotate = 10f;
    public bool CanSwap { get; private set; }

    private Transform _currentPos;

    void Start()
    {
        _currentPos = _left;
        StartCoroutine(Swap(_left, _swapTime));
    }

    void Update()
    {
        if (GameSystem.Instance.IsGameRunning) {
            if (MyInputAction.MoveBindings || (MyInputAction.Move == 1f) == (_currentPos == _left) && MyInputAction.Move != 0f) {
                SwapPosition();
            }
        }
    }


    public void SwapPosition() {
        if (!CanSwap) {
            return;
        }

        if (_currentPos == _left) {
            _currentPos = _right;
        } else {
            _currentPos = _left;
        }

        StartCoroutine(Swap(_currentPos, _swapTime));
    }

    private IEnumerator Swap(Transform target, float duration) {
        CanSwap = false;

        float timer = 0f;

        float angle = (target.position.x > transform.position.x) ? _angleRotate : -_angleRotate;

        // StartCoroutine(Rotates(angle, duration / 4f, false));

        while (timer < duration && Vector3.Distance(transform.position, target.position) > 0.1f) {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target.position, timer / duration);
            yield return null;
        }

        CanSwap = true;
        // StartCoroutine(Rotates(-angle, duration / 4f, true));
    }

    private IEnumerator Rotates(float angle, float duration, bool resetSwap) {
        float timer = 0f;
        Quaternion targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + angle, transform.rotation.eulerAngles.z);
        while (timer < duration && transform.rotation.x != targetRot.x && transform.rotation.y != targetRot.y && transform.rotation.z != targetRot.z) {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, timer / duration);
            yield return null;
        }

        if (resetSwap) {
            CanSwap = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Danger")) {
            OnPlayerDied?.Invoke(this.gameObject);
        }
    }

}
