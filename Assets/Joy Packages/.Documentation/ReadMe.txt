| Setup
 =======
The package contains a .dll file and two folders. The .dll file is strictly needed and must be included in your project under the folder /Assets/Plugins/. The "Demo" folder contains only a demo scene with its assets and it can be deleted once you read the example code. The "Documentation" folder contains both this ReadMe.txt and a zip containing the offline API documentation.

| First steps
 =============
VirtualJoystick was originally designed to be very similiar to the standard Unity's Input class in its usage.
In this last version were made a lot of optimizations in performance and the structure of the class has been slightly changed. Let's see what changed.
 - Three interfaces have been added (IVJControl, IPad, IButton). They give you guidelines to implement your own custom controls. The controls provided by me (Pad, Button and Toggle) implement those interfaces.
 - All the controls have now an event based interface. Buttons and pads raise events both when they begin and end their interactions with user touches (Pressed, Released). Toggles raise an event whenever their state changes.
 - The analog pad controls can now be read in their entirety via the Vector2 VirtualJoystick.GetAxes(sring tag) method.
 - All the controls keep their traditional approach but for the toggles is encouraged the event-based approach for performance enhencement.
 - The alignment of the controls is now more intuitive. You can spicify both the anchor point of the graphic and the screen origin.
 - The main visible new feature is the pop-up pad! A pad marked as pop-up (right or left) isn't displayed on the screen by default but, when the user touches the (right or left half of the) screen the pad shows up in that position. It's useful because you can keep the UI as clean as possible and show the controls only when they actually are needed.

| How to use the plugin
 =======================
In order to use the plugin you have to add the component to a GameObject (better a dedicated null GameObject that you can mark not to be destroyed on load).
Once configured at your wish via inspector you have to create a reference to that instance from which access its members.
Explaining the procedure here further may confuse... instead I encourage to read fully the demo script (Demo.cs) since it gives an example for each feature offered by this plugin.

| API reference
 ===============
Don't forget the API reference available online (also for download) at http://unity.codx.altervista.org/asset.php?name=VirtualJoystick%20-%20Multi-touch . If you have any doubt over any of the functionalities in thid plugin maybe you find exactly what you need in the detailed description of each element.