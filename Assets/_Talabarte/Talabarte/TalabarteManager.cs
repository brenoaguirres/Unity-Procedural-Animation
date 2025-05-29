using UnityEngine;
using System;
using System.Collections.Generic;

public class TalabarteManager : MonoBehaviour
{
    public List<TalabarteLadderSnap> talabarteLadderSnaps;
    public Talabarte talabarte1;
    public Talabarte talabarte2;

    public Action OnBothTalabarteSnapped;
    public Action OnFinishedTalabarte;

    private int ladderSnapIndex = 0;
    private int snapCount = 0;

    private void Start()
    {
        foreach (var snap in talabarteLadderSnaps)
            snap.gameObject.SetActive(false);

        ActivateCurrentLadderSnaps();

        talabarte1.OnLadderSnap += OnTalabarteLadderSnapped;
        talabarte2.OnLadderSnap += OnTalabarteLadderSnapped;
    }

    private void ActivateCurrentLadderSnaps()
    {
        if (ladderSnapIndex < talabarteLadderSnaps.Count)
        {
            talabarteLadderSnaps[ladderSnapIndex].gameObject.SetActive(true);
            if (ladderSnapIndex + 1 < talabarteLadderSnaps.Count)
                talabarteLadderSnaps[ladderSnapIndex + 1].gameObject.SetActive(true);
        }
    }

    public void OnTalabarteLadderSnapped()
    {
        snapCount++;
        if (snapCount >= 2)
        {
            OnBothTalabarteSnapped?.Invoke();
            snapCount = 0;
            AdvanceTalabarteStep();
        }
    }

    public void AdvanceTalabarteStep()
    {
        if (ladderSnapIndex < talabarteLadderSnaps.Count)
        {
            talabarteLadderSnaps[ladderSnapIndex].gameObject.SetActive(false);
            if (ladderSnapIndex + 1 < talabarteLadderSnaps.Count)
                talabarteLadderSnaps[ladderSnapIndex + 1].gameObject.SetActive(false);
        }

        ladderSnapIndex += 2;

        if (ladderSnapIndex >= talabarteLadderSnaps.Count)
        {
            OnFinishedTalabarte?.Invoke();
        }
        else
        {
            ActivateCurrentLadderSnaps();
        }
    }
}
