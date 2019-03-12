using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
public class SpriteToTexture
{ /// <summary>
  /// 切割Sprite导出单个对象
  /// 图片需要设置可读
  /// </summary>
    [MenuItem("Assets/Create/SpriteSplit2Export", false, 12)]
    public static void SpriteChildToExport()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            //获得选择对象路径;
            string spritePath = AssetDatabase.GetAssetPath(Selection.objects[i]);
            //所有子Sprite对象;
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).OfType<Sprite>().ToArray();
            if (sprites.Length < 1)
            {
                EditorUtility.DisplayDialog("错误", "当前选择文件不是Sprite!", "确认");
                Debug.LogError("Sorry,There is not find sprite!");
                return;
            }
            string[] splitSpritePath = spritePath.Split(new char[] { '/' });
            //文件夹路径 通过完整路径再去掉文件名称即可;
            string fullFolderPath = Inset(spritePath, 0, splitSpritePath[splitSpritePath.Length - 1].Length + 1) + "/" + Selection.objects[i].name;
            //同名文件夹;
            string folderName = Selection.objects[i].name;
            string adjFolderPath = InsetFromEnd(fullFolderPath, Selection.objects[i].name.Length + 1);
            //验证路径;
            if (!AssetDatabase.IsValidFolder(fullFolderPath))
            {
                AssetDatabase.CreateFolder(adjFolderPath, folderName);
            }

            for (int j = 0; j < sprites.Length; j++)
            {   //进度条;
                string pgTitle = (i + 1).ToString() + "/" + Selection.objects.Length + " 开始导出Sprite";
                string info = "当前Srpte: " + j + "->" + sprites[j].name;
                float nowProgress = (float)j / (float)sprites.Length;
                EditorUtility.DisplayProgressBar(pgTitle, info, nowProgress);
                //创建Texture;

                byte[] data = ConvertSpriteToTexture(sprites[j]);
                //判断保存路径;
                string savePath = fullFolderPath + "/" + sprites[j].name + ".png";
                //生成png;
                File.WriteAllBytes(savePath, data);
            }
            //释放进度条;
            EditorUtility.ClearProgressBar();
            //刷新资源，不然导出后你以为没导出，还要手动刷新才能看到;
            AssetDatabase.Refresh();
        }

    }


    public static byte[] ConvertSpriteToTexture(Sprite sprite)
    {
        Texture2D tex = sprite.texture;
        Rect r = sprite.textureRect;
        Texture2D subtex = tex.CropTexture((int)r.x, (int)r.y, (int)r.width, (int)r.height);
        byte[] data = subtex.EncodeToPNG();
        return data;
    }


    /// <summary>
    /// 截取路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="leftIn">左起点</param>
    /// <param name="rightIn">右起点</param>
    /// <returns></returns>
    public static string Inset(string path, int leftIn, int rightIn)
    {
        return path.Substring(leftIn, path.Length - rightIn - leftIn);
    }

    /// <summary>
    /// 截取路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="inset"></param>
    /// <returns></returns>
    public static string InsetFromEnd(string path, int inset)
    {
        return path.Substring(0, path.Length - inset);
    }
}
static class Texture2DExtensions
{
    /**
     * CropTexture
     * 
     * Returns a new texture, composed of the specified cropped region.
     */
    public static Texture2D CropTexture(this Texture2D pSource, int left, int top, int width, int height)
    {
        if (left < 0)
        {
            width += left;
            left = 0;
        }
        if (top < 0)
        {
            height += top;
            top = 0;
        }
        if (left + width > pSource.width)
        {
            width = pSource.width - left;
        }
        if (top + height > pSource.height)
        {
            height = pSource.height - top;
        }

        if (width <= 0 || height <= 0)
        {
            return null;
        }

        Color[] aSourceColor = pSource.GetPixels(0);

        //*** Make New
        Texture2D oNewTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = width * height;
        Color[] aColor = new Color[xLength];

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            int sourceIndex = (y + top) * pSource.width + left;
            for (int x = 0; x < width; x++)
            {
                aColor[i++] = aSourceColor[sourceIndex++];
            }
        }

        //*** Set Pixels
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();

        //*** Return
        return oNewTex;
    }
}