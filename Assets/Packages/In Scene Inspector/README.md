# In-Scene-Inspector (ISI)

Unity Asset for Editing Scenes from withing the Scene. 

## Download

In Scene Inspector is curently in beta. The [unitypackage can be download here](http://tinyphoenixgames.com/sites/default/files/forum_topic_attachments/in-scene-inspector-v0.0.5b_0.unitypackage).

## Installation

Once your download is complete you should have a file named in-scene-inspector-v#.unitypackage. This can then be imported into Unity:
1. From the unity top menu select "Assets" > "Import Package" > "Custom Package..."
2. Find the ".unitypackage" file and click open once selected
3. In the "Import Unity Package" window ensure that all the assets are checked and  click "Import."
4. Confirm that the folder "In Scene Inspector" has been added directly under your Unity "Asset" folder.

## Usage

Once you've got everything downloaded and installed you should have the following folder structure under your Unity "Assets" folder:

* Scene Inspector
  * Images - Images used to style the inspector. These can be directly replaced (at least in the beta) although it would be appreciated if the ISI logo is included somewhere.
  * Prefabs
    * Field Types - Prefabs used in generated interface for members of certain variable types. 
    * Widgets - Extra functionality required by ISI included in the form of widgets. 
  * Scripts - DLL containing all needed scripts used by ISI.

### Using Prefabs

There are 2 prefabs included in the "Prefabs" folder:

* In Scene Inspector - All in one solution including UI canvas, event system, and Inspector Panel.
* Inspector Panel - The panel only. This is the core and all that is required for full functionality. 

The first thing you should try is adding the prefab (Assets/Scene Inspector/Prefabs/In Scene Inspector.prefab) to your scene. You will see 3 buttons:

* In Scene Inspector - This is a toggle tab that allows the panel drawer to be closed and opened.
* Select Object - This is a button to open a dropdown menu with a full scene objects parent/child hierarchy. Buttons in this menu can be clicked to select for component browsing.
* Select Component - Once you have an object selected this button can be clicked to expose all attached components. Click on any component to inspect. Inspecting a component steps through accessible parameters (public/serializable) and generates interface to modify them. 

#### Prefab Field Types

Currently, there is 1 custom field type (Boolean) when ISI inspects a component and finds a bool it uses the corresponding "Bool Field" prefab as interface to change the member. 

For all other field types, a text box is generated. When a value is submitted ISI attempts to cast the value to the type of the member.

### Console reporting 

When attempting to change the value of a component member ISI reports success or failure. 

* Success - Setting OBJECT_NAME (COMPONENT_NAME) field FIELD_NAME to (VAR_TYPE) VALUE
* Failure - FormattingException with parse information

### Unity Editor for In Scene Inspector

In the custom Unity Editor for ISI you have the following settings that can be configured: 

* Default Object - Object to be selected by default when the project starts
* Default Component - Component to be selected by default when the project starts
* Height - The height of the inspector panel.
* Hide - Toggls the visibility and interatablility of all ISI components.
* Use Custom Inspector - Unchecking this allows you to have full control over all of the accessible parameters of ISI. 

### Creating a User Test Build

To create a restricted build to send to a tester you can:

1. Set the "Default Object" in the Unity Editor.
2. Set the "Default Component" in the Unity Editor.
3. Find the game object "Select Object Button" under Inspector Panel and delete it.
4. Find the game object "Select Component Button" under Inspector Panel and delete it.

That will result in a build that gives users access to change only accessible members in the selected component. 

### Scripts

* DropDownList.cs - Stores basice structure of list.
* DropDownMenu.cs - Focus and contents functionality included in select object and component drop down lists.
* InSceneInspector.cs - Core inspection functionality.
* InSceneInspectorEditor.cs - Custome Unity Editor formatting.  
* ScrollRect2.cs - Extention to UnityEngine.UI.ScrollRect with the added functionality of saving scroll position when the scroll rect height changes.

## Contributing

ISI is not an open source project. If you have access to the repository this is the standard process to submit a contribution for review.

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Credits

© 2016 Copyright Tiny Phoenix Games. All Rights Reserved.

## License

License include in project but can also be found on the [Tiny Phoenix Website](http://tinyphoenixgames.com/sites/default/files/shared-files/LICENSE.txt)
