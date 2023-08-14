using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager instance = null;
        [SerializeField] private List<Transform> objectivesToDestroy = new();
        private int _objectivesCount;
        private UI.Menus.PauseMenu pauseMenu;

        public int objectivesCount
        {
            get
            {
                return _objectivesCount;
            }
            set
            {
                _objectivesCount = value;
                if (_objectivesCount <= 0)
                {
                    if (this) 
                    {
                        StartCoroutine(ShowVictoryScreen());
                    }
                }
            }
        }

        void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        void Start()
        {
            foreach (Transform objective in objectivesToDestroy)
            {
                objective.gameObject.AddComponent<ObjectiveToDestroy>();
            }
            objectivesCount = objectivesToDestroy.Count;
        }

        public void RemoveObjective()
        {
            objectivesCount -= 1;
        }
        
        private IEnumerator ShowVictoryScreen()
        {
            pauseMenu = FindObjectOfType<UI.Menus.PauseMenu>();
            yield return new WaitForSecondsRealtime(2);
            pauseMenu.ShowEndgameScreen();
        }
        
    }
}
