using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.Objectives
{
    public class ObjectiveToDestroy : MonoBehaviour
    {
        private void OnDestroy() 
        {
            UI.InGame.Objectives.ObjectiveManager.instance.RemoveObjective();
        }
    }
}
