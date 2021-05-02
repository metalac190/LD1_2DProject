using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] private float _alphaSet = 0.8f;

    private float _timeActivated;
    private float _alpha;
    private float _alphaMultiplier = 0.85f;

    private PlayerMovement _player;
    private Transform _playerSpriteTransform;

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _playerSpriteRenderer;

    private Color _playerColor;
    private Color _newColor;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = FindObjectOfType<PlayerMovement>();

        _playerSpriteRenderer = _player.SpriteRenderer;
        _playerSpriteTransform = _playerSpriteRenderer.transform;
        _playerColor = _playerSpriteRenderer.color;
        // start new sprite sequence
        _alpha = _alphaSet;
        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        transform.position = _playerSpriteTransform.position;
        transform.rotation = _playerSpriteTransform.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        ProgressAnimation();
    }

    private void ProgressAnimation()
    {
        _alpha *= _alphaMultiplier;
        _newColor = new Color(_playerColor.r, _playerColor.g, _playerColor.b, _alpha);
        _spriteRenderer.color = _newColor;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
