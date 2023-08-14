using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Player;
using UnityEngine.EventSystems;

namespace InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;
        public List<Transform> selectedUnits = new List<Transform>();
        public Transform selectedBuilding = null;
        [SerializeField] private LayerMask interactableLayer = new LayerMask();
        private RaycastHit hit;
        private bool isDragging = false;
        private Vector3 mousePos;

        private void Awake() 
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        private void OnGUI() 
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.blue);
            }
        }

        public void HandleUnitMovement()
        {
            if (Input.GetMouseButtonDown(0) && !BuildingsPlacementManager.instance.flyingBuilding && !BuildingsPlacementManager.instance.isPlaceable)
            {
                
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000, interactableLayer))
                {
                    if (addedUnit(hit.transform, Input.GetKey(KeyCode.LeftShift), Input.GetKey(KeyCode.LeftControl)))
                    {
                       
                    }
                    else if (addedBuilding(hit.transform))
                    {

                    }
                } 
                else
                {
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    DeselectUnits();
                }
                foreach (Transform child in My.Player.PlayerManager.instance.playerUnits)
                {
                    foreach (Transform unit in child)
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                            addedUnit(unit, true);
                        }   
                    }
                }
                isDragging = false;
            }

            if(Input.GetMouseButtonDown(1) && BuildingsPlacementManager.instance.flyingBuilding)
            {
                BuildingsPlacementManager.instance.DestroyFlyingBuilding();
                BuildingsPlacementManager.instance.flyingBuilding = null;
                BuildingsPlacementManager.instance.isPlaceable = false;
            }

            if(BuildingsPlacementManager.instance.flyingBuilding && Input.GetMouseButtonDown(0))
            {
                BuildingsPlacementManager.instance.ConfirmBuildingPlacement();
            }

            if(Input.GetMouseButtonDown(1) && HaveSelectedUnits())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    LayerMask layerHit = hit.transform.gameObject.layer;
        
                    switch (layerHit.value)
                    {
                        case 8:
                            foreach(Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                pU.StopMining();
                                pU.CancelConstructingBuilding();
                                pU.MoveOrderUnit(hit.transform.position);
                            }
                            break;
                        case 9:
                            foreach(Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                pU.StopMining();
                                pU.CancelConstructingBuilding();
                                pU.SetAggroTarget(hit.transform);
                            }
                            break;
                        case 10:
                            foreach(Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                Units.Basic.Workers.ResourceMiner miningManager = unit.gameObject.GetComponent< Units.Basic.Workers.ResourceMiner>();

                                if(hit.collider.gameObject.GetComponent<Resources.ResourceType>())
                                {
                                    if(miningManager != null)
                                    {
                                        pU.CancelConstructingBuilding();
                                        pU.CommandMining(hit.collider.gameObject);
                                    }
                                    else
                                    {
                                        UI.InGame.MessageString.instance.PrintMessage("This unit can't gather resources!");
                                    }
                                }
                            }
                            break;
                        default: 
                            foreach(Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                pU.StopMining();
                                pU.CancelConstructingBuilding();
                                pU.MoveOrderUnit(hit.point);
                            }
                            break;
                    }
                } 
            }
            else if (Input.GetMouseButtonDown(1) && selectedBuilding != null)
            {
                selectedBuilding.gameObject.GetComponent<Interactables.IBuilding>().SetSpawnMarkerLocation();
            }
            if (Input.GetKeyDown(KeyCode.D) && selectedBuilding != null)
            {
                selectedBuilding.gameObject.GetComponentInChildren<Units.UnitStatDisplay>().Die();
            }
        }

        private void DeselectUnits()
        {
            if (selectedBuilding)
            {
                selectedBuilding.gameObject.GetComponent<Interactables.IBuilding>().OnInteractExit();
                selectedBuilding = null;
            }
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].gameObject.GetComponent<Interactables.IUnit>().OnInteractExit();
            }
            selectedUnits.Clear();
            UI.InGame.ActionPanel.ActionFrame.instance.ClearActions();
        }

        private bool isWithinSelectionBounds(Transform tf)
        {
            if (!isDragging)
            {
                return false;
            }
            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }

        private bool HaveSelectedUnits()
        {
            if (selectedUnits.Count > 0)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        private Interactables.IUnit addedUnit(Transform tf, bool canMultiSelect = false, bool selectAllOfType = false)
        {
            Interactables.IUnit iUnit = tf.GetComponent<Interactables.IUnit>();
            if (iUnit)
            {
                if (!canMultiSelect)
                {
                    DeselectUnits();
                }

                if (selectAllOfType)
                {
                    Transform unitParent = tf.parent.gameObject.transform;
                    foreach (Transform unit in unitParent)
                    {
                        Interactables.IUnit iCurrentUnit = unit.GetComponent<Interactables.IUnit>();
                        selectedUnits.Add(iCurrentUnit.gameObject.transform);
                        iCurrentUnit.OnInteractEnter();
                    }
                }
                else
                {
                    if (selectedUnits.Contains(iUnit.gameObject.transform) && canMultiSelect && !isDragging)
                    {
                        selectedUnits.Remove(iUnit.gameObject.transform);
                        iUnit.OnInteractExit();
                        return null;
                    }
                    else
                    {
                        selectedUnits.Add(iUnit.gameObject.transform);
                        iUnit.OnInteractEnter();
                    }
                }
                return iUnit;
            }
            else
            {
                return null;
            }
        }
        
        private Interactables.IBuilding addedBuilding(Transform tf)
        {
            Interactables.IBuilding iBuilding = tf.GetComponent<Interactables.IBuilding>();
            if (iBuilding)
            {
                DeselectUnits();
                selectedBuilding = iBuilding.gameObject.transform;
                iBuilding.OnInteractEnter();
                return iBuilding;
            }
            else
            {
                return null;
            }
        }
    }
}