using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HFramework
{
    public class CoroutineService: MonoBehaviour, ICoroutine
    {
   
        private Dictionary<string, List<Coroutine>> mCoroutineDic = new Dictionary<string, List<Coroutine>>();

        /// <summary>
		/// Executes the coroutine.
		/// </summary>
		public void Execute(IEnumerator enumarator, string name)
        {
            var coroutine = StartCoroutine(enumarator);

            if (mCoroutineDic.ContainsKey(name))
            {
                mCoroutineDic[name].Add(coroutine);
            }
            else
            {
                mCoroutineDic.Add(name, new List<Coroutine>());
            }
        }

        /// <summary>
        /// Kill the name of the all coroutine with.
        /// </summary>
        public void Kill(string name)
        {
            if (mCoroutineDic.ContainsKey(name))
            {
                var coList = mCoroutineDic[name];
                for (int i = 0; i < coList.Count; i++)
                {
                    if (null != coList[i])
                    {
                        StopCoroutine(coList[i]);
                        coList[i] = null;
                    }
                }
                coList.Clear();
            }
        }


        public void OnDestroy()
        {
            StopAllCoroutines();
            mCoroutineDic.Clear();
        }
    }
}
