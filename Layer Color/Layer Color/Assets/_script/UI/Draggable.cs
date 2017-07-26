using UnityEngine;
using UnityEngine.EventSystems;

namespace UIComponents.Items
{
    /// <summary>
    /// uGUIをカーソルでドラッグ出来るようにする
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("UI Components/Items/Draggable")]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        /// <summary>カーソルの座標</summary>
        private Vector2 position = Vector2.zero;


        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="pointerEventData">ドラッグ情報</param>
        public void OnBeginDrag(PointerEventData pointerEventData)
        {
            this.position = pointerEventData.position;
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="pointerEventData">ドラッグ情報</param>
        public void OnDrag(PointerEventData pointerEventData)
        {
            var d = pointerEventData.position - this.position;

            transform.position = d + (Vector2)transform.position;
            this.position = pointerEventData.position;
        }
    }
}
