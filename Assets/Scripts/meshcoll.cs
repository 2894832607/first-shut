using UnityEngine;
using UnityEditor;

public class  meshcoll  : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Add Mesh Colliders")]
    static void AddMeshCollidersToAll()
    {
        // ��ȡ���������� MeshRenderer ���
        MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // ��ȡ�� MeshRenderer ������ MeshFilter
            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                // ����Ƿ��Ѿ��� MeshCollider
                if (meshFilter.gameObject.GetComponent<MeshCollider>() == null)
                {
                    // ��� MeshCollider
                    meshFilter.gameObject.AddComponent<MeshCollider>();
                    Debug.Log($"Added MeshCollider to {meshFilter.gameObject.name}");
                }
            }
        }
    }
#endif
}