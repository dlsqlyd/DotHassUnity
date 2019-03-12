using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HFramework.Net
{
    public class ActionFactory
    {
    
        public static Func<string,int,GameAction> CreateAction;

        static ActionFactory()
        {
            CreateAction = DefaultCreateAction;
        }

        public static GameAction DefaultCreateAction(string channelName, int actionId)
        {
            return new GameAction();
        }


        private static readonly Hashtable lookupType = new Hashtable();
        private static readonly string ActionFormat = "{0}Action{1}";
        public static GameAction ActionIDCreateAction(string channelName, int actionId)
        {
            GameAction gameAction = null;
            try
            {
                string name = string.Format(ActionFormat, channelName, actionId);
                var type = (Type)lookupType[name];
                lock (lookupType)
                {
                    if (type == null)
                    {
                        type = Type.GetType(name);
                        lookupType[name] = type;
                    }
                }
                if (type != null)
                {
                    gameAction = Activator.CreateInstance(type) as GameAction;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("GameAction create error:" + ex);
            }
            return gameAction;
        }

    }
}
