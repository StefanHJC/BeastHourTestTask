using Skills;
using UnityEngine;

namespace PlayerStateMachine
{
    public class SkillElapsedTransition : Transition
    {
        [SerializeField] private Skill _transitSkill;

        private void OnEnable()
        {
            player.SkillElapsed += OnSkillElapsed;
        }

        private void OnDisable()
        {
            player.SkillElapsed  -= OnSkillElapsed;
        }

        private void OnSkillElapsed(Skill appliedSkill)
        {
            if (appliedSkill.GetType() == _transitSkill.GetType())
                IsNeedTransit = true;
        }
    }
}