using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using static System.Single;

public class ObjectPool : MonoBehaviour
{
    public Action OnFieldsFilled;
    [SerializeField] private Cube _cube;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private TMP_InputField[] _inputFields;

    private ObjectPool<Cube> _cubePool;

    private float _speed;
    private float _objectPerSecond;
    private float _lastSpawnedTime;
    private float _delay = 200f;
    public bool _isSpeedFilled { get; private set; } = false;
    public bool _isDelayFilled { get; private set; } = false;
    public bool _isDirectionFilled { get; private set; } = false;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false,
            200, 100_000);
    }

    private void Update()
    {
        if (!_isSpeedFilled || !_isDelayFilled || !_isDirectionFilled) return;
        OnFieldsFilled?.Invoke();
        var delay = 1f / _objectPerSecond;
        if (!(_lastSpawnedTime + delay < Time.time)) return;
        var cubesSpawnTimeInFrame = Mathf.CeilToInt(Time.deltaTime / _delay);
        while (cubesSpawnTimeInFrame > 0)
        {
            _cubePool.Get();
            cubesSpawnTimeInFrame--;
        }
        _lastSpawnedTime = Time.time;
    }
    
    public void IsDelayFilled()
    {
        if (!TryParse(_inputFields[0].text, out var parsedValue)) return;
        parsedValue = Parse(_inputFields[0].text);
        _objectPerSecond = parsedValue;
        _isDelayFilled = true;
    }

    public void IsSpeedFilled()
    {
        if (!TryParse(_inputFields[1].text, out var parsedValue)) return;
        parsedValue = Parse(_inputFields[1].text);
        _speed = parsedValue;
        _isSpeedFilled = true;

    }

    public void IsDirectionFilled()
    {
        if (!TryParse(_inputFields[2].text, out var parsedValue)) return;
        parsedValue = Parse(_inputFields[2].text);
        _endPoint.position = new Vector3(parsedValue, 0, 0);
        _isDirectionFilled = true;
    }
    
    private Cube CreatePooledObject()
    {
        var instance = Instantiate(_cube, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObjectToPool;
        instance.gameObject.SetActive(false);
        return instance;
    }
    
    private void ReturnObjectToPool(Cube instance)
    {
        _cubePool.Release(instance);
    }

    private void OnReturnToPool(Cube instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Cube instance)
    {
        instance.gameObject.SetActive(true);
        SpawnCube(instance);
        instance.transform.SetParent(transform, true);
    }

    private void OnDestroyObject(Cube instance)
    {
        Destroy(instance);
    }

    private void SpawnCube(Cube instance)
    {
        instance.transform.position = _spawnPoint.position;
        instance.MoveCube(_spawnPoint.position, _endPoint.position, _speed);
    }
}
