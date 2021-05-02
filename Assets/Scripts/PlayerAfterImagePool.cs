using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject _afterImagePrefab;
    [SerializeField] private int _poolStartSize = 10;

    private Queue<GameObject> _availableObjects = new Queue<GameObject>();
    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool(_poolStartSize);
    }

    private void GrowPool(int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            var instanceToAdd = Instantiate(_afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        _availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(_availableObjects.Count == 0)
        {
            GrowPool(1);
        }

        var instance = _availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
