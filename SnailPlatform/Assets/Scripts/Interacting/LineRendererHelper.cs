using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EW: Klasa ustawia pozycje rysowania linii line renderera
/// </summary>
public class LineRendererHelper : MonoBehaviour
{
    public List<Transform> TransformList;
    public LineRenderer LineRenderer;

    private void Start()
    {
        LineRenderer.positionCount = TransformList.Count;
    }

    void Update()
    {
        for (int i = 0; i < TransformList.Count; i++)
        {
            LineRenderer.SetPosition(i, TransformList[i].position);
        }
    }
}