using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DotHass.Unity
{
    public static class TransformExtension
    {


        #region Position

        /**
			Sets the X position of this transform.
		*/

        public static void SetX(this Transform transform, float x)
        {
            var newPosition = new Vector3(x, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }

        /**
			Sets the Y position of this transform.
		*/

        public static void SetY(this Transform transform, float y)
        {
            var newPosition = new Vector3(transform.position.x, y, transform.position.z);

            transform.position = newPosition;
        }

        /**
			Sets the Z position of this transform.
		*/

        public static void SetZ(this Transform transform, float z)
        {
            var newPosition = new Vector3(transform.position.x, transform.position.y, z);

            transform.position = newPosition;
        }

        /**
			Sets the X and Y position of this transform.
		*/

        public static void SetXY(this Transform transform, float x, float y)
        {
            var newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;
        }

        /**
			Sets the X and Z position of this transform.
		*/

        public static void SetXZ(this Transform transform, float x, float z)
        {
            var newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }

        /**
			Sets the Y and Z position of this transform.
		*/

        public static void SetYZ(this Transform transform, float y, float z)
        {
            var newPosition = new Vector3(transform.position.x, y, z);
            transform.position = newPosition;
        }

        /**
			Sets the X, Y and Z position of this transform.
		*/

        public static void SetXYZ(this Transform transform, float x, float y, float z)
        {
            var newPosition = new Vector3(x, y, z);
            transform.position = newPosition;
        }

        /**
			Translates this transform along the X axis.
		*/

        public static void TranslateX(this Transform transform, float x)
        {
            var offset = new Vector3(x, 0, 0);

            transform.position += offset;
        }

        /**
			Translates this transform along the Y axis.
		*/

        public static void TranslateY(this Transform transform, float y)
        {
            var offset = new Vector3(0, y, 0);

            transform.position += offset;
        }

        /**
			Translates this transform along the Z axis.
		*/

        public static void TranslateZ(this Transform transform, float z)
        {
            var offset = new Vector3(0, 0, z);
            transform.position += offset;
        }

        /**
			Translates this transform along the X and Y axes.
		*/

        public static void TranslateXY(this Transform transform, float x, float y)
        {
            var offset = new Vector3(x, y, 0);
            transform.position += offset;
        }

        /**
			Translates this transform along the X and Z axes.
		*/

        public static void TranslateXZ(this Transform transform, float x, float z)
        {
            var offset = new Vector3(x, 0, z);
            transform.position += offset;
        }

        /**
			Translates this transform along the Y and Z axes.
		*/

        public static void TranslateYZ(this Transform transform, float y, float z)
        {
            var offset = new Vector3(0, y, z);
            transform.position += offset;
        }

        /**
			Translates this transform along the X, Y and Z axis.
		*/

        public static void TranslateXYZ(this Transform transform, float x, float y, float z)
        {
            var offset = new Vector3(x, y, z);
            transform.position += offset;
        }

        /**
			Sets the local X position of this transform.
		*/

        public static void SetLocalX(this Transform transform, float x)
        {
            var newPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local Y position of this transform.
		*/

        public static void SetLocalY(this Transform transform, float y)
        {
            var newPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local Z position of this transform.
		*/

        public static void SetLocalZ(this Transform transform, float z)
        {
            var newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local X and Y position of this transform.
		*/

        public static void SetLocalXY(this Transform transform, float x, float y)
        {
            var newPosition = new Vector3(x, y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local X and Z position of this transform.
		*/

        public static void SetLocalXZ(this Transform transform, float x, float z)
        {
            var newPosition = new Vector3(x, transform.localPosition.z, z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local Y and Z position of this transform.
		*/

        public static void SetLocalYZ(this Transform transform, float y, float z)
        {
            var newPosition = new Vector3(transform.localPosition.x, y, z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the local X, Y and Z position of this transform.
		*/

        public static void SetLocalXYZ(this Transform transform, float x, float y, float z)
        {
            var newPosition = new Vector3(x, y, z);
            transform.localPosition = newPosition;
        }

        /**
			Sets the position to 0, 0, 0.
		*/

        public static void ResetPosition(this Transform transform)
        {
            transform.position = Vector3.zero;
        }

        /**
			Sets the local position to 0, 0, 0.
		*/

        public static void ResetLocalPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        #endregion

        #region Scale

        /**
			Sets the local X scale of this transform.
		*/

        public static void SetScaleX(this Transform transform, float x)
        {
            var newScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }

        /**
			Sets the local Y scale of this transform.
		*/

        public static void SetScaleY(this Transform transform, float y)
        {
            var newScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
            transform.localScale = newScale;
        }

        /**
			Sets the local Z scale of this transform.
		*/

        public static void SetScaleZ(this Transform transform, float z)
        {
            var newScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
            transform.localScale = newScale;
        }

        /**
			Sets the local X and Y scale of this transform.
		*/

        public static void SetScaleXY(this Transform transform, float x, float y)
        {
            var newScale = new Vector3(x, y, transform.localScale.z);
            transform.localScale = newScale;
        }

        /**
			Sets the local X and Z scale of this transform.
		*/

        public static void SetScaleXZ(this Transform transform, float x, float z)
        {
            var newScale = new Vector3(x, transform.localScale.y, z);
            transform.localScale = newScale;
        }

        /**
			Sets the local Y and Z scale of this transform.
		*/

        public static void SetScaleYZ(this Transform transform, float y, float z)
        {
            var newScale = new Vector3(transform.localScale.x, y, z);
            transform.localScale = newScale;
        }

        /**
			Sets the local X, Y and Z scale of this transform.
		*/

        public static void SetScaleXYZ(this Transform transform, float x, float y, float z)
        {
            var newScale = new Vector3(x, y, z);
            transform.localScale = newScale;
        }

        /**
			Scale this transform in the X direction.
		*/

        public static void ScaleByX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z);
        }

        /**
			Scale this transform in the Y direction.
		*/

        public static void ScaleByY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z);
        }

        /**
			Scale this transform in the Z direction.
		*/

        public static void ScaleByZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * z);
        }

        /**
			Scale this transform in the X, Y direction.
		*/

        public static void ScaleByXY(this Transform transform, float x, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y * y, transform.localScale.z);
        }

        /**
			Scale this transform in the X, Z directions.
		*/

        public static void ScaleByXZ(this Transform transform, float x, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z * z);
        }

        /**
			Scale this transform in the Y and Z directions.
		*/

        public static void ScaleByYZ(this Transform transform, float y, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z * z);
        }

        /**
			Scale this transform in the X and Y directions.
		*/

        public static void ScaleByXY(this Transform transform, float r)
        {
            transform.ScaleByXY(r, r);
        }

        /**
			Scale this transform in the X and Z directions.
		*/

        public static void ScaleByXZ(this Transform transform, float r)
        {
            transform.ScaleByXZ(r, r);
        }

        /**
			Scale this transform in the Y and Z directions.
		*/

        public static void ScaleByYZ(this Transform transform, float r)
        {
            transform.ScaleByYZ(r, r);
        }

        /**
			Scale this transform in the X, Y and Z directions.
		*/

        public static void ScaleByXYZ(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(
                x, y, z);
        }

        /**
			Scale this transform in the X, Y and Z directions.
		*/

        public static void ScaleByXYZ(this Transform transform, float r)
        {
            transform.ScaleByXYZ(r, r, r);
        }


        /**
			Resets the local scale of this transform in to 1 1 1.
		*/

        public static void ResetScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }

        #endregion

        #region FlipScale

        /**
			Negates the X scale.
		*/

        public static void FlipX(this Transform transform)
        {
            transform.SetScaleX(-transform.localScale.x);
        }

        /**
			Negates the Y scale.
		*/

        public static void FlipY(this Transform transform)
        {
            transform.SetScaleY(-transform.localScale.y);
        }

        /**
			Negates the Z scale.
		*/

        public static void FlipZ(this Transform transform)
        {
            transform.SetScaleZ(-transform.localScale.z);
        }

        /**
			Negates the X and Y scale.
		*/

        public static void FlipXY(this Transform transform)
        {
            transform.SetScaleXY(-transform.localScale.x, -transform.localScale.y);
        }

        /**
			Negates the X and Z scale.
		*/

        public static void FlipXZ(this Transform transform)
        {
            transform.SetScaleXZ(-transform.localScale.x, -transform.localScale.z);
        }

        /**
			Negates the Y and Z scale.
		*/

        public static void FlipYZ(this Transform transform)
        {
            transform.SetScaleYZ(-transform.localScale.y, -transform.localScale.z);
        }

        /**
			Negates the X, Y and Z scale.
		*/

        public static void FlipXYZ(this Transform transform)
        {
            transform.SetScaleXYZ(-transform.localScale.z, -transform.localScale.y, -transform.localScale.z);
        }

        /**
			Sets all scale values to the absolute values.
		*/

        public static void FlipPostive(this Transform transform)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                Mathf.Abs(transform.localScale.y),
                Mathf.Abs(transform.localScale.z));
        }

        #endregion

        #region Rotation

        /**
			Rotates the transform around the X axis.
		*/

        public static void RotateAroundX(this Transform transform, float angle)
        {
            var rotation = new Vector3(angle, 0, 0);
            transform.Rotate(rotation);
        }

        /**
			Rotates the transform around the Y axis.
		*/

        public static void RotateAroundY(this Transform transform, float angle)
        {
            var rotation = new Vector3(0, angle, 0);
            transform.Rotate(rotation);
        }

        /**
			Rotates the transform around the Z axis.
		*/

        public static void RotateAroundZ(this Transform transform, float angle)
        {
            var rotation = new Vector3(0, 0, angle);
            transform.Rotate(rotation);
        }

        /**
			Sets the X rotation.
		*/

        public static void SetRotationX(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(angle, 0, 0);
        }

        /**
			Sets the Y rotation.
		*/

        public static void SetRotationY(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        /**
			Sets the Z rotation.
		*/

        public static void SetRotationZ(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        /**
			Sets the local X rotation.
		*/

        public static void SetLocalRotationX(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
        }

        /**
			Sets the local Y rotation.
		*/

        public static void SetLocalRotationY(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        /**
			Sets the local Z rotation.
		*/

        public static void SetLocalRotationZ(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        /**
			Resets the rotation to 0, 0, 0.
		*/

        public static void ResetRotation(this Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }

        /**
			Resets the local rotation to 0, 0, 0.
		*/

        public static void ResetLocalRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        #endregion

        #region All

        /**
			Resets the ;local position, local rotation, and local scale.
		*/

        public static void ResetLocal(this Transform transform)
        {
            transform.ResetLocalRotation();
            transform.ResetLocalPosition();
            transform.ResetScale();

        }

        /**
			Resets the position, rotation, and local scale.
		*/

        public static void Reset(this Transform transform)
        {
            transform.ResetRotation();
            transform.ResetPosition();
            transform.ResetScale();
        }

        #endregion

        #region Children

        public static void DestroyChildren(this Transform transform)
        {
            //Add children to list before destroying
            //otherwise GetChild(i) may bomb out
            var children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }

            foreach (var child in children)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildrenImmediate(this Transform transform)
        {
            //Add children to list before destroying
            //otherwise GetChild(i) may bomb out
            var children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }

            foreach (var child in children)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            var children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }

            return children;
        }

        public static void Sort(this Transform transform, Func<Transform, IComparable> sortFunction)
        {
            var children = transform.GetChildren();
            var sortedChildren = children.OrderBy(sortFunction).ToList();

            for (int i = 0; i < sortedChildren.Count(); i++)
            {
                sortedChildren[i].SetSiblingIndex(i);
            }
        }

        public static void SortAlphabetically(this Transform transform)
        {
            transform.Sort(t => t.name);
        }

        /**
			A lazy enumerable of this objects transform, and all it's children down the hierarchy.
			
			@version_e_1_1
		*/

        public static IEnumerable<Transform> SelfAndAllChildren(this Transform transform)
        {
            var openList = new Queue<Transform>();

            openList.Enqueue(transform);

            while (openList.Any())
            {
                var currentChild = openList.Dequeue();

                yield return currentChild;

                var children = transform.GetChildren();

                foreach (var child in children)
                {
                    openList.Enqueue(child);
                }
            }
        }

        #endregion


        #region recttransform



        /// <summary>
        /// 关于RectTransform的position和localPosition，它们的值根其所属的Canvas的渲染模式有关。
        /// 在Screen Space——Overlay的模式下，由于Canvas的世界尺寸与其像素尺寸在数值上相等，因此其rectTransform的position与其在屏幕空间的坐标在数值上也相等。这种模式下，要获取某个RectTransform的屏幕坐标，直接使用position就可以。
        /// 在Screen Space——Camera的模式和World Space下，RectTransform的渲染与摄像机有关，在获取其屏幕坐标时，需要利用canvas.worldCamera，或者transform.TransformPoint等坐标转换函数进行坐标转换。
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="canvas"></param>
        /// <param name="camera"></param>
        /// <returns></returns>

        public static  Vector3 GetSpacePos(RectTransform rect, Canvas canvas, Camera camera)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return rect.position;
            }
            return camera.WorldToScreenPoint(rect.position);
        }




        public static void SetPosition(this RectTransform rectTransform, Vector3 position)
        {
            rectTransform.localPosition = position;
        }

        public static void SetRotation(this RectTransform rectTransform, Vector3 rotation)
        {
            rectTransform.localRotation = Quaternion.Euler(rotation);
        }

        public static void SetScale(this RectTransform rectTransform, Vector3 scale)
        {
            rectTransform.localScale = scale;
        }

        public static float GetOffsetPivotX(this RectTransform rectTransform)
        {
            return -(rectTransform.pivot.x * rectTransform.GetWidth());
        }

        public static float GetOffsetPivotY(this RectTransform rectTransform)
        {
            return -(rectTransform.pivot.y * rectTransform.GetHeight());
        }

        public static void AnchorToCorners(this RectTransform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            if (transform.parent == null)
                return;

            var parent = transform.parent.GetComponent<RectTransform>();

            Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                              transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

            Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                              transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

            transform.anchorMin = newAnchorsMin;
            transform.anchorMax = newAnchorsMax;
            transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
        }

        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        public static Vector2 GetSize(this RectTransform trans)
        {
            return trans.rect.size;
        }

        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }

        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

        public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void ResetRectTransform(this RectTransform rectTrans)
        {
            rectTrans.anchoredPosition = Vector3.zero;
            rectTrans.localRotation = Quaternion.identity;
            rectTrans.localScale = Vector3.one;

            rectTrans.anchorMin = Vector2.one * 0.5f;
            rectTrans.anchorMax = Vector2.one * 0.5f;
            rectTrans.pivot = Vector2.one * 0.5f;
        }

        public static void FillParent(this RectTransform rectTrans)
        {
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            rectTrans.pivot = Vector2.one * 0.5f;
            rectTrans.offsetMin = Vector2.zero;
            rectTrans.offsetMax = Vector2.zero;
        }
        #endregion

    }
}