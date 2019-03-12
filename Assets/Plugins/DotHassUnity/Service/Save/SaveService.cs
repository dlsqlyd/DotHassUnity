using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFramework
{
    public class SaveService
    {

        public void  Save<T>(string key,T value)
        {
            SaveGame.Save<T>(key, value);
        }


        public void Save<T>(string key, T value, bool encode)
        {
            SaveGame.Save<T>(key, value, encode);
        }


        public T Load<T>(string key)
        {
            return SaveGame.Load<T>(key);
        }

        public T Load<T>(string key, T defaultValue)
        {
            return SaveGame.Load<T>(key, defaultValue);
        }

    }
}
