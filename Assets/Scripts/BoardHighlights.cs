using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    public static BoardHighlights Instance { set; get; }

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    //Creates a new highlight prefab
    private GameObject GetHighlightObject()
    {
        var go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    //Gets a three dimensional array and highlights allowed moves
    public void HighlightAllowedMoves(bool[,,] moves, Vector3 board1Offset, Vector3 board2Offset, Vector3 board3Offset)
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 8; j++)
        for (var k = 0; k < 8; k++)
            if (moves[k, i, j])
            {
                var go = GetHighlightObject();
                go.SetActive(true);
                var vectorWithOffset = new Vector3(k, 0, j);
                switch (i)
                {
                    case 0:
                        vectorWithOffset += board1Offset;
                        break;
                    case 1:
                        vectorWithOffset += board2Offset;
                        break;
                    case 2:
                        vectorWithOffset += board3Offset;
                        break;
                }

                go.transform.position = vectorWithOffset;
            }
    }

    //Clears all highlights
    public void HideHighlights()
    {
        foreach (var go in highlights) go.SetActive(false);
    }
}