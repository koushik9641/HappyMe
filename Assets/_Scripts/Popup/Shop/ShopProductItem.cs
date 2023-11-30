using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopProductItem : MonoBehaviour
{
    [SerializeField] private Button _button = null;
    [SerializeField] private Text   _textPrice = null;

    public Button button => _button;

    public Text textPrice { get  { return _textPrice; } set { _textPrice = value; }  }
}
