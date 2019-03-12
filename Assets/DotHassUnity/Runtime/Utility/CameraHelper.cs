using UnityEngine;

namespace DotHass.Unity
{
    public static class CameraHelper
    {
        public static Camera FindCameraForLayer(int layer)
        {
            int layerMask = (1 << layer);
            Camera[] cameras = GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];

            for (int i = 0; i < cameras.Length; i++)
            {
                if ((cameras[i].cullingMask & layerMask) != 0)
                    return cameras[i];
            }

            return null;
        }


        //屏幕位置转到世界坐标..相当于   orthoCamera.ScreenToWorldPoint()
        public static Vector3 NormalizeScreenPosition(Camera orthoCamera, Vector3 screenPosition)
        {
            float size = orthoCamera.orthographicSize;

            float v = size * 2;
            float h = v * ((float)Screen.width / (float)Screen.height);

            float x = ((screenPosition.x / Screen.width) - 0.5f) * h;
            float y = ((screenPosition.y / Screen.height) - 0.5f) * v;

            Vector3 norm = new Vector3(x, y, 0.0f);

            return norm;
        }

        public static Vector3 ViewportProtrusion(Vector3 viewportPosition, Vector2 ratio)
        {
            float protX;
            if (viewportPosition.x < (1.0f - ratio.x) / 2.0f)
                protX = (1.0f - ratio.x) / 2.0f;
            else if (viewportPosition.x > 1.0f - ((1.0f - ratio.x) / 2.0f))
                protX = 1.0f - ((1.0f - ratio.x) / 2.0f);
            else
                protX = viewportPosition.x;

            float protY;
            if (viewportPosition.y < (1.0f - ratio.y) / 2.0f)
                protY = (1.0f - ratio.y) / 2.0f;
            else if (viewportPosition.y > 1.0f - ((1.0f - ratio.y) / 2.0f))
                protY = 1.0f - ((1.0f - ratio.y) / 2.0f);
            else
                protY = viewportPosition.y;

            return new Vector3(protX, protY, viewportPosition.z);
        }




    



        /// <summary>
        /// ViewPort Space（视口坐标）:视口坐标是标准的和相对于相机的。
        /// 相机的左下角为（0，0）点，右上角为（1，1）点，Z的位置是以相机的世界单位来衡量的。
        ///相机必须是objectTransformPosition的gameobject的相机
        ///不能是其他相机
        //画布的0,0位于屏幕的中心，而worldToviewPortpoint将左下角视为0,0。因此，您需要减去画布的高度/宽度*0.5以获得正确的位置。
        /// </summary>
        /// <param name="objectTransformPosition"></param>
        public static Vector3 worldspacetocanvasspace(Vector3 objectTransformPosition, RectTransform CanvasRect)
        {
            // Get the position on the canvas
            Vector3 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
            Vector3 result = new Vector3(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)), 0);

            return CanvasRect.TransformPoint(result); ;

        }

        /// <summary>
        /// 方法2使用RectTransformUtility
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>

        public static Vector3 WorldToCanvasPosition(Vector3 position, RectTransform CanvasRect, Canvas Canvas, Camera UICamera)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            Vector2 result;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, screenPoint, Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : UICamera, out result);

            return CanvasRect.TransformPoint(result); 
        }
    }
}