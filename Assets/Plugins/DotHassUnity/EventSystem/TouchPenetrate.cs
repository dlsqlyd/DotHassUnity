using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 允许事件能够穿透到下层
/// https://www.xuanyusong.com/archives/4241
/// 需求：
/// 1.当点击英雄按钮会弹出菜单。和blocking。
/// 2.点击任何地方blocking隐藏菜单和blocking
/// 3.如果blocking下面有其他可射线的object（按钮等），穿透blocking触发事件
/// </summary>
public class TouchPenetrate : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 监听点击
    /// 在同一对象上按下并释放指针时调用
    /// 在OnPointerUp后发生，所以如果没有触发up动作（鼠标可能移除点击物体范围）。。就不会发生click事件
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }


    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            var nextTarget = results[i].gameObject;
            if (current != nextTarget)
            {
                //新建立一个事件数据。。保证PointerEventData是正常的
                var eventData = new PointerEventData(EventSystem.current);
                eventData.rawPointerPress = data.rawPointerPress;
                eventData.pointerPress = nextTarget;
                eventData.pointerEnter = nextTarget;
                eventData.pointerDrag = nextTarget;
                ExecuteEvents.Execute(nextTarget, eventData, function);
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                break;
            }
        }
    }

}