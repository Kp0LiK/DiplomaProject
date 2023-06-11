using UnityEngine;

namespace Client
{
    public class CustomCursor : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursorTexture;
        
        private float _cursorScale = 4f;

        private void Start()
        {
            // Load the custom cursor texture
            _cursorTexture = Resources.Load<Texture2D>("MouseCursor");
            
            Vector2 scaledHotspot = new Vector2(_cursorTexture.width * _cursorScale / 2f,
                _cursorTexture.height * _cursorScale / 2f);

            // Set the cursor texture and hotspot
            Cursor.SetCursor(_cursorTexture, scaledHotspot, CursorMode.Auto);

            // Set the cursor texture and hotspot
        }
    }
}