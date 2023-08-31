using GamePlay.Hero;
using Sirenix.OdinInspector;
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
            OnPressed();
         }
      }

      [Button]
      private void OnPressed()
      {
         _heroModel.Core.shoot.OnGetPressedFire.Invoke();
      }
   }
}

