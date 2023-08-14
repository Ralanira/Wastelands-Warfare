using UnityEngine;

namespace Interactables
{
    public class Interactable : MonoBehaviour
    {
        public bool isInteracting = false;
        [SerializeField] private GameObject selectionHighlight = null;
        [SerializeField] private GameObject cursorHighlight = null;
        private Units.UnitStatDisplay unitstat;

        void Start() 
        {
            HideSelectionHighlight();
            HideCursorHighlight(); 
        }

        public virtual void OnInteractEnter()
        {
            ShowSelectionHighlight();
            isInteracting = true;
        }

        public virtual void OnInteractExit()
        {
            HideSelectionHighlight();
            isInteracting = false;
        }

        public virtual void ShowSelectionHighlight()
        {
            selectionHighlight.SetActive(true);
        }

        public virtual void HideSelectionHighlight()
        {
            selectionHighlight.SetActive(false);
        }

        public virtual void ShowCursorHighlight()
        {
            cursorHighlight.SetActive(true);
        }

        public virtual void HideCursorHighlight()
        {
            cursorHighlight.SetActive(false);
        }

        private void OnParticleCollision(GameObject other)
        {
            float dmg = other.GetComponent<ProjectilesBehaviour>().damage;
            unitstat = GetComponentInChildren<Units.UnitStatDisplay>();
            unitstat.TakeDamage(dmg);
        }
    }
}

