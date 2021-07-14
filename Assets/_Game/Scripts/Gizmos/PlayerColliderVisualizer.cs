using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderVisualizer : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _playerPrefabCollider;

    private void OnDrawGizmos()
    {
        if (_playerPrefabCollider != null)
        {
            // convert Vector2D fields to Vector3
            Vector3 boxSize = new Vector3(_playerPrefabCollider.size.x, _playerPrefabCollider.size.y);
            Vector3 colliderOffset = new Vector3(_playerPrefabCollider.offset.x, _playerPrefabCollider.offset.y);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + colliderOffset, boxSize);
        }
    }
}
