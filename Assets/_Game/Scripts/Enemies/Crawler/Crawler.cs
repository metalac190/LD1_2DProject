using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Actor
{
    [Header("Dependencies")]
    [SerializeField]
    private CrawlerData _data;

    public CrawlerData Data => _data;
}
