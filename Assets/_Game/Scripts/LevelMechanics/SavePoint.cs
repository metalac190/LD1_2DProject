using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SavePoint : MonoBehaviour
{
    [SerializeField]
    private Transform _newSpawnPoint;
    [SerializeField]
    private GameSessionData _gameSessionData;

    private Collider2D _collider;

    private void Awake()
    {
        // ensure it's marked as trigger
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("Set new spawn point");
        // if we're not in the layer, return
        Player player = otherCollider.GetComponent<Player>();
        if(player != null)
        {
            _gameSessionData.SpawnLocation = _newSpawnPoint.position;
        }
    }
}
