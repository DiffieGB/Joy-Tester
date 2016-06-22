using UnityEngine;
using System.Collections;

[AddComponentMenu("Fast Controls/Renderer")]
public class FastControlsRenderer : FastControls {
	protected override void MeshInstanceCreated(Mesh mesh) {
		// If you are using multithreaded rendering you may not use MarkDynamic since it is not supported.
		// If you are not using multithreaded rendering you may get an extra performance boost by uncommenting the line below.
		#if !(UNITY_2_6	|| UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5)
		//mesh.MarkDynamic();
		#endif
	}
}
