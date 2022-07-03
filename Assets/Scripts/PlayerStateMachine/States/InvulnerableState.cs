using UnityEngine;

namespace PlayerStateMachine
{
    public class InvulnerableState : State
    {
        [SerializeField] private Color _invulnerableColor;

        private void OnEnable()
        {
            player.SetColor(_invulnerableColor);
            //PlayerColorController.Instance.CmdChangeColor(player, _invulnerableColor);
        }

        private void OnDisable()
        {
            //PlayerColorController.Instance.CmdChangeColor(player, Color.white);
            player.SetColor(Color.white);
        }
    }
}