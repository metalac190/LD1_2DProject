using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] private float _alphaSet = 0.8f;

    private float _timeActivated;
    private float _alpha;
    [SerializeField]
    private float _alphaDecay = 10f;

    private PlayerMovement_Old _player;
    private Transform _playerSpriteTransform;

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _playerSpriteRenderer;

    private Color _playerColor;
    private Color _newColor;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = FindObjectOfType<PlayerMovement_Old>();

        _playerSpriteRenderer = _player.SpriteRenderer;
        _playerSpriteTransform = _playerSpriteRenderer.transform;
        _playerColor = _playerSpriteRenderer.color;
        transform.localScale = _playerSpriteTransform.localScale;
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
        _alpha -= _alphaDecay * Time.deltaTime;
        _newColor = new Color(_playerColor.r, _playerColor.g, _playerColor.b, _alpha);
        _spriteRenderer.color = _newColor;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
