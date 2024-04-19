using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [field: SerializeField]
    public string ItemName {  get; private set; }
}
