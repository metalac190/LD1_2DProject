using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] private float _alphaSet = 0.8f;

    private float _timeActivated;
    private float _alpha;
    [SerializeField]
    private float _alphaDecay = 10f;

    private PlayerMovement_Old _player;
    private Transform _playerSpriteTransform;

    private SpriteRenderer _playerSpriteRenderer;
    private PlayerAfterImagePool _pool;

    private Color _playerColor;
    private Color _newColor;

    bool _isInitialized = false;

    public void Initialize(Player player, PlayerAfterImagePool pool)
    {
        _pool = pool;
        _playerSpriteRenderer = player.PlayerAnimator.SpriteRenderer;

        _playerSpriteTransform = _playerSpriteRenderer.transform;
        _playerColor = _playerSpriteRenderer.color;
        transform.localScale = _playerSpriteTransform.localScale;
        // start new sprite sequence
        _alpha = _alphaSet;
        _renderer.sprite = _playerSpriteRenderer.sprite;
        transform.position = _playerSpriteTransform.position;
        transform.rotation = _playerSpriteTransform.rotation;
        _timeActivated = Time.time;
        // remove the parent, so it doesn't follow player
        transform.SetParent(null);

        _isInitialized = true;
    }

    private void OnDisable()
    {
        _isInitialized = false;
    }

    private void Update()
    {
        if (_isInitialized)
        {
            ProgressAnimation();
        }
    }

    private void ProgressAnimation()
    {
        _alpha -= _alphaDecay * Time.deltaTime;
        _newColor = new Color(_playerColor.r, _playerColor.g, _playerColor.b, _alpha);
        _renderer.color = _newColor;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            _pool.AddToPool(this);
            
        }
    }
}
