using DotHass.Unity.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseAction : GameAction
{
    protected override object DecodePackage(byte[] bytes)
    {
        var data = base.DecodePackage(bytes);
        Debug.Log(data);
        return data;
    }

}