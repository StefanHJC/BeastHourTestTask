using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Space]
    [Header("Input Fields")]
    [SerializeField] private InputField _playerNameInputField;

    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _hostRoomButton;
    [SerializeField] private Button _joinRoomButton;

    [Space]
    [SerializeField] private CustomNetworkRoomManager _roomManager;

    public static readonly string PlayerPrefsNameKey = "Name";

    public string playerName = "Player";

    private void Start()
    {
        _playerNameInputField.onEndEdit.AddListener(SavePlayerName);
        _hostRoomButton.onClick.AddListener(HostRoom);
        _joinRoomButton.onClick.AddListener(JoinRoom);
        TryGetPlayerName();
    }

    public void TryGetPlayerName()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
            _playerNameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);   
    }

    public void SavePlayerName(string userInput)
    {
        string playerName = string.Empty;
        
        if (string.IsNullOrEmpty(userInput) == false)
        {
            playerName = userInput;
            PlayerPrefs.SetString("Name", playerName);
        }
    }

    private void HostRoom()
    {
        _hostRoomButton.onClick.RemoveListener(HostRoom);
        _roomManager.StartHost();
    }

    private void JoinRoom()
    {
        _joinRoomButton.onClick.RemoveListener(JoinRoom);
        _roomManager.StartClient();
    }
}
