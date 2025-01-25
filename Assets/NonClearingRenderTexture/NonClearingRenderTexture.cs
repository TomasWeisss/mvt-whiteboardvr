using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering;

[RequireComponent(typeof(Camera))]
public class NonClearingRenderTexture : MonoBehaviour
{
	// Public
	[Header("Input")]
	[Tooltip("The RenderTexture in which the Camera will render into. Defines the properties of the output.")]
	public RenderTexture renderTexture;

	[Header("Output")]
	[Tooltip(
		"The RenderTexture output for the Camera, that will retain the contents of the previous render (as if Camera was set to Don't Clear).")]
	public RenderTexture outputRenderTexture;

	[Tooltip("A Texture2D containing the same data as the outputRenderTexture")]
	public Texture2D outputTexture2D;

	// References
	[SerializeField] private Shader renderShader;
	private Camera _camera;
	private Texture2D _tempTex;
	private Material _renderMat;
	


	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}
	private void Start()
	{
		Assert.IsNotNull(renderTexture, "Input RenderTexture must be defined");
		_camera.targetTexture = renderTexture;
		
		// Textures
		_tempTex = new Texture2D(renderTexture.width, renderTexture.height, renderTexture.graphicsFormat,
			TextureCreationFlags.None);
		outputRenderTexture = new RenderTexture(renderTexture);
		outputTexture2D = new Texture2D(renderTexture.width, renderTexture.height, renderTexture.graphicsFormat,
			TextureCreationFlags.None);
		
		// Material
		_renderMat = new Material(renderShader);
		_renderMat.SetTexture("_PreviousTex", outputTexture2D);
	}
	private void Update()
	{
		_camera.Render();

		CopyRenderTextureTo2D(renderTexture, _tempTex);

		Graphics.Blit(_tempTex, outputRenderTexture, _renderMat); 
		
		CopyRenderTextureTo2D(outputRenderTexture, outputTexture2D);
	}

	

	private void CopyRenderTextureTo2D(RenderTexture rt, Texture2D tex)
	{
		RenderTexture.active = rt;
		tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		tex.Apply();
		RenderTexture.active = null;
	}
}
