using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FastControlsRenderer))]
public class FastControlsEditor : FastControlsEditorBase
{
	private FastControlsPreview previewWindow = null;
	
	protected override void OnEnable() {
		base.OnEnable();

		// Automatically add FastControlsAtlas but do not require it so atlas can be shared between renderers
		// By using [RequireComponent (typeof (FastControlsAtlas))] would always require it on same gameobject
		FastControlsRenderer atlasRenderer = (FastControlsRenderer) target;

		if(atlasRenderer != null) {
			if(atlasRenderer.atlas == null) {
				atlasRenderer.atlas = atlasRenderer.GetComponent<FastControlsAtlas>();
				
				if(atlasRenderer.atlas == null) {
					atlasRenderer.atlas = atlasRenderer.gameObject.AddComponent<FastControlsAtlas>();
				}
			}
		}

		// Show Fast Controls preview window
		if(!Application.isPlaying) {
			previewWindow = (FastControlsPreview)EditorWindow.GetWindow<FastControlsPreview>("Fast Controls", false, typeof(SceneView));

			if(previewWindow != null) {
				previewWindow.fastControls = (FastControls)target;
				previewWindow.Repaint();
			}
		}

		// If the shader is null, try to load the default shader, if it is not found a fixed function shader is used instead
		FastControls fastControls = (FastControls)target;

		if(fastControls != null) {			
			if(fastControls.shader == null) {
				Shader shader = (Shader)AssetDatabase.LoadAssetAtPath("Assets/FastControls/Shaders/FastControls.shader", typeof(Shader));
				if(shader != null) {
					fastControls.shader = shader;
				}
			}
		}
	}

	public override void OnControlsChanged() {
		if(previewWindow != null) {
			previewWindow.Repaint();
		}
	}
	
	protected override void OnDisable() {
		base.OnDisable();

		if(previewWindow != null) {
			previewWindow.fastControls = null;
			previewWindow.Close();
			previewWindow = null;
		}
	}
}
