using UnityEngine;
using UnityEngine.UI;

public class Popup_PurchaseSuccess : Popup
{
    [SerializeField] private Text _textDescription = null;

    public Text textDescription { get { return _textDescription; } set { _textDescription = value; } }
}
