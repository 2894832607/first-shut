using UnityEngine;
using UnityEditor;

public class  meshcoll  : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Add Mesh Colliders")]
    static void AddMeshCollidersToAll()
    {
        // 获取场景中所有 MeshRenderer 组件
        MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // 获取与 MeshRenderer 关联的 MeshFilter
            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                // 检查是否已经有 MeshCollider
                if (meshFilter.gameObject.GetComponent<MeshCollider>() == null)
                {
                    // 添加 MeshCollider
                    meshFilter.gameObject.AddComponent<MeshCollider>();
                    Debug.Log($"Added MeshCollider to {meshFilter.gameObject.name}");
                }
            }
        }
    }
#endif
}