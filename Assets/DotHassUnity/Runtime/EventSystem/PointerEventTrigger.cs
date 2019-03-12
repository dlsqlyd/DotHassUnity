using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DotHass.Unity
{


    /// <summary>
    /// 只实现点击部分接口
    /// 不实现拖动等其他接口
    /// 全部实现的话射线会触发很多不需要的事件。然后会阻止事件的继续发生。
    /// 
    /// 比如。一个scrollview。其中有很多button挂上PointerEventTrigger。如果该类实现了拖动的接口。。就会阻挡scrollview的拖动
    /// </summary>
    public class PointerEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        private float holdTime = 1f;

        public delegate void VoidDelegate(PointerEventData eventData);
        public delegate void BoolDelegate(PointerEventData eventData, bool value);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onUp;
        public BoolDelegate onHover;
        public VoidDelegate onLongPress;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick.Invoke(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Invoke("OnLongPress", holdTime);
            if (onDown != null) onDown.Invoke(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            CancelInvoke("OnLongPress");
            if (onUp != null) onUp.Invoke(eventData);
        }
        private void OnLongPress()
        {
            if (onLongPress != null) onLongPress.Invoke(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onHover != null) onHover.Invoke(eventData, true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (onHover != null) onHover.Invoke(eventData, false);
        }
    }
}
