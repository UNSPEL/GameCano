using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstaclePool;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private float _outsideDelayTime = 2f;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _minXRadius = 1f;
    [SerializeField] private float _maxXRadius = 2f;
    [SerializeField] private float _outsideMinXRadius = 7f;
    [SerializeField] private float _outsideMaxXRadius = 20f;
    [SerializeField] private float _obstacleSpeed = 7f;

    private float save_delayTime, save_outsideDelayTime, save_obstacleSpeed;

    
    void Start()
    {
        save_obstacleSpeed = _obstacleSpeed;
        save_outsideDelayTime = _outsideDelayTime;
        save_delayTime = _delayTime;

        foreach (var obs in _obstaclePool) {
            obs.GetComponent<ObstacleBehaviour>()?.SetSpeed(_obstacleSpeed);
        }

        StartCoroutine(Spawner());
        StartCoroutine(LeftSpawner());
        StartCoroutine(RightSpawner());
    }

    public void IncreaseSpeed() {
        _delayTime -= 0.2f;
        _outsideDelayTime -= 0.2f;
        _obstacleSpeed += 1f;
        foreach (var obs in _obstaclePool) {
            obs.GetComponent<ObstacleBehaviour>()?.SetSpeed(_obstacleSpeed);
        }
    }

    public void Restart() {
        _delayTime = save_delayTime;
        _outsideDelayTime = save_outsideDelayTime;
        _obstacleSpeed = save_obstacleSpeed;

        foreach (var obs in _obstaclePool) {
            obs.SetActive(false);
        }
    }

    private IEnumerator Spawner() {
        while(true) {
            if (GameSystem.Instance.IsGameRunning) {
                int index = Random.Range(0, _obstaclePool.Length);
                GameObject obj = _obstaclePool[index];
                if (obj.activeInHierarchy) {
                    obj = ReChoose(index+1);
                }

                if (obj != null) {
                    obj.SetActive(true);
                    float x = Random.Range(_spawnPoint.position.x + _minXRadius, _spawnPoint.position.x + _maxXRadius);
                    
                    float direction = Random.Range(0, 2);
                    if (direction == 0) x = -x;

                    obj.transform.position = new(x, _spawnPoint.position.y, _spawnPoint.position.z);
                }
            }
            yield return new WaitForSeconds(_delayTime);
        }
    }

    private IEnumerator LeftSpawner() {
        while(true) {
            if (GameSystem.Instance.IsGameRunning) {
                int index = Random.Range(0, _obstaclePool.Length);
                GameObject obj = _obstaclePool[index];
                if (obj.activeInHierarchy) {
                    obj = ReChoose(index+1);
                }

                if (obj != null) {
                    obj.SetActive(true);
                    float x = Random.Range(_spawnPoint.position.x - _outsideMaxXRadius, _spawnPoint.position.x - _outsideMinXRadius);
                    obj.transform.position = new(x, _spawnPoint.position.y, _spawnPoint.position.z);
                }
            }
            yield return new WaitForSeconds(_outsideDelayTime);
        }
    }

    private IEnumerator RightSpawner() {
        while(true) {
            if (GameSystem.Instance.IsGameRunning) {
                int index = Random.Range(0, _obstaclePool.Length);
                GameObject obj = _obstaclePool[index];
                if (obj.activeInHierarchy) {
                    obj = ReChoose(index+1);
                }

                if (obj != null) {
                    obj.SetActive(true);
                    float x = Random.Range(_spawnPoint.position.x + _outsideMinXRadius, _spawnPoint.position.x + _outsideMaxXRadius);
                    obj.transform.position = new(x, _spawnPoint.position.y, _spawnPoint.position.z);
                }
            }
            yield return new WaitForSeconds(_outsideDelayTime);
        }
    }

    private GameObject ReChoose(int startIndex) {
        for (int i = startIndex; i < _obstaclePool.Length; i++) {
            if (!_obstaclePool[i].activeInHierarchy) {
                return _obstaclePool[i];
            }
        }

        for (int i = 0; i < startIndex; i++) {
            if (!_obstaclePool[i].activeInHierarchy) {
                return _obstaclePool[i];
            }
        }

        return null;
    }
}
