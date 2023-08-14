using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    [CreateAssetMenu(fileName = "Command", menuName = "New Command/Basic")]
    public class PlayerUnitCommands : ScriptableObject
    {
        public enum CommandType
        {
            Move,
            Stop,
            HoldPosition,
            Patrol,
            Attack,
            Build
        }
        [Header("Settings")]
        [Space(15)]
        public new string name;
        public GameObject icon;
    }
}
