using UnityEditor;

[CustomEditor(typeof(FastControlsAtlas))]
public class FastControlsAtlasEditor : FastControlsAtlasEditorBase {
	
	// You can change the import format of atlas assets here
	// If you want to manage formats manually, make this method return false or comment it out
	// Note: texture isReadable must be true or atlas creation will fail
	protected override bool OnTextureAssetLoad(string assetPath, ref TextureImporter textureImporter) {
		bool importSettingsChanged = false;
		
		if(textureImporter.textureType != TextureImporterType.GUI) {
			textureImporter.textureType = TextureImporterType.GUI;
			importSettingsChanged = true;
		}
		
		if(!textureImporter.isReadable) {
			textureImporter.isReadable = true;
			importSettingsChanged = true;
		}
		
		if(textureImporter.textureFormat != TextureImporterFormat.AutomaticTruecolor) {
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			importSettingsChanged = true;
		}
		
		//textureImporter.alphaIsTransparency = true;
		//textureImporter.mipmapEnabled = false;
		
		return importSettingsChanged;
	}
}
