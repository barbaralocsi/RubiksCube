using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] List<GameObject> hints;
    int active = -1;

    void Start()
    {
        hints = new List<GameObject>();
        foreach (Transform childTransform in transform)
        {
            hints.Add(childTransform.gameObject);
        }
    }

    public void ShowRandomHint()
    {
        //Only show hint if there is no active currently
        if (active < 0)
        {
            int random = Random.Range(0, hints.Count);
            hints[random].SetActive(true);
            active = random;
        }
    }

    public void HideHint()
    {
        if (active >= 0)
        {
            hints[active].SetActive(false);
        }
        active = -1;
    }
}
