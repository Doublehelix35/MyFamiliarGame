// Base script from https://www.youtube.com/watch?v=LnAoD7hgDxw
// Makin' stuff look good (Youtube)

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
}
