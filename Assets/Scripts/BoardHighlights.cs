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

    //Creates a new highlight prefab
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

    //Gets a three dimensional array and highlights allowed moves
    public void HighlightAllowedMoves(bool[,,] moves,Vector3 board1Offset,Vector3 board2Offset,Vector3 board3Offset)
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
                        
                        go.transform.position =  vectorWithOffset;
                    }
                }
            }
        }
    }

    //Clears all highlights
    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
        {
            go.SetActive(false);
        }
    }
}
