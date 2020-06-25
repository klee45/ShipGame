using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Effect;
using static GeneralEffect;


public abstract class EffectDict : MonoBehaviour
{
    [SerializeField]
    protected Tag[] immuneTags;

    public SortedEffectDict<IGeneralEffect> generalEffects;
    public SortedEffectDict<IMovementEffect> movementEffects;
    public SortedEffectDict<ITickEffect> tickEffects;

    public delegate void DictChange();
    public event DictChange OnChange;

    protected virtual void Awake()
    {
        generalEffects = new SortedEffectDict<IGeneralEffect>(this);
        movementEffects = new SortedEffectDict<IMovementEffect>(this);
        tickEffects = new SortedEffectDict<ITickEffect>(this);
    }

    private void InvokeChange()
    {
        OnChange?.Invoke();
    }

    public void SetImmuneTags(Tag[] tags)
    {
        this.immuneTags = tags;
    }

    public Tag[] GetTags()
    {
        return immuneTags;
    }

    public virtual List<List<IEffect>> GetAddEffects()
    {
        List<List<IEffect>> lst = new List<List<IEffect>>();
        lst.AddRange(generalEffects.GetAdds());
        lst.AddRange(movementEffects.GetAdds());
        lst.AddRange(tickEffects.GetAdds());
        return lst;
    }

    public virtual List<IEffect> GetUpdateEffects()
    {
        List<IEffect> lst = new List<IEffect>();
        lst.AddRange(generalEffects.GetUniques());
        lst.AddRange(movementEffects.GetUniques());
        lst.AddRange(tickEffects.GetUniques());
        return lst;
    }

    public interface IEffectAdds { }
    public interface IEffectUpdates
    {
        IEffect UpdateEffect(IEffect effect, out bool didReplace);
    }

    public class SortedEffectDict<U> where U : IEffect
    {
        private EffectDict parent;

        private Dictionary<System.Type, IEffect> updateDict;
        private Dictionary<System.Type, List<IEffect>> addDict;
        private List<U> lst;

        public SortedEffectDict(EffectDict parent)
        {
            this.parent = parent;
            updateDict = new Dictionary<System.Type, IEffect>();
            addDict = new Dictionary<System.Type, List<IEffect>>();
            lst = new List<U>();
        }

        public List<List<IEffect>> GetAdds()
        {
            return addDict.Values.ToList();
        }

        public List<IEffect> GetUniques()
        {
            return updateDict.Values.ToList();
        }

        public List<U> GetAll()
        {
            return lst;
        }

