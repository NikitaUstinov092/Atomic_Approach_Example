using Declarative;
using UnityEngine;

namespace Atomic.GamePlay.Scripts.Zombie
{
    public class ZombieModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        public ZombieModel_Core Core = new();
        
        [Section]
        [SerializeField]
        public ZombieModel_View View = new();
    }
}


