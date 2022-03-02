using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Humanoid> players = new List<Humanoid>();
    GameObject playersParent;

    private void Start()
    {
        playersParent = GameObject.Find("Players");
        foreach (Transform child in playersParent.transform)
        {
            players.Add(child.GetComponent<Humanoid>());
        }
    }
}
