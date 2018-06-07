using UnityEngine;

public class TileAblePlaneVisualiser : MonoBehaviour
{
	private Vector2 offset = new Vector2();
	private Renderer render;
	[SerializeField] private bool isInverted;

	private const string MAIN_TEXTURE = "_MainTex";

	private void Start () {
		render = GetComponent<Renderer>();
	}
	
	private void Update () {
		if(isInverted)
		{
			float _speed = ChunkMover.Instance.Speed * -1;
			offset.y += (_speed / 100) * Time.deltaTime;
			render.material.SetTextureOffset(MAIN_TEXTURE, new Vector2(0, offset.y));

		}
		if (!isInverted)
		{
			float _speed = ChunkMover.Instance.Speed;
			offset.y += (_speed / 100) * Time.deltaTime;
			render.material.SetTextureOffset(MAIN_TEXTURE, new Vector2(0, offset.y));
		}
	}
}