using UnityEngine.Events;
using UnityEngine;
using Mirror;

public class InputHandler : NetworkBehaviour
{
    private Player _player;

    public static InputHandler Instance { get; private set; }

    public Vector2 KeyboardInput { get; private set; }
    public Vector2 MouseInput { get; private set; }

    public event UnityAction LeftMouseButtonPerformed;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_player == null)
            return;

        var input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            LeftMouseButtonPerformed?.Invoke();

        if (Input.GetKey(KeyCode.A))
            input.x = -1;
        if (Input.GetKey(KeyCode.D))
            input.x = 1;
        if (Input.GetKey(KeyCode.W))
            input.y = 1;
        if (Input.GetKey(KeyCode.S))
            input.y = -1;
        
        KeyboardInput = input;
        _player.MoveInput = KeyboardInput;
        MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
