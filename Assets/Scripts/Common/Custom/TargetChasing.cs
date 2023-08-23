using System;
using UnityEngine;

namespace Common.Custom
{
    public class TargetChasing: MonoBehaviour
    {
        [SerializeField] private Transform target;
        private void FixedUpdate()
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}