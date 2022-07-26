# ECSharp

This project is a prototype for a lightweight ECS implementation with archetypal storage, heavily inspired by FLECS. It performs relatively well but there is room for improvement.

## FSharp example

```fsharp
open ECSharp.FSharp
open ECSharp

type NameComponent = 
    struct
        val Name : string
        new (n : string) = {Name = n}
    end

let w = new World()

w 
|> World.CreateEntity
|> Entity.WithValue (NameComponent "Martha")
|> ignore

w
|> Processor.Add (Processor.Create (fun _ -> () ))

w 
|> World.GetEntity 0 
|> Entity.Get<NameComponent>
|> fun x -> x.Name
|> printfn "Hello %s"

```


## Example C#

Let's create a name and health component :

```csharp
public struct HealthComponent
{
    public float LifePoints;
    public float Shield;
}
public struct NameComponent
{
    public string Name;
}
public struct ModelComponent
{
    public byte[] Buffer;
}
```

As you can see, components are just structs.

Then we create a processor for `NameComponent` :

```csharp
public class NameProcessor : Processor<QueryEntity<NameComponent>>
{
    public override void Update()
    {
        // Query1 returns a list of archetypes containing the types
        // mentionned in the first QueryEntity of the processor's generics
        Query1
        .AsParallel()
        .ForAll(
            x => {
                for(int i = 0; i< x.Length; i++)
                    x.GetComponentArrayStruct<NameComponent>()[i] = 
                        new NameComponent{Name = "Lola2"};
            }
        );
    }
}

public class ModelProcessor : Processor<QueryEntity<ModelComponent>>
{
    public override void Update()
    {
        // Query1 returns a list of archetypes containing the types
        // mentionned in the first QueryEntity of the processor's generics
        Query1
        .AsParallel()
        .ForAll(
            x => {
                for(int i = 0; i< x.Length; i++)
                    x.GetComponentArray<ModelComponent>()[i].Buffer[5] = 1;
            }
        );
    }
}
```

And finally the code to create our world :

```csharp
var world = new World();

world.CreateEntity()
    .With(new NameComponent{Name = "Name"});
world.CreateEntity()
    .With(new NameComponent{Name = "Name2"})
    .With(new HealthComponent{})
    .With(new ModelComponent());

world.Add(new NameProcessor());
// After this line of code every NameComponent will be updated by the processor
world.Update();

```
