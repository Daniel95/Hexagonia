using UnityEngine;

/// <summary>
/// An music visualizer effect that moves a tileable texture and scales an gameobject based on the music.
/// </summary>
public class TileableTextureMover : MonoBehaviour
{
	private const string MAIN_TEXTURE = "_MainTex";

	[SerializeField] private bool isInverted;
	[SerializeField] private float speedMultiplier = 0.1f;

    private Vector2 offset = new Vector2();
	private Renderer render;

	private void Awake()
    {
		render = GetComponent<Renderer>();
	}

    private void Update()
    {
        float _speed = ChunkMover.Instance.Speed;

        if (isInverted)
        {
            _speed *= -1;
        }

        offset.y += (_speed * speedMultiplier) * Time.deltaTime;
        render.material.SetTextureOffset(MAIN_TEXTURE, new Vector2(0, offset.y));
	}
}