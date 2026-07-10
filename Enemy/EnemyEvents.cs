using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyEvents
    {
        public static event Action OnSetNewDestination;

        public static void InvokeNewDestination()
        {
            OnSetNewDestination?.Invoke();
        }
    }
}