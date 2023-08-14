using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MinimapTools
{
    public class MinimapFrame : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask interactableLayer = new LayerMask();

        void FixedUpdate()
        {
            Ray[] corners = new Ray[4];
            corners[0] = mainCam.ScreenPointToRay(new Vector3(0f,0f));
            corners[3] = mainCam.ScreenPointToRay(new Vector3(Screen.width, 0f));
            corners[1] = mainCam.ScreenPointToRay(new Vector3(0f, Screen.height));
            corners[2] = mainCam.ScreenPointToRay(new Vector3(Screen.width, Screen.height));
            RaycastHit[] hits = new RaycastHit[4];
            for (int i = 0; i < 4; i++)
            {
                if (Physics.Raycast(corners[i], out hits[i], 1000, interactableLayer))
                {
                    lineRenderer.SetPosition(i, hits[i].point); 
                }
            }
        }
    }
}
