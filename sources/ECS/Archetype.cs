using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ECSharp.Arrays;
using ECSharp.ComponentData;

namespace ECSharp
{
    public class Archetype
    {
        public static Archetype Empty = new(new List<ComponentBase>());
        public Dictionary<Type, ComponentList> Storage = new();
        public List<long> EntityID = new();

        public ArchetypeID ID = new();

        public ArchetypeEdges Edges = new();

        public int Length => EntityID.Count;

        public Archetype(IEnumerable<ComponentBase> components)
        {
            foreach(var c in components)
            {
                Storage[c.GetComponentType()] = c.EmptyArray();
            }
            ID = new ArchetypeID(components.Select(x => x.GetComponentType()));
        }

        public Archetype(IEnumerable<ComponentList> componentArrays)
        {
            foreach(var ca in componentArrays)
            {
                Storage[ca.ComponentType] = ca.New();
            };
            ID = new ArchetypeID(componentArrays.Select(x => x.ComponentType));
            
        }

        public bool IsSupersetOf(Archetype t) => this.ID.IsSupersetOf(t.ID);
        public bool IsSubsetOf(Archetype t) => this.ID.IsSubsetOf(t.ID);
        public IEnumerable<Type> TypeIntersect(Archetype t) => this.ID.Intersect(t.ID);
        public IEnumerable<Type> TypeExcept(Archetype t) => this.ID.Except(t.ID);


        public void SetValue<T>(int index, in T component) where T : struct
        {
            ((ComponentList<T>)Storage[typeof(T)])[index] = component;
        }

        public ComponentList<T> GetComponentArray<T>() where T : struct
        {
            return (ComponentList<T>)Storage[typeof(T)];
        }
        public void GetEntityComponent<T>(int i, out T c) where T : struct
        {
            c = ((ComponentList<T>)Storage[typeof(T)])[i];
        }

        public ComponentList GetArray<T>() => Storage[typeof(T)];

        public ComponentList GetComponentArray(Type t)
        {
            return Storage[t];
        }
        public void AddComponent<T>(in T component, long entity) where T : struct
        {
            if(Storage.ContainsKey(typeof(T)))
            {
                ((ComponentList<T>)Storage[typeof(T)]).Add(component);
                EntityID.Add(entity);
            }
        }

        public void RemoveEntity(Entity e) => EntityID.RemoveAt(EntityID.IndexOf(e.Index));
        
        public void SetComponent<T>(int index, in T component) where T : struct
        {
            if(Storage.ContainsKey(typeof(T)))
            {
                ((ComponentList<T>)Storage[typeof(T)])[index] = component;
            }
        }

        internal void AddEntity(Entity entity) => EntityID.Add(entity.Index);

        public override string ToString()
        {
            var result = 
            new StringBuilder()
                .Append("Type : [")
                .Append(string.Join(";", Storage.Keys.Select(x => x.Name)??new List<string>()))
                .Append(']')
                .AppendLine()
                .Append("Storages : [")
                .Append(string.Join(";",Storage.Values.Select(x => x.ToString())))
                .Append(']');
            return result.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is Archetype archetype &&
                //    EqualityComparer<Dictionary<Type, IComponentArray>>.Default.Equals(Storage, archetype.Storage);
                   EqualityComparer<ArchetypeID>.Default.Equals(ID, archetype.ID);
                //    EqualityComparer<List<IComponentArray>>.Default.Equals(Components, archetype.Components) &&
                //    Length == archetype.Length;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}