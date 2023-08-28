using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakAttack : MonoBehaviour
{
   public bool GetState() => _state;
   
   private bool _state;
   /// <summary>
   /// Не удалять, аниматор работает с методом
   /// </summary>
   private void Attack()
   {
      _state = true;
   }
  
   /// <summary>
   /// Не удалять, аниматор работает с методом
   /// </summary>
   private void EndAttack()
   {
      _state = false;
   }
}
