using System;
using System.Collections.Generic;

/// <summary>
/// Pairs a songlist to a specific scene.
/// </summary>
[Serializable]
internal class SongsByScenesPair
{
#pragma warning disable CS0649,
    public Scenes Scene;
    public List<Song> Songs;
#pragma warning restore CS0649,
}