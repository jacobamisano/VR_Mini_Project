using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
    public void vanish()
    {
        gameObject.SetActive(false);
    }

    public void reappear()
    {
        gameObject.SetActive(true);
    }
}