        private bool AllowedTags(IEffect effect)
        {
            foreach (Tag tag in effect.GetTags())
            {
                foreach (Tag immune in parent.immuneTags)
                {
                    if (tag == immune)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ListInsert(U effect)
        {
            int i = 0;
            int insertPriority = effect.GetPriority();
            foreach (U e in lst)
            {
                if (e.GetPriority() > insertPriority)
                {
                    lst.Insert(i, effect);
                    return;
                }
                i++;
            }
            lst.Add(effect);
        }
        
        public void AddUpdate<T>(T effect) where T : U, IEffectUpdates
        {
            if (AllowedTags(effect))
            {
                System.Type type = effect.GetType();
                if (updateDict.TryGetValue(type, out IEffect savedEffect))
                {
                    effect.UpdateEffect(savedEffect, out bool didReplace);
                    if (didReplace)
                    {
                        RemoveUpdateHelper(savedEffect);
                        ListInsert(effect);
                        effect.OnDestroyEvent += RemoveUpdateHelper;
                    }
                }
                else
                {
                    updateDict.Add(type, effect);
                    ListInsert(effect);
                    effect.OnDestroyEvent += RemoveUpdateHelper;
                }
                parent.InvokeChange();
            }
        }

        public void RemoveUpdate<T>(T effect) where T : U, IEffectUpdates
        {
            if (AllowedTags(effect))
            {
                RemoveUpdateHelper(effect);
            }
        }

        private void RemoveUpdateHelper(IEffect effect)
        {
            RemoveAsGeneric(effect);
            updateDict.Remove(effect.GetType());
            effect.OnDestroyEvent -= RemoveUpdateHelper;
            parent.InvokeChange();
        }

        public void Add<T>(T effect) where T : U, IEffectAdds
        {
            if (AllowedTags(effect))
            {
                System.Type type = effect.GetType();
                if (addDict.TryGetValue(type, out List<IEffect> effects))
                {
                    effects.Add(effect);
                }
                else
                {
                    List<IEffect> lst = new List<IEffect>();
                    lst.Add(effect);
                    addDict.Add(type, lst);
                }
                ListInsert(effect);
                effect.OnDestroyEvent += RemoveAddHelper;
                parent.InvokeChange();
            }
        }

        public void RemoveAdd<T>(T effect) where T : U, IEffectAdds
        {
            if (AllowedTags(effect))
            {
                RemoveAddHelper(effect);
            }
        }

        private void RemoveAddHelper(IEffect effect)
        {
            System.Type type = effect.GetType();
            if (addDict.TryGetValue(type, out List<IEffect> effectList))
            {
                int count = effectList.Count;
                if (count == 1)
                {
                    RemoveAsGeneric(effect);
                    addDict.Remove(type);
                    effect.OnDestroyEvent -= RemoveAddHelper;
                }
                else if (count <= 0)
                {
                    Debug.LogWarning("Effect add dict had no more elements when trying to remove");
                }
                else
                {
                    RemoveAsGeneric(effect);
                    effectList.Remove(effect);
                    effect.OnDestroyEvent -= RemoveAddHelper;
                }
            }
            parent.InvokeChange();
        }

        private void RemoveAsGeneric(IEffect effect)
        {
            if (effect is U e)
            {
                lst.Remove(e);
            }
        }
    }

    /*
    public class EffectContainer<U> where U : IEffect
    {
        private SortedList<int, U> lst = new SortedList<int, U>();
        public void Add<T>(T effect) where T : U
        {
            lst.Add(effect.GetPriority(), effect);
        }
    }
    */

    /*
    public class EffectContainer<U> where U : IEffect
    {
        private class CountedEffect
        {
            private U effect;
            private int count;
            public CountedEffect(U effect)
            {
                SetEffect(effect);
            }
            public void SetEffect(U effect)
            {
                this.effect = effect;
                count++;
            }
            public void UpdateEffect(U effect)
            {
                this.effect.UpdateEffect(effect);
                count++;
            }
            public U GetEffect()
            {
                return effect;
            }
            public bool CheckRemove()
            {
                return --count <= 0;
            }
        }

        private List<U> effects;
        private Dictionary<string, CountedEffect> uniqueEffects;
        public EffectContainer()
        {
            effects = new List<U>();
            uniqueEffects = new Dictionary<string, CountedEffect>();
        }

        public void Sort()
        {
            effects.OrderBy(p => -p.GetPriority());
        }

        public void Set(U effect, string name)
        {
            if (uniqueEffects.TryGetValue(name, out CountedEffect countedEffect))
            {
                countedEffect.SetEffect(effect);
            }
            else
            {
                CreateNamedEffectEntry(effect, name);
            }
        }

        public void Update(U effect, string name)
        {
            if (uniqueEffects.TryGetValue(name, out CountedEffect countedEffect))
            {
                countedEffect.UpdateEffect(effect);
            }
            else
            {
                CreateNamedEffectEntry(effect, name);
            }
        }

        private void CreateNamedEffectEntry(U effect, string name)
        {
            uniqueEffects[name] = new CountedEffect(effect);
        }

        public void Remove(U effect, string name)
        {
            if (uniqueEffects.TryGetValue(name, out CountedEffect countedEffect))
            {
                if (countedEffect.CheckRemove())
                {
                    uniqueEffects.Remove(name);
                }
            }
            else
            {
                Debug.LogWarning(string.Format("Tried to remove unique effect with name \"{0}\" that didn't exist!", name));
            }
        }

        public void Add(U effect)
        {
            effects.Add(effect);
        }

        public void Remove(U effect)
        {
            effects.Remove(effect);
        }

        public List<U> GetAll()
        {
            return effects;
        }
    }
    */
}
