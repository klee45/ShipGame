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
    private Entity entity;

    public TempSortedEffectDict<IGeneralEffect, IGeneralEffectCase> generalEffects;
    public TempSortedEffectDict<IMovementEffect, IMovementEffectCase> movementEffects;
    public TempSortedEffectDict<ITickEffect, ITickEffectCase> tickEffects;
    public TempSortedEffectDict<IFixedTickEffect, IFixedTickEffectCase> fixedTickEffects;

    public delegate void DictChange();
    public event DictChange OnChange;

    protected virtual void Awake()
    {
        this.entity = GetComponentInParent<Entity>();
        generalEffects = new TempSortedEffectDict<IGeneralEffect, IGeneralEffectCase>(this);
        movementEffects = new TempSortedEffectDict<IMovementEffect, IMovementEffectCase>(this);
        tickEffects = new TempSortedEffectDict<ITickEffect, ITickEffectCase>(this);
        fixedTickEffects = new TempSortedEffectDict<IFixedTickEffect, IFixedTickEffectCase>(this);
        if (immuneTags == null)
        {
            immuneTags = TagHelper.empty;
        }
    }

    private void InvokeChange()
    {
        OnChange?.Invoke();
    }

    public Entity GetEntity()
    {
        return entity;
    }

    public void SetImmuneTags(Tag[] tags)
    {
        this.immuneTags = tags;
    }

    public Tag[] GetTags()
    {
        return immuneTags;
    }

    /*
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
    */
    
    /*
    public interface IEffectAdds<V> where V : IEffect
    {
    }

    public interface IEffectUpdates<V> where V : IEffect
    {
        V UpdateEffect(V effect, out bool didReplace);
    }
    */

    public delegate void PriorityChangeEvent(int previous);
    public delegate void EffectCaseFinishedEvent();

    public interface IEffectCase<U> where U : IEffect
    {
        event EffectCaseFinishedEvent OnEffectCaseFinished;
        event PriorityChangeEvent OnPriorityChange;
        void Update<V>(V effect) where V : Effect, U;
        int GetPriority();
    }


    public interface IFixedTickEffectCase : IEffectCase<IFixedTickEffect>
    {
        void FixedTick(float timescale);
    }
    public abstract class AFixedTickEffectCase<W> : AEffectCase<IFixedTickEffect, W>, IFixedTickEffectCase where W : Effect, IFixedTickEffect
    {
        public AFixedTickEffectCase(EffectCaseType type) : base(type)
        {
        }

        public void FixedTick(float timescale)
        {
            foreach (IFixedTickEffect t in effectsList.GetAll())
            {
                t.FixedTick(timescale);
            }
        }
    }


    public interface ITickEffectCase : IEffectCase<ITickEffect>
    {
        void Tick(float timescale);
    }
    public class TickEffectCase<W> : AEffectCase<ITickEffect, W>, ITickEffectCase where W : Effect, ITickEffect
    {
        public TickEffectCase(EffectCaseType type) : base(type)
        {
        }

        public void Tick(float timescale)
        {
            foreach (ITickEffect t in effectsList.GetAll())
            {
                t.Tick(timescale);
            }
        }
    }
   

    public interface IMovementEffectCase : IEffectCase<IMovementEffect>
    {
        Vector3 GetMovement(float deltaTime);
    }
    public abstract class AMovementEffectCase<W> : AEffectCase<IMovementEffect, W>, IMovementEffectCase where W : Effect, IMovementEffect
    {
        public AMovementEffectCase(EffectCaseType type) : base(type)
        {
        }

        public abstract Vector3 GetMovement(float deltaTime);
    }


    public interface IGeneralEffectCase : IEffectCase<IGeneralEffect>
    {
        void Apply(Entity entity);
        void Cleanup(Entity entity);
    }
    public abstract class AGeneralEffectCase<W> : AEffectCase<IGeneralEffect, W>, IGeneralEffectCase where W : Effect, IGeneralEffect
    {
        protected Entity affectedEntity;

        public AGeneralEffectCase(Entity e, EffectCaseType type) : base(type)
        {
            affectedEntity = e;
        }

        public abstract void Apply(Entity entity);
        public abstract void Cleanup(Entity entity);

        public override void Update<V>(V effect)
        {
            Cleanup(affectedEntity);
            base.Update(effect);
            Apply(affectedEntity);
        }

        protected override void Remove<V>(V effect)
        {
            Cleanup(affectedEntity);
            base.Remove(effect);
            Apply(affectedEntity);
        }
    }

    public enum EffectCaseType
    {
        SingleKeep,
        SingleReplace,
        Multiple
    }

    public interface IEffectList<U, W> where U : IEffect where W : Effect, U
    {
        IEnumerable<W> GetAll();
        W GetFirst();
        W GetLast();
        void Update<V>(V effect) where V : Effect, U;
        void Remove<V>(V effect) where V : Effect, U;
        int GetPriority();
    }

    private class EffectSingleKeep<U, W> : AEffectSingle<U, W> where U : IEffect where W : Effect, U
    {
        public EffectSingleKeep(AEffectCase<U, W> parent) : base(parent)
        {
        }

        public override void Update<V>(V effect)
        {
            if (effect is W e)
            {
                int oldPriority = this.effect.GetPriority();
                if (e.GetPriority() != oldPriority)
                {
                    parent.PriorityChangeInvoke(oldPriority);
                }
                this.effect = e;
            }
        }
    }

    private class EffectSingleReplace<U, W> : AEffectSingle<U, W> where U : IEffect where W : Effect, U
    {
        public EffectSingleReplace(AEffectCase<U, W> parent) : base(parent)
        {
        }

        public override void Update<V>(V effect)
        {
            if (effect is W e)
            {
                int oldPriority = this.effect.GetPriority();
                if (e.GetPriority() != oldPriority)
                {
                    parent.PriorityChangeInvoke(oldPriority);
                }
                this.effect = e;
            }
        }
    }

    private abstract class AEffectSingle<U, W> : IEffectList<U, W> where U : IEffect where W : Effect, U
    {
        protected W effect;
        protected AEffectCase<U, W> parent;

        public AEffectSingle(AEffectCase<U, W> parent)
        {
            this.parent = parent;
        }

        public int GetPriority()
        {
            return effect.GetPriority();
        }

        public IEnumerable<W> GetAll()
        {
            yield return effect;
        }

        public W GetFirst()
        {
            return effect;
        }

        public W GetLast()
        {
            return effect;
        }

        public abstract void Update<V>(V effect) where V : Effect, U;

        public void Remove<V>(V effect) where V : Effect, U
        {
            if (effect is W e)
            {
                if (this.effect == e)
                {
                    parent.EffectCaseFinishedInvoke();
                }
            }
        }
    }

    private class EffectList<U, W> : IEffectList<U, W> where U : IEffect where W : Effect, U
    {
        private List<W> effects;
        private int priority = 0;
        private AEffectCase<U, W> parent;
        
        public EffectList(AEffectCase<U, W> parent)
        {
            this.parent = parent;
        }

        public int GetPriority()
        {
            return priority;
        }

        public IEnumerable<W> GetAll()
        {
            return effects;
        }

        public W GetFirst()
        {
            return effects.First();
        }

        public W GetLast()
        {
            return effects.Last();
        }

        public void Update<V>(V effect) where V : Effect, U
        {
            if (effect is W e)
            {
                effects.Add(e);
                effect.OnDestroyEvent += (_) => effects.Remove(e);
                int temp = effect.GetPriority();
                if (temp < priority)
                {
                    int old = priority;
                    priority = temp;
                    parent.PriorityChangeInvoke(old);
                }
            }
        }

        public void Remove<V>(V effect) where V : Effect, U
        {
            if (effect is W e)
            {
                effects.Remove(e);
                if (effects.Count > 0)
                {
                    if (e.GetPriority() <= priority)
                    {
                        ReCalculatePriority();
                    }
                }
                else
                {
                    parent.EffectCaseFinishedInvoke();
                }
            }
        }

        private void ReCalculatePriority()
        {
            bool changed = false;
            int old = priority;
            foreach (Effect effect in effects)
            {
                int temp = effect.GetPriority();
                if (temp < priority)
                {
                    changed = true;
                    priority = temp;
                }
            }
            if (changed)
            {
                parent.PriorityChangeInvoke(old);
            }
        }
    }

    public abstract class AEffectCase<U, W> : IEffectCase<U> where U : IEffect where W : Effect, U
    {
        protected readonly IEffectList<U, W> effectsList;

        public event PriorityChangeEvent OnPriorityChange;
        public event EffectCaseFinishedEvent OnEffectCaseFinished;

        public AEffectCase(EffectCaseType type)
        {
            switch(type)
            {
                case EffectCaseType.SingleKeep:
                    effectsList = new EffectSingleKeep<U, W>(this);
                    break;
                case EffectCaseType.SingleReplace:
                    effectsList = new EffectSingleReplace<U, W>(this);
                    break;
                case EffectCaseType.Multiple:
                    effectsList = new EffectList<U, W>(this);
                    break;
                default:
                    Debug.LogError("Unknown effect effect case type");
                    break;
            }
        }

        public void PriorityChangeInvoke(int old)
        {
            OnPriorityChange?.Invoke(old);
        }

        public void EffectCaseFinishedInvoke()
        {
            OnEffectCaseFinished?.Invoke();
        }

        // Returns true if the priority is different
        public virtual void Update<V>(V effect) where V : Effect, U
        {
            effectsList.Update(effect);
        }

        protected virtual void Remove<V>(V effect) where V : Effect, U
        {
            effectsList.Remove(effect);
        }

        public int GetPriority()
        {
            return effectsList.GetPriority();
        }
    }

    public delegate W CreateEffectCase<U, W>() where U : IEffect where W : IEffectCase<U>;

    public class TempSortedEffectDict<U, W> where U : IEffect where W : IEffectCase<U>
    {
        private EffectDict parent;
        private Dictionary<System.Type, W> dict;
        private List<W> lst;

        public TempSortedEffectDict(EffectDict parent)
        {
            this.parent = parent;
        }

        public List<W> GetAll()
        {
            return lst;
        }

        public void Add<V>(V effect, CreateEffectCase<U, W> createNewCase) where V : Effect, U
        {
            if (AllowedTags(effect))
            {
                System.Type type = effect.GetType();
                if (dict.TryGetValue(type, out W val))
                {
                    val.Update(effect);
                }
                else
                {
                    W newCase = createNewCase();
                    newCase.Update(effect);
                    dict.Add(type, newCase);
                    newCase.OnEffectCaseFinished += () => lst.Remove(newCase);
                    newCase.OnPriorityChange += (p) => TryResort(newCase, p);
                    ListInsert(newCase);
                }
            }
        }

        private void TryResort(W effectCase, int oldPriority)
        {
            int priority = effectCase.GetPriority();
            int pos = lst.IndexOf(effectCase);
            int newPos;
            if (priority > oldPriority)
            {
                for (newPos = pos; newPos < lst.Count - 1; newPos++)
                {
                    if (lst[newPos + 1].GetPriority() > priority)
                    {
                        break;
                    }
                }
                if (pos != newPos)
                {
                    lst.RemoveAt(pos);
                    lst.Insert(newPos - 1, effectCase);
                }
            }
            else if (priority < oldPriority)
            {
                for (newPos = pos; newPos > 1; newPos--)
                {
                    if (lst[newPos - 1].GetPriority() < priority)
                    {
                        break;
                    }
                }
                if (pos != newPos)
                {
                    lst.RemoveAt(pos);
                    lst.Insert(newPos, effectCase);
                }
            }
        }

        private void ListInsert(W effectCase)
        {
            int i = 0;
            int insertPriority = effectCase.GetPriority();
            foreach (W e in lst)
            {
                if (e.GetPriority() > insertPriority)
                {
                    lst.Insert(i, effectCase);
                    return;
                }
                i++;
            }
            lst.Add(effectCase);
        }

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

    /*
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
            System.Type type = effect.GetType();
            if (updateDict.TryGetValue(type, out IEffect savedEffect))
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

        protected virtual void AddUpdateFoundExisting<T>(IEffect savedEffect, T effect) where T : Effect, U, IEffectUpdates<U>
        {
            if (savedEffect is T e)
            {
                effect.UpdateEffect(e, out bool didReplace);
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
        }

        protected virtual void AddUpdateNotExisting<T>(T effect) where T : Effect, U, IEffectUpdates<U>
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

        public void AddUpdate<T>(T effect) where T : Effect, U, IEffectUpdates<U>
        {
            if (AllowedTags(effect))
            {
                AddUpdateHelper(effect);
            }
        }
        protected abstract void AddUpdateHelper<T>(T effect) where T : Effect, U, IEffectUpdates<U>;

        public void RemoveUpdate<T>(T effect) where T : Effect, U, IEffectUpdates<U>
        {
            if (AllowedTags(effect))
            {
                RemoveUpdateHelper(effect);
            }
        }
        protected abstract void RemoveUpdateHelper<T>(T effect) where T : Effect, U, IEffectUpdates<U>;

        public void Add<T>(T effect) where T : Effect, U, IEffectAdds<U>
        {
            if (AllowedTags(effect))
            {
                AddHelper(effect);
            }
        }
        protected abstract void AddHelper<T>(T effect) where T : Effect, U, IEffectAdds<U>;

        public void RemoveAdd<T>(T effect) where T : Effect, U, IEffectAdds<U>
        {
            if (AllowedTags(effect))
            {
                RemoveAddHelper(effect);
            }
        }
        protected abstract void RemoveAddHelper<T>(T effect) where T : Effect, U, IEffectAdds<U>;
        
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
    */
}
