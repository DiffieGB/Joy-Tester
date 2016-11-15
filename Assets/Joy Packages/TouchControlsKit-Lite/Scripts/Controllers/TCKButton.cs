/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKButton.cs                          *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
    [RequireComponent( typeof( Image ) )]
    public class TCKButton : ControllerBase,
        IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
    {        
        public bool swipeOut = false;

        [SerializeField]
        private Sprite normalsprite = null;
        public Sprite pressedSprite = null;

        public Color32 normalColor, pressedColor;

        public Sprite normalSprite
        {
            get { return normalsprite; }
            set
            {
                if( normalsprite == value )
                    return;

                normalsprite = value;
                baseImage.sprite = normalsprite;
            }
        }        
        
        private int 
            pressedFrame = -1, releasedFrame = -1, clickedFrame = -1;


        // isPRESSED
        internal bool isPRESSED {  get { return touchDown; } }
        // isDOWN
        internal bool isDOWN { get { return ( pressedFrame == Time.frameCount - 1 ); } }
        // isUP
        internal bool isUP { get { return ( releasedFrame == Time.frameCount - 1 ); } }
        // isCLICK
        internal bool isCLICK { get { return ( clickedFrame == Time.frameCount - 1 ); } }
                
        
        // Update Position
        protected override void UpdatePosition( Vector2 touchPos )
        {
            base.UpdatePosition( touchPos );

            if( !touchDown )
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;
                pressedFrame = Time.frameCount;

                ButtonDown();
            }            
        }
                

        // Button Down
        protected void ButtonDown()
        {
            baseImage.sprite = pressedSprite;
            baseImage.color = visible ? pressedColor : ( Color32 )Color.clear;
        }

        // Button Up
        protected void ButtonUp()
        {
            baseImage.sprite = normalSprite;
            baseImage.color = visible ? normalColor : ( Color32 )Color.clear;
        }

        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();

            releasedFrame = Time.frameCount;
            ButtonUp();            
        }        

        // OnPointer Down
        public void OnPointerDown( PointerEventData pointerData )
        {
            if( !touchDown )
            {
                touchId = pointerData.pointerId;
                UpdatePosition( pointerData.position );
            }
        }

        // OnDrag
        public void OnDrag( PointerEventData pointerData )
        {
            if( Input.touchCount >= touchId && touchDown )
            {
                UpdatePosition( pointerData.position );
            }
        }

        // OnPointer Exit
        public void OnPointerExit( PointerEventData pointerData )
        {
            if( !swipeOut )            
                OnPointerUp( pointerData );
        }

        // OnPointer Up
        public void OnPointerUp( PointerEventData pointerData )
        {
            ControlReset();
        }

        // OnPointer Click
        public void OnPointerClick( PointerEventData pointerData )
        {
            clickedFrame = Time.frameCount;
        }
    }
}