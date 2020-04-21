using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class Factory
    {
        List<RecycleObject> pool = new List<RecycleObject>();
        RecycleObject prefab;
        int defaultPoolSize;

        public Factory(RecycleObject prefab, int defaultPoolSize)
        {
            this.prefab = prefab;
            this.defaultPoolSize = defaultPoolSize;
        }

        void CreatPool()
        {
            for (int i = 0; i < defaultPoolSize; i++)
            {
                RecycleObject obj = GameObject.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }

        public RecycleObject Get()
        {
            if(pool.Count ==0)
            {
                CreatPool();
            }

            int lastIndex = pool.Count - 1;
            RecycleObject obj = pool[lastIndex];
            pool.RemoveAt(lastIndex);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Restore(RecycleObject obj)
        {
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }
}