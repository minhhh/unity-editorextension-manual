using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomEditor (typeof(ExplosionShape)), CanEditMultipleObjects]
public class ExplosionShapeEditor : Editor
{
    // Creating New Explosions
    [MenuItem ("Teaching/Create Explosion Shape")]
    static void CreatePuzzle ()
    {
        string path = EditorUtility.SaveFilePanel ("Create Explosion Shape", "Assets/", "explode.asset", "asset");
        if (path == "")
            return;
    
        path = FileUtil.GetProjectRelativePath (path);
    
        ExplosionShape es = CreateInstance<ExplosionShape> ();
        AssetDatabase.CreateAsset (es, path);
        AssetDatabase.SaveAssets ();
    }

    // Editing Explosions
    public override void OnInspectorGUI ()
    {
        if (Event.current.type == EventType.Layout) {
            return;
        }
    
        Rect position = new Rect (0, 
                            50, // Accounts for Header
                            Screen.width, 
                            Screen.height - 32);
    
        foreach (var item in targets) {
            ExplosionShape pw = item as ExplosionShape;
            Rect usedRect = InspectExplodeShape (position, pw);    
            position.y += usedRect.height;
        }        
    }

    // Editing Single Explosion
    public static Rect InspectExplodeShape (Rect position, ExplosionShape tarPuz)
    {
        GUI.changed = false;
        Rect saveOrig = position;

        // Size
        int newWidth = EditorGUI.IntField (new Rect (position.x, 
                           position.y, 
                           position.width * 0.5f, 
                           EditorGUIUtility.singleLineHeight), 
                           "width", 
                           tarPuz.m_width);
    
        position.y += EditorGUIUtility.singleLineHeight;

        int newHeight = EditorGUI.IntField (new Rect (position.x, 
                            position.y, 
                            position.width * 0.5f,
                            EditorGUIUtility.singleLineHeight), 
                            "height", 
                            tarPuz.m_height);    
        position.y += EditorGUIUtility.singleLineHeight;

        // Resize
        if ((newWidth != tarPuz.m_width) || (newHeight != tarPuz.m_height)) {
            int[] newData = new int[newWidth * newHeight];

            for (int x = 0; (x < newWidth) && (x < tarPuz.m_width); x++)
                for (int y = 0; (y < newHeight) && (y < tarPuz.m_height); y++)
                    newData [x + y * newWidth] = tarPuz.m_data [x + y * tarPuz.m_width];

            tarPuz.m_width = newWidth;
            tarPuz.m_height = newHeight;
            tarPuz.m_data = newData;
        }


        // Setup Block Size and Font
        float xWidth = Mathf.Min (position.width * 0.5f / Mathf.Max (1, tarPuz.m_width),
                           position.height / Mathf.Max (1, tarPuz.m_height));
        GUIStyle myFontStyle = new GUIStyle (EditorStyles.textField);
        myFontStyle.fontSize = Mathf.FloorToInt (xWidth * 0.7f);

        // Edit Blocks
        for (int x = 0; x < tarPuz.m_width; x++) {
            for (int y = 0; y < tarPuz.m_height; y++) {
                tarPuz.m_data [x + y * tarPuz.m_width] = 
          EditorGUI.IntField (new Rect (position.x + xWidth * x,
                    position.y + xWidth * y,
                    xWidth,
                    xWidth), 
                    tarPuz.m_data [x + y * tarPuz.m_width], myFontStyle);
        
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty (tarPuz);
    
        return new Rect (saveOrig.x, saveOrig.y, saveOrig.width, EditorGUIUtility.singleLineHeight + (tarPuz.m_height * xWidth));
    }

    // Preview Explosion
    public override bool HasPreviewGUI ()
    {
        return true;
    }

    public override void OnPreviewGUI (Rect tarRect, GUIStyle background)
    {
        ExplosionShape exShape = target as ExplosionShape;

        // Get Size
        float blockSize = Mathf.Min (tarRect.width / exShape.m_width, tarRect.height / exShape.m_height);
        float offX = (tarRect.width - blockSize * exShape.m_width) / 2 + tarRect.x;
        float offY = (tarRect.height - blockSize * exShape.m_height) / 2 + tarRect.y;

        // Get Max
        int maxExplode = Mathf.Max (exShape.m_data);
        float maxExDiv = 1.0f / (float)maxExplode;

        // Draw Blocks
        for (int x = 0; x < exShape.m_width; ++x)
            for (int y = 0; y < exShape.m_height; ++y)
                if (exShape.m_data [x + y * exShape.m_width] > 0)
                    EditorGUI.DrawRect (new Rect (offX + x * blockSize + 1, 
                        offY + y * blockSize + 1, 
                        blockSize - 2, 
                        blockSize - 2), 
                        new Color (0, 0, 0, exShape.m_data [x + y * exShape.m_width] * maxExDiv));
    
    }

    // For Static Thumbnails
    public override Texture2D RenderStaticPreview (string assetPath, Object[] subAssets, int TexWidth, int TexHeight)
    {
        ExplosionShape exShape = target as ExplosionShape;    
        Texture2D staticPreview = new Texture2D (TexWidth, TexHeight);

        // Get Size
        int blockSize = Mathf.FloorToInt (Mathf.Min (TexWidth / exShape.m_width, TexHeight / exShape.m_height));
        int offX = (TexWidth - blockSize * exShape.m_width) / 2;
        int offY = (TexHeight - blockSize * exShape.m_height) / 2;

        // Get Max
        int maxExplode = Mathf.Max (exShape.m_data);
        float maxExDiv = 1.0f / (float)maxExplode;
    
        // Blank Slate
        Color blankCol = new Color (0, 0, 0, 0);
        Color[] colBlock = new Color[TexWidth * TexHeight];
        for (int i = 0; i < colBlock.Length; ++i)
            colBlock [i] = blankCol;
        staticPreview.SetPixels (0, 0, TexWidth, TexHeight, colBlock);

        // Draw Blocks
        for (int x = 0; x < exShape.m_width; ++x) {
            for (int y = 0; y < exShape.m_height; ++y) {
                if (exShape.m_data [x + y * exShape.m_width] > 0) {
                    int subX = offX + x * blockSize;
                    int subY = TexHeight - (offY + y * blockSize) - blockSize;
                    Color blockColour = new Color (0, 0, 0, exShape.m_data [x + y * exShape.m_width] * maxExDiv);
                    for (int px = 0; px < blockSize; ++px)
                        for (int py = 0; py < blockSize; ++py)
                            staticPreview.SetPixel (subX + px, subY + py, blockColour);
                }
            }
        }
    
        staticPreview.Apply ();
        return staticPreview;
    }
}