using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity.Net
{
    public  class GameAction
    {
        public Dictionary<string, string> Params { get; set; }

        public Action<object> Callback;

        protected virtual object DecodePackage(byte[] bytes)
        {
            var data = Encoding.UTF8.GetString(bytes);
            return data;
        }

        public virtual void Handler(byte[] bytes)
        {
            var data = this.DecodePackage(bytes);
            try
            {
                Callback?.Invoke(data);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("Action {0} callback process error:{1}", this.GetType().Name, ex));
            }
        }


        public virtual void HandlerError(ErrorInfo error)
        {
            try
            {
                Callback?.Invoke(error);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("Action {0} callback process error:{1}", this.GetType().Name, ex));
            }
        }
    }
}
