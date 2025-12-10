#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CanEditMultipleObjects, CustomEditor(typeof(CreateTexture))]
public class CreateTextureInspector : Editor
{
    CreateTexture myTarget;

    public void OnEnable()
    {
        myTarget = (CreateTexture)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("New Texture"))
        {
            // Validate that the target has required components
            if (myTarget.GetComponent<Renderer>() == null && myTarget.GetComponent<SpriteRenderer>() == null)
            {
                EditorGUILayout.HelpBox("This GameObject needs a Renderer or SpriteRenderer component to generate a texture.", MessageType.Error);
                return;
            }

            myTarget.GenerateTexture((int)myTarget.textureSize, myTarget.filterMode, myTarget.wrapMode);
        }
    }
}
#endif
