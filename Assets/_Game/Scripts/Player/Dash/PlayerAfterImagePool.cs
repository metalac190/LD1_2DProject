using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private PlayerAfterImage _afterImagePrefab;
    [SerializeField] private int _poolStartSize = 10;

    private Queue<PlayerAfterImage> _availableObjects = new Queue<PlayerAfterImage>();
    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool(_poolStartSize);
    }

    public PlayerAfterImage PlaceAfterImage(Player player)
    {
        PlayerAfterImage afterImage = GetFromPool();
        afterImage.Initialize(player);

        return afterImage;
    }

    private void GrowPool(int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            PlayerAfterImage instanceToAdd = Instantiate(_afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(PlayerAfterImage instance)
    {
        instance.gameObject.SetActive(false);
        instance.transform.position = Vector3.zero;
        instance.transform.SetParent(this.transform);
        _availableObjects.Enqueue(instance);
    }

    public PlayerAfterImage GetFromPool()
    {
        if(_availableObjects.Count == 0)
        {
            GrowPool(1);
        }

        var instance = _availableObjects.Dequeue();
        instance.gameObject.SetActive(true);

        return instance;
    }
}
