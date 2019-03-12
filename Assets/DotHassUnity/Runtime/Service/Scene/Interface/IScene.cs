using UnityEngine;
using System.Collections;


namespace DotHass.Unity
{
    public interface IScene
    {
        string Name { get; set; }
        bool StartUpComplete { get; set; }
        IEnumerator Enter();

        IEnumerator Exit();
    }
}

