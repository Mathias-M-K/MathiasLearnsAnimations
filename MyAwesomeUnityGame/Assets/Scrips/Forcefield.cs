using UnityEngine;

namespace Scrips
{
    public class Forcefield : MonoBehaviour
    {
    
        public Material mat;
        public float offset;

        public float intensity;
        public float smoothingFactor;


        [Header("Color Settings")] 
        public Color baseColor;     //Default color of forcefield
        public Color impactColor;   //Color when hit
        public Color currentColor;  //Color right now
        public Color targetColor;   //Target COlor

        [Header("Prototyping")] 
        public KeyCode key;

        private string _forcefieldColorRef = "Color_E7FC1265";
    
        // Start is called before the first frame update
        void Start()
        {
            mat = GetComponent<Renderer>().material;
            baseColor = GetForcefieldColor();

        }

        // Update is called once per frame
        void Update()
        {
            currentColor = mat.GetColor(_forcefieldColorRef);

            Color newColor = currentColor*intensity;
        
            if (currentColor != baseColor)
            {
                newColor  = Color.Lerp(currentColor, targetColor*intensity, Time.deltaTime * smoothingFactor); ;
            }
            
            SetForcefieldColor(newColor);
        }

        private void OnCollisionEnter(Collision other)
        {
            SetForcefieldColor(impactColor*intensity);
        }

        private void SetForcefieldColor(Color color)
        {
            mat.SetColor(_forcefieldColorRef,color);
        }

        private Color GetForcefieldColor()
        {
            return mat.GetColor(_forcefieldColorRef);
        }




    }
}
