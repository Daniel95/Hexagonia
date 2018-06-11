using System;
using System.Collections.Generic;

/// <summary>
/// Pairs a songlist to a specific scene.
/// </summary>
[Serializable]
internal class Songlist
{
#pragma warning disable CS0649,
    public Scenes Scene;
    public List<Song> SongList;
#pragma warning restore CS0649,
}