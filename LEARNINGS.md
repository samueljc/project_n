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

## Localization

Using `StreamingAssets` to write the translation files to disk.

https://docs.unity3d.com/Manual/StreamingAssets.html

Dump the translations as JSON and load them into a map.

```C#
string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
if (File.Exists(filePath)) {
  string data = File.ReadAllText(filePath);
  LocalizationData loadedData = JsonUtilty.FromJSON<LocalizationData>(data);
  // load dictionary with text (C# can't serialize a dictionary)
} else {
  // log error; just use default
}
```

If you have a `StartupManager` you can put start in a coroutine (`IEnumerator`)
and yield while everything loads. Then when everything is ready you transition
to the menu via the `SceneManager`.

## Custom Tools

Inherit from `EditorWindow`.

`EditorUtility.SaveFilePanel` for saving data to a file.

`Init` is kind of like `Awake`.
`OnGUI` is kind of like `Update`.

## Instantiate

Instantiating objects ends up calling `Awake` and `OnEnable`. `OnEnable` will
be skipped if the object is disabled, so if you expect some values to be
defined before `OnEnable` runs you need your prefab to be disabled by default,
set those values, and then activate the object.
