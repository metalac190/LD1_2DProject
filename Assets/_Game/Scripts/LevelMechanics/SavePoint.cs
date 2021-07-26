using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SavePoint : MonoBehaviour
{
    [SerializeField]
    private Transform _newSpawnPoint;

    private Collider2D _collider;
    private GameSession _gameSession;

    private void Awake()
    {
        // ensure it's marked as trigger
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _gameSession = GameSession.Instance;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("Set new spawn point");
        // if we're not in the layer, return
        Player player = otherCollider.GetComponent<Player>();
        if(player != null)
        {
            _gameSession.SpawnLocation = _newSpawnPoint.position;
        }
    }
}
