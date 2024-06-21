using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Collectable
{
    [SerializeField] int diamondValue = 1;

    protected override void Collected()
    {
        GameManager.MyInstance.AddDiamonds(diamondValue);
        Destroy(this.gameObject);
    }
}
