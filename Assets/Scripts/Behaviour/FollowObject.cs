using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _offset;

    void LateUpdate()
    {
        transform.position = _target.transform.position + _offset;
    }
}
