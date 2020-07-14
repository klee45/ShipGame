using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Effect;
using static EntityEffect;


public abstract class EffectDict : MonoBehaviour
{
    [SerializeField]
    protected Tag[] immuneTags;

    public ApplyOnceEffectDict<IGeneralEffect> generalEffects;
    public SortedEffectDict<IMovementEffect> movementEffects;
    public SortedEffectDict<ITickEffect> tickEffects;
    public SortedEffectDict<IFixedTickEffect> fixedTickEffects;

    public delegate void DictChange();
    public event DictChange OnChange;

    protected virtual void Awake()
    {
        generalEffects = new ApplyOnceEffectDict<IGeneralEffect>(this);
        movementEffects = new SortedEffectDict<IMovementEffect>(this);
        tickEffects = new SortedEffectDict<ITickEffect>(this);
        fixedTickEffects = new SortedEffectDict<IFixedTickEffect>(this);
        if (immuneTags == null)
        {
            immuneTags = TagHelper.empty;
        }
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

    public abstract class ASortedEffectDict<U> where U : IEffect
    {
        protected EffectDict parent;

        public ASortedEffectDict(EffectDict parent)
        {
            this.parent = parent;
        }

        public abstract List<List<IEffect>> GetAdds();
        public abstract List<IEffect> GetUniques();
        public abstract List<U> GetAll();

        public void AddUpdate<T>(T effect) where T : Effect, U, IEffectUpdates
        {
            if (AllowedTags(effect))
            {
                AddUpdateHelper(effect);
            }
        }
        protected abstract void AddUpdateHelper<T>(T effect) where T : Effect, U, IEffectUpdates;

        public void RemoveUpdate<T>(T effect) where T : Effect, U, IEffectUpdates
        {
            if (AllowedTags(effect))
            {
                RemoveUpdateHelper(effect);
            }
        }
        protected abstract void RemoveUpdateHelper<T>(T effect) where T : Effect, U, IEffectUpdates;

        public void Add<T>(T effect) where T : Effect, U, IEffectAdds
        {
            if (AllowedTags(effect))
            {
                AddHelper(effect);
            }
        }
        protected abstract void AddHelper<T>(T effect) where T : Effect, U, IEffectAdds;

        public void RemoveAdd<T>(T effect) where T : Effect, U, IEffectAdds
        {
            if (AllowedTags(effect))
            {
                RemoveAddHelper(effect);
            }
        }
        protected abstract void RemoveAddHelper<T>(T effect) where T : Effect, U, IEffectAdds;
        
        private bool AllowedTags<T>(T effect) where T : Effect, U
        {
            foreach (Tag tag in effect.GetTags())
            {
                foreach (Tag immune in parent.immuneTags)
                {
                    if (tag == immune)
                    {
                        Destroy(effect);
                        return false;
                    }
                }
            }
            return true;
        }
    }

    /**
    public class EmptyEffectDict<U, V> : ASortedEffectDict<U> where U : IGeneralEffectBase<V> where V : Entity
    {
        private static readonly List<List<IEffect>> emptyAdds = new List<List<IEffect>>();
        private static readonly List<IEffect> emptyUpdates = new List<IEffect>();

        private List<U> all;
        private List<bool> ready;

        private int needsChecking;

        protected V entity;

        public EmptyEffectDict(EffectDict parent) : base(parent)
        {
            entity = parent.GetComponentInParent<V>();
            all = new List<U>();
            ready = new List<bool>();
            needsChecking = 0;
            //Debug.Log(entity);
        }

        public override List<List<IEffect>> GetAdds() { return emptyAdds; }
        public override List<IEffect> GetUniques() { return emptyUpdates; }

        public override List<U> GetAll() { return all; }

        public void Activate(V entity)
        {
            if (needsChecking > 0)
            {
                for (int i = 0; i < ready.Count; i++)
                {
                    if (ready[i])
                    {
                        all[i].Apply(entity);
                        ready[i] = false;
                        needsChecking--;
                    }
                }
            }
        }

        private void AddHelper<T>(T effect) where T : Effect, U
        {
            if (AllowedTags(effect))
            {
                all.Add(effect);
                ready.Add(true);
                needsChecking++;
                effect.OnDestroyEvent += (e) => effect.Cleanup(entity);
            }
        }

        private void RemoveHelper<T>(T effect) where T : Effect, U
        {
            if (AllowedTags(effect))
            {
                for (int i = 0; i < all.Count; i++)
                {
                    // Does this auto-cast?
                    if (all[i].Equals(effect))
                    {
                        all.RemoveAt(i);
                        ready.RemoveAt(i);
                        effect.Cleanup(entity);
                        break;
                    }
                }
            }
        }

        public override void Add<T>(T effect)
        {
            //Debug.Log("Add");
            AddHelper(effect);
        }

        public override void RemoveAdd<T>(T effect)
        {
            //Debug.Log("Remove add");
            RemoveHelper(effect);
        }

        public override bool AddUpdate<T>(T effect)
        {
            //Debug.Log("Add update");
            Debug.LogWarning("General effects should not be unique! They are applied once and removed once");
            AddHelper(effect);
        }

        public override void RemoveUpdate<T>(T effect)
        {
            //Debug.Log("Remove update");
            Debug.LogWarning("General effects should not be unique! They are applied once and removed once");
            RemoveHelper(effect);
        }
    }
    **/

    public class ApplyOnceEffectDict<U> : SortedEffectDict<U> where U : IGeneralEffectBase<Entity>
    {
        private Entity entity;

        public ApplyOnceEffectDict(EffectDict parent) : base(parent)
        {
            entity = parent.GetComponentInParent<Entity>();
        }

        protected override void AddHelper<T>(T effect)
        {
            base.AddHelper(effect);
            effect.Apply(entity);
            effect.OnDestroyEvent += (e) => effect.Cleanup(entity);
        }

        protected override void AddUpdateHelper<T>(T effect)
        {
            base.AddUpdateHelper(effect);
        }

        protected override void AddUpdateFoundExisting<T>(IEffect savedEffect, T effect)
        {
            if (savedEffect is T e)
            {
                e.Cleanup(entity);
                base.AddUpdateFoundExisting(savedEffect, effect);
                e.Apply(entity);
            }
        }

        protected override void AddUpdateNotExisting<T>(T effect)
        {
            base.AddUpdateNotExisting(effect);
            effect.Apply(entity);
            effect.OnDestroyEvent += (e) => effect.Cleanup(entity);
        }
    }

    public class SortedEffectDict<U> : ASortedEffectDict<U> where U : IEffect
    {
        private Dictionary<System.Type, IEffect> updateDict;
        private Dictionary<System.Type, List<IEffect>> addDict;
        private List<U> lst;

        public SortedEffectDict(EffectDict parent) : base(parent)
        {
            updateDict = new Dictionary<System.Type, IEffect>();
            addDict = new Dictionary<System.Type, List<IEffect>>();
            lst = new List<U>();
        }

        public override List<List<IEffect>> GetAdds()
        {
            return addDict.Values.ToList();
        }

        public override List<IEffect> GetUniques()
        {
            return updateDict.Values.ToList();
        }

        public override List<U> GetAll()
        {
            return lst;
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

        protected override void AddUpdateHelper<T>(T effect)
        {
            //Debug.Log(type);
            //Debug.Log(updateDict.Count);
            if (updateDict.TryGetValue(effect.GetType(), out IEffect savedEffect))
            {
                //Debug.Log("Existing");
                AddUpdateFoundExisting(savedEffect, effect);
            }
            else
            {
                //Debug.Log("Not existing");
                AddUpdateNotExisting(effect);
            }
            parent.InvokeChange();
        }

        protected virtual void AddUpdateFoundExisting<T>(IEffect savedEffect, T effect) where T : Effect, U, IEffectUpdates
        {
            effect.UpdateEffect(savedEffect, out bool didReplace);
            if (didReplace)
            {
                RemoveUpdateCallback(savedEffect);
                ListInsert(effect);
                effect.OnDestroyEvent += RemoveUpdateCallback;
            }
            else
            {
                Destroy(effect);
            }
        }

        protected virtual void AddUpdateNotExisting<T>(T effect) where T : Effect, U, IEffectUpdates
        {
            updateDict.Add(effect.GetType(), effect);
            ListInsert(effect);
            effect.OnDestroyEvent += RemoveUpdateCallback;
        }

        protected override void RemoveUpdateHelper<T>(T effect)
        {
            RemoveUpdateCallback(effect);
        }

        private void RemoveUpdateCallback(IEffect effect)
        {
            RemoveAsGeneric(effect);
            updateDict.Remove(effect.GetType());
            effect.OnDestroyEvent -= RemoveUpdateCallback;
            parent.InvokeChange();
        }

        protected override void AddHelper<T>(T effect)
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
            effect.OnDestroyEvent += RemoveAddCallback;
            parent.InvokeChange();
        }

        protected override void RemoveAddHelper<T>(T effect)
        {
            RemoveAddCallback(effect);
        }

        private void RemoveAddCallback(IEffect effect)
        {
            System.Type type = effect.GetType();
            if (addDict.TryGetValue(type, out List<IEffect> effectList))
            {
                int count = effectList.Count;
                if (count == 1)
                {
                    RemoveAsGeneric(effect);
                    addDict.Remove(type);
                    effect.OnDestroyEvent -= RemoveAddCallback;
                }
                else if (count <= 0)
                {
                    Debug.LogWarning("Effect add dict had no more elements when trying to remove");
                }
                else
                {
                    RemoveAsGeneric(effect);
                    effectList.Remove(effect);
                    effect.OnDestroyEvent -= RemoveAddCallback;
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
