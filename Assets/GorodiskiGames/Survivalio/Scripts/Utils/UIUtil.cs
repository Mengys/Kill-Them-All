using System;
using  UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class UIUtil
    {
        public static Color GetDisabledColor(Color c)
        {
            return new Color(c.r, c.g, c.b, .5f);
        }

        public static Vector2 GetContentSize(RectTransform content, GridLayoutGroup layoutGroup)
        {
            var items = content.childCount;
            var itemsPerRow = layoutGroup.constraintCount;
            var rows = (int)Mathf.Ceil((float)items / itemsPerRow);

            var paddingTop = layoutGroup.padding.top;
            var paddingBottom = layoutGroup.padding.bottom;
            var spacing = layoutGroup.spacing.y;

            var ySlotSize = layoutGroup.cellSize.y;
            var ySize = paddingTop + (rows * ySlotSize) + ((rows - 1) * spacing) + paddingBottom;

            return new Vector2(content.sizeDelta.x, ySize);
        }
    }
}