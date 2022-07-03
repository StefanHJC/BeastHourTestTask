using UnityEngine.UI;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Text))]
public class TextMessageBox : NetworkBehaviour
{
    private Text _textBox;

    [SyncVar(hook = nameof(SetMessageBoxText))]
    public string text;

    public static TextMessageBox Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _textBox = GetComponent<Text>();
    }

    private void SetMessageBoxText(string _, string newValue)
    {
        _textBox.text = newValue;
    }
}
