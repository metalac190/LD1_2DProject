using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SavePoint : TriggerVolume
{
    [SerializeField]
    private Transform _newSpawnPoint;

    private GameSession _gameSession;

    private void Awake()
    {
        _gameSession = GameSession.Instance;
    }

    public override void TriggerEntered(Collider2D collider)
    {
        Debug.Log("Set new spawn point");
        // if we're not in the layer, return
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            _gameSession.SpawnLocation = _newSpawnPoint.position;
        }
    }


}
