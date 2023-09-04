using GamePlay.Custom.GameMachine;
using GamePlay.Hero;
using UnityEngine;

namespace GamePlay.Custom.Input
{
   public class ShootInput : MonoBehaviour, IUpdateListener
   {
      [SerializeField]
      private HeroModel _heroModel;

      void IUpdateListener.Update()
      {
         if (UnityEngine.Input.GetMouseButtonDown(0))
            OnPressed();
      }
      private void OnPressed()
      {
         _heroModel.Core.ShootComp.OnGetPressedFire.Invoke();
      }
   }
}

