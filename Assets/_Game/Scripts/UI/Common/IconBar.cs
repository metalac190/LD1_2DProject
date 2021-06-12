using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Sprite _iconImage = null;
    [SerializeField] Image _backgroundImage = null;
    [SerializeField] Image _iconPrefab = null;

    [Header("Icon Settings")]
    [SerializeField] Color _colorTintFilled = Color.cyan;
    [SerializeField] Color _colorTintEmpty = Color.clear;
    [SerializeField] Color _backgroundColor = Color.clear;
    [SerializeField] GridLayoutGroup _iconLayoutGroup = null;
    [SerializeField] int _startingIcons = 2;
    [SerializeField] int _maxStartingIcons = 5;

    List<Image> _icons = new List<Image>();
    public int MaxIconCount => _icons.Count;

    RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _backgroundImage.color = _backgroundColor;

        ConstrainGridPadding();
        SetMaxIcons(_maxStartingIcons);
        FillIcons(_startingIcons);
    }

    public void SetMaxIcons(int newMaxNumber)
    {
        if (newMaxNumber == MaxIconCount || newMaxNumber < 0)
            return;
        // if our new max is lower than our current amount
        if(newMaxNumber < MaxIconCount)
        {
            int numberToRemove = MaxIconCount - newMaxNumber;
            for (int i = 0; i < numberToRemove; i++)
            {
                RemoveIcon(i);
            }
        }

        // if our new max is higher than our current amount
        if(newMaxNumber > _icons.Count)
        {
            int numberToAdd = newMaxNumber - MaxIconCount;
            for (int i = 0; i < numberToAdd; i++)
            {
                CreateIcon();
            }
        }
    }

    private void RemoveIcon(int index)
    {
        Image _iconToRemove = _icons[index];
        _icons.RemoveAt(index);
        Destroy(_iconToRemove.gameObject);
    }

    public void FillIcons(int numberToFill)
    {
        // start from the bottom
        for (int i = 0; i < _icons.Count; i++)
        {
            // fill the requested number of icons
            if(i <= numberToFill-1)
            {
                FillIcon(_icons[i]);
            }
            // clear the rest
            else if (i > numberToFill-1)
            {
                ClearIcon(_icons[i]);
            }
        }
    }

    void CreateIcon()
    {
        Image icon = Instantiate(_iconPrefab, _iconLayoutGroup.transform);

        icon.sprite = _iconImage;
        ClearIcon(icon);
        _icons.Add(icon);
    }

    void FillIcon(Image image)
    {
        image.color = _colorTintFilled;
    }

    void ClearIcon(Image image)
    {
        image.color = _colorTintEmpty;
    }

    void ConstrainGridPadding()
    {
        // constrain grid padding to size of our entire UI object
        // note this currently only supports square icons
        _iconLayoutGroup.cellSize = new Vector2
            (_rectTransform.sizeDelta.y, _rectTransform.sizeDelta.y);
    }
}
