
namespace PlayerStateMachine
{
    public class HitTransition : Transition
    {
        private void OnEnable()
        {
            player.Hitted += OnPlayerHitted;
        }

        private void OnDisable()
        {
            player.Hitted -= OnPlayerHitted;
        }

        private void OnPlayerHitted()
        {
            IsNeedTransit = true;
        }
    }
}