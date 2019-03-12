#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace DotHass.Unity
{
    /// <summary>
    /// 显示raycastTarget的目标
    /// https://www.xuanyusong.com/archives/4291
    /// </summary>
    public class RaycastTargetUILine : MonoBehaviour
    {
        static Vector3[] fourCorners = new Vector3[4];
        void OnDrawGizmos()
        {
            foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
            {
                if (g.raycastTarget)
                {
                    RectTransform rectTransform = g.transform as RectTransform;
                    rectTransform.GetWorldCorners(fourCorners);
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);

                }
            }
        }
    }
}

#endif