Use [NonSerialized] to prevent an item from being remembered between
executions. This will also prevent it from having a default value, so if
you want a default you'll also need a serizlied default field. You can
then implement the `ISerializationCallbackReceiver` interface and do
something like the following:

```
public void OnAfterDeserialize() {
  this.value = this.defaultValue;
}

public void OnBeforeSerialize() {}
```