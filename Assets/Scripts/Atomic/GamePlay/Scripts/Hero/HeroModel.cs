using Declarative;
using UnityEngine;


namespace Lessons.Gameplay.Atomic1
{
    public sealed class HeroModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        public HeroModel_Core Core = new();
        
        [Section]
        [SerializeField]
        public HeroModel_View View = new();
    }
}