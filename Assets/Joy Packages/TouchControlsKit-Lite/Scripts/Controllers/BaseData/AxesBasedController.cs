/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 AxesBasedController.cs                *
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
using m_IEnumerator = System.Collections.IEnumerator;

namespace TouchControlsKit
{
    [System.Serializable]
    public sealed class Axis
    {
        internal const int DIGITS = 2;

        public bool enabled = true;
        public bool inverse = false;

        internal float value { get; private set; }

        // SetValue
        internal void SetValue( float value )
        {
            if( enabled )            
                this.value = ( float )System.Math.Round( ( double )value, 3 );            
            else
                this.value = 0f;
        }
    }

    public abstract class AxesBasedController : ControllerBase
    {
        public float sensitivity = 1f;

        public bool axesLag = false;
        public float axesLagSpeed = 10f;

        public Axis axisX, axisY;

        [SerializeField]
        private bool showBaseImage = true;

        protected Vector2 defaultPosition, currentPosition, currentDirection;
        
        // Show TouchZone
        public bool ShowTouchZone
        {
            get { return showBaseImage; }
            set
            {
                if( showBaseImage == value )
                    return;

                showBaseImage = value;
                ShowHideTouchZone();
            }
        }
        // ShowHide TouchZone
        private void ShowHideTouchZone()
        {
            if( showBaseImage )
            {
                baseImage.color = baseImageNativeColor;
            }
            else
            {
                baseImageNativeColor = baseImage.color;
                baseImage.color = ( Color32 )Color.clear;
            }
        }


        // Set Axis
        protected void SetAxis( float x, float y )
        {
            x = axisX.inverse ? -x : x;
            y = axisY.inverse ? -y : y;

            if( axesLag )
            {
                if( axisX.enabled )
                {
                    StopCoroutine( "SmoothAxisX" );
                    StartCoroutine( "SmoothAxisX", x );                    
                }
                else
                    axisX.SetValue( 0f );

                if( axisY.enabled )
                {
                    StopCoroutine( "SmoothAxisY" );
                    StartCoroutine( "SmoothAxisY", y );
                }
                else
                    axisY.SetValue( 0f );
            }
            else
            {
                axisX.SetValue( x );
                axisY.SetValue( y );
            }
        }

        // Smooth AxisX
        private m_IEnumerator SmoothAxisX( float targetValue )
        {
            while( System.Math.Round( ( double )axisX.value, Axis.DIGITS ) != System.Math.Round( ( double )targetValue, Axis.DIGITS ) )
            {
                axisX.SetValue( Mathf.Lerp( axisX.value, targetValue, Time.smoothDeltaTime * axesLagSpeed ) );
                yield return null;
            }

            axisX.SetValue( targetValue );
        }
        // Smooth AxisY
        private m_IEnumerator SmoothAxisY( float targetValue )
        {
            while( System.Math.Round( ( double )axisY.value, Axis.DIGITS ) != System.Math.Round( ( double )targetValue, Axis.DIGITS ) )
            {
                axisY.SetValue( Mathf.Lerp( axisY.value, targetValue, Time.smoothDeltaTime * axesLagSpeed ) );
                yield return null;
            }

            axisY.SetValue( targetValue );
        }

        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();
            SetAxis( 0f, 0f );
        }
    }
}