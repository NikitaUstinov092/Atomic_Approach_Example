using Lessons.Gameplay.Atomic1;
using UnityEngine;

public class ShootInput : MonoBehaviour
{
   [SerializeField]
   private HeroModel _heroModel;

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Debug.Log("++");
         _heroModel.Core.shoot.OnGetPressedFire.Invoke();
      }
        
   }
}

