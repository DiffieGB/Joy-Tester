using UnityEngine;

[AddComponentMenu("Fast Controls/Atlas")]
public class FastControlsAtlas : FastControlsAtlasBase {
	public int GetIndexByName(string name) {
		if(name != null && atlasItems != null) {
			for(int i=0; i<atlasItems.Length; i++) {
				if(atlasItems[i].name.Equals(name)) {
					return i;
				}
			}
		}
		return -1;
	}
	
	public bool GetTexCoordsByIndex(int i, out Vector2 textureOffset, out Vector2 textureTiling) {
		if(atlasItems != null && i >= 0 && i<atlasItems.Length) {
			textureOffset = atlasItems[i].textureOffset;
			textureTiling = atlasItems[i].textureTiling;
			return true;
		}
		textureOffset = Vector2.zero;
		textureTiling = Vector2.one;
		return false;
	}
	
	public bool GetTexCoordsByName(string name, out Vector2 textureOffset, out Vector2 textureTiling) {
		return GetTexCoordsByIndex(GetIndexByName(name), out textureOffset, out textureTiling);
	}
	
	public string GetNameByIndex(int i) {
		if(atlasItems != null && i >= 0 && i<atlasItems.Length) {
			return atlasItems[i].name;
		}
		return null;
	}
}
