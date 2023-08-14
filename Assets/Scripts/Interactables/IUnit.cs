using UnityEngine;

namespace Interactables
{
    public class IUnit : Interactable
    {
        [SerializeField] UI.InGame.ActionPanel.PlayerActions actions;
        public override void OnInteractEnter()
        {
            UI.InGame.ActionPanel.ActionFrame.instance.SetActionButtons(actions);
            base.OnInteractEnter();
        }

        public override void OnInteractExit()
        {
            base.OnInteractExit();
        }
    }
}

