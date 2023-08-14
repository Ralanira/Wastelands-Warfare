using UnityEngine;

namespace Interactables
{
    public class IBuilding : Interactable
    {
        public UI.InGame.ActionPanel.PlayerActions actions;
        [SerializeField] private GameObject spawnMarker = null;
        [SerializeField] private GameObject spawnMarkerGraphics = null;

        public override void OnInteractEnter()
        {
            if (!this.gameObject.GetComponent<Units.BuildingConstructing>())
            {
                UI.InGame.ActionPanel.ActionFrame.instance.SetActionButtons(actions);
            }
            if(spawnMarker != null)
            {
                spawnMarkerGraphics.SetActive(true);
            }
            base.OnInteractEnter();
        }

        public override void OnInteractExit()
        {
            UI.InGame.ActionPanel.ActionFrame.instance.ClearActions();
            if(spawnMarker != null)
            {
                spawnMarkerGraphics.SetActive(false);
            }
            base.OnInteractExit();
        }

        public void SetSpawnMarkerLocation()
        {
            if(spawnMarker != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    spawnMarker.transform.position = hit.point;
                }
            }
        }
    }
}

