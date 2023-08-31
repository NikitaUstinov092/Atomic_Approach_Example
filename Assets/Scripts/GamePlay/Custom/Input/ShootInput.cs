using GamePlay.Hero;
using UnityEngine;

namespace GamePlay.Custom.Input
{
   public class ShootInput : MonoBehaviour
   {
      [SerializeField]
      private HeroModel _heroModel;

      private void Update()
      {
         if (UnityEngine.Input.GetMouseButtonDown(0))
         {
            _heroModel.Core.shoot.OnGetPressedFire.Invoke();
         }
      }
   }
}

