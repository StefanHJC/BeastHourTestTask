using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(menuName = "New Skill/Dash", fileName = "Dash", order = 51)]
    public class Dash : Skill
    {
        [SerializeField] private int _speedMultiplier;
        [SerializeField] private float _distance;
        [SerializeField] private float _cooldown;

        private Vector3 _startPositon;

        public override float Cooldown => _cooldown;

        public override event UnityAction<Skill> EffectElapsed;

        public override void Apply(Player player)
        {
            var currentMoveDirection = new Vector3(player.CurrentSpeed.x, 0, player.CurrentSpeed.y);
            currentMoveDirection = currentMoveDirection.normalized;
            Debug.DrawRay(player.transform.position, currentMoveDirection, Color.red, 100f);
            _startPositon = player.transform.position;

            player.StartCoroutine(StartDash(player, currentMoveDirection));
        }

        private IEnumerator StartDash(Player player, Vector3 direction)
        {
            while (Vector3.Distance(player.transform.position, _startPositon) < _distance)
            {
                player.transform.Translate(direction * _speedMultiplier * Time.deltaTime, Space.World);

                yield return null;
            }
            EffectElapsed?.Invoke(this);
        }
    }        
}