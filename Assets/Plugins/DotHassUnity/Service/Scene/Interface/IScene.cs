using UnityEngine;
using System.Collections;


namespace HFramework
{
    public interface IScene
    {
        string Name { get; set; }
        bool StartUpComplete { get; set; }
        IEnumerator Enter();

        IEnumerator Exit();
    }
}

