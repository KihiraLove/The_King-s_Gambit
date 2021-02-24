using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance { set; get; }
    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    
    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void HighlightAllowedMoves(bool[,,] moves,int boardOffsetY,int boardOffsetZ)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (moves[k, i, j])
                    {
                        GameObject go = GetHighlightObject();
                        go.SetActive(true);
                        go.transform.position = new Vector3(k, i * boardOffsetY, j + i * boardOffsetZ);
                    }
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
        {
            go.SetActive(false);
        }
    }
}
