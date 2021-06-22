using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// This script will flash a UI Image component with the given parameters. Useful for creating
/// quick, animated UI flashes.
/// Created by: Adam Chandler
/// Make sure that you attach this script to an Image component. You can optionally call the
/// flash remotely and pass in new flash values, or you can predefine settings in the Inspector
/// </summary>

public class DamageFlash
{
    private SpriteRenderer _renderer;
    private Health _health;

    private Color _flashColor;
    private float _flashInDuration = .02f;
    private float _flashHoldDuration = .1f;
    private float _flashOutDuration = .1f;

    private Color _startingColor;
    private Coroutine _flashRoutine = null;

    public DamageFlash(Health health, SpriteRenderer renderer, Color flashColor)
    {
        _health = health;
        _renderer = renderer;
        _flashColor = flashColor;

        _startingColor = _renderer.color;
    }

    #region Public Functions

    public void Flash()
    {
        if (_flashInDuration <= 0) { return; }    // 0 speed wouldn't make sense

        if (_flashRoutine != null)
            StopFlash();
        _flashRoutine = _health.StartCoroutine(FlashRoutine(_flashColor, _flashInDuration, 
            _flashHoldDuration, _flashOutDuration));
    }

    public void StopFlash()
    {
        if (_flashRoutine != null)
            _health.StopCoroutine(_flashRoutine);

        SetInitialValues();
    }
    #endregion

    #region Private Functions
    IEnumerator FlashRoutine(Color flashColor, float flashInDuration, 
        float flashHoldDuration, float flashOutDuration)
    {
        // flash in
        for (float elapsed = 0; elapsed <= flashInDuration; elapsed += Time.deltaTime)
        {
            _renderer.color = Color.Lerp(_startingColor, flashColor, elapsed / flashInDuration);
            yield return null;
        }
        // hold
        yield return new WaitForSeconds(_flashHoldDuration);
        // flash out
        for (float elapsed = 0; elapsed <= flashOutDuration; elapsed += Time.deltaTime)
        {
            _renderer.color = Color.Lerp(flashColor, _startingColor, elapsed / flashOutDuration);
            yield return null;
        }
    }

    private void SetInitialValues()
    {
        _renderer.color = _startingColor;
    }

    #endregion
}

