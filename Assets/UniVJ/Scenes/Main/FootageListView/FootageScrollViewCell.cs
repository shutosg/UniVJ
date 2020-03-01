using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

/// <summary>
/// 素材リストのセル。
/// </summary>
public class FootageScrollViewCell : FancyGridViewCell<FootageScrollViewData, FootageScrollViewContext>
{
    [SerializeField] Text _sceneName;
    [SerializeField] Image _cursorImage;
    [SerializeField] Button _selectButton;
    [SerializeField] Button _button;
    [SerializeField] Color _selectedCursorColor;
    [SerializeField] Color _nonSelectedCursorColor;

    void Start()
    {
        _button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        _selectButton.onClick.AddListener(() => {
            Context.OnCellClicked?.Invoke(Index);
            Context.OnCellSelectClicked?.Invoke(Index);
        });
    }

    public override void UpdateContent(FootageScrollViewData itemData)
    {
        _sceneName.text = itemData.SceneName;

        var selected = Context.SelectedIndex == Index;
        _cursorImage.color = selected ? _selectedCursorColor : _nonSelectedCursorColor;
    }
}

