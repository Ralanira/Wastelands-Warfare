using UnityEngine;

namespace UI.InGame.MinimapTools
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private RectTransform playerInMap;
        [SerializeField] private RectTransform map2dEnd;
        [SerializeField] private Transform map3dParent;
        [SerializeField] private Transform map3dEnd;

        private Vector3 normalized, mapped;

        private void Update()
        {
            normalized = Divide(
                map3dParent.InverseTransformPoint(this.transform.position),
                map3dEnd.position - map3dParent.position);
            normalized.y = normalized.z;
            mapped = Multiply(normalized, map2dEnd.localPosition);
            mapped.z = 0;
            playerInMap.localPosition = mapped;
        }

        private static Vector3 Divide(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        private static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}