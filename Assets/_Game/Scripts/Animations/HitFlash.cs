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

public class HitFlash
{
    private SpriteRenderer _renderer;
    private MonoBehaviour _monobehaviour;

    private Color _flashColor;

    private float _flashBlendDuration = .02f;
    private float _flashHoldDuration = .08f;

    private Color _startingColor;
    private Coroutine _flashRoutine = null;

    public HitFlash(MonoBehaviour monobehaviour, SpriteRenderer renderer, Color flashColor)
    {
        _monobehaviour = monobehaviour;
        _renderer = renderer;
        _flashColor = flashColor;
        _startingColor = _renderer.color;

        //CalculateFlashBlends(flashDuration);
    }

    /*
    private void CalculateFlashBlends(float flashDuration)
    {
        // if our flash isn't valid, don't do it
        if (flashDuration <= 0)
        {
            _flashInDuration = 0;
            _flashHoldDuration = 0;
            _flashOutDuration = 0;
        }
        // if we don't have enough time for transitions, just hold the flash
        else if (flashDuration < _flashInDuration + _flashOutDuration)
        {
            _flashInDuration = 0;
            _flashHoldDuration = flashDuration;
            _flashOutDuration = 0;
        }
        else
        {
            _flashHoldDuration = flashDuration - (_flashInDuration + _flashOutDuration);
        }
    }*/

    #region Public Functions

    public void Flash(float duration)
    {
        if (duration <= 0) { return; }    // 0 speed wouldn't make sense

        if (_flashRoutine != null)
            StopFlash();
        _flashRoutine = _monobehaviour.StartCoroutine(FlashRoutine(_flashColor, duration));
    }

    public void StopFlash()
    {
        if (_flashRoutine != null)
            _monobehaviour.StopCoroutine(_flashRoutine);

        SetInitialValues();
    }
    #endregion

    #region Private Functions
    IEnumerator FlashRoutine(Color flashColor, float duration)
    {
        float numberOfFlashes = duration /
            ((_flashBlendDuration*2) + _flashHoldDuration);

        // guarantee at least 1 flash
        numberOfFlashes = Mathf.CeilToInt(numberOfFlashes);
        // start sequence
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // flash in
            for (float elapsed = 0; elapsed <= _flashBlendDuration; elapsed += Time.deltaTime)
            {
                _renderer.color = Color.Lerp(_startingColor, flashColor, elapsed / _flashBlendDuration);
                yield return null;
            }
            _renderer.color = flashColor;
            // hold
            yield return new WaitForSeconds(_flashHoldDuration);
            // flash out
            for (float elapsed = 0; elapsed <= _flashBlendDuration; elapsed += Time.deltaTime)
            {
                _renderer.color = Color.Lerp(flashColor, _startingColor, elapsed / _flashBlendDuration);
                yield return null;
            }
            _renderer.color = _startingColor;

            yield return new WaitForSeconds(_flashHoldDuration);
        }

        _renderer.color = _startingColor;
    }

    private void SetInitialValues()
    {
        _renderer.color = _startingColor;
    }

    #endregion
}

