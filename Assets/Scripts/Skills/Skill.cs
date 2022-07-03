using UnityEngine.Events;
using UnityEngine;

namespace Skills
{
    public abstract class Skill : ScriptableObject
    {
        public abstract float Cooldown { get; }

        public abstract event UnityAction<Skill> EffectElapsed;

        public abstract void Apply(Player player);
    }
}