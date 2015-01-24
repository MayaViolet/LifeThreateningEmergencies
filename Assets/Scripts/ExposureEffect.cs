using UnityEngine;

[ExecuteInEditMode(), RequireComponent(typeof(Camera))]
class ExposureEffect : MonoBehaviour {

	[Range(0, 2)]
	public float exposure = 1;
	float lastExposure = 1;

	Shader _shader;
	Material _material;
	Material material
	{
		get
		{
			if (_material == null)
			{
				if (_shader == null)
				{
					_shader = Shader.Find("Unlit/Texture + Colour");
				}
				_material = new Material(_shader);
				_material.color = new Color(exposure/2, exposure/2, exposure/2, 1);
			}
			return _material;
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)	
	{
		if (exposure != lastExposure)
		{
			material.color = new Color(exposure/2, exposure/2, exposure/2, 1);
		}
		lastExposure = exposure;
		Graphics.Blit(source, destination, material);
	}
		
}