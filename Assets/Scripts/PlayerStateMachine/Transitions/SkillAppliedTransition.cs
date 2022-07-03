using UnityEngine;
using Skills;

namespace PlayerStateMachine
{
    public class SkillAppliedTransition : Transition
    {
        [SerializeField] private Skill _transitSkill;

        private void OnEnable()
        {
            player.SkillApplied += OnSkillApplied;
        }

        private void OnDisable()
        {
            player.SkillApplied -= OnSkillApplied;
        }

        private void OnSkillApplied(Skill appliedSkill)
        {
            if (appliedSkill.GetType() == _transitSkill.GetType())
                IsNeedTransit = true;
        }
    }
}