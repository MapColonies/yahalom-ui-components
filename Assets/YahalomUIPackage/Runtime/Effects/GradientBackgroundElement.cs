using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.Effects
{
    /// <summary>
    /// Draws a vertical gradient using a dynamically generated 1x2 Texture2D.
    /// Used internally by BasicButton as a background.
    /// </summary>
    public class GradientBackgroundElement : VisualElement
    {
        private Texture2D _gradientTexture;
        private bool _textureDirty = true;

        private Color _topColor = Color.white;
        private Color _bottomColor = Color.black;

        public Color TopColor
        {
            get => _topColor;
            set
            {
                if (_topColor != value)
                {
                    _topColor = value;
                    _textureDirty = true;
                    MarkDirtyRepaint();
                }
            }
        }

        public Color BottomColor
        {
            get => _bottomColor;
            set
            {
                if (_bottomColor != value)
                {
                    _bottomColor = value;
                    _textureDirty = true;
                    MarkDirtyRepaint();
                }
            }
        }

        public GradientBackgroundElement()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void EnsureTexture()
        {
            if (_gradientTexture == null)
            {
                _gradientTexture = new Texture2D(1, 2, TextureFormat.RGBA32, false)
                {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    hideFlags = HideFlags.HideAndDontSave
                };
                _textureDirty = true;
            }

            if (_textureDirty)
            {
                // NOTE: swapped to match UI "Top Color" visually on top
                _gradientTexture.SetPixel(0, 0, _topColor);      // top
                _gradientTexture.SetPixel(0, 1, _bottomColor);   // bottom
                _gradientTexture.Apply();
                _textureDirty = false;
            }
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            Rect rect = contentRect;
            if (rect.width <= 0 || rect.height <= 0)
                return;

            EnsureTexture();
            if (_gradientTexture == null)
                return;

            var meshWriteData = ctx.Allocate(4, 6, _gradientTexture);

            float xMin = rect.xMin;
            float xMax = rect.xMax;
            float yMin = rect.yMin;
            float yMax = rect.yMax;

            Vertex v;

            // bottom-left
            v = new Vertex
            {
                position = new Vector3(xMin, yMin, 0),
                tint = Color.white,
                uv = new Vector2(0, 0)
            };
            meshWriteData.SetNextVertex(v);

            // bottom-right
            v = new Vertex
            {
                position = new Vector3(xMax, yMin, 0),
                tint = Color.white,
                uv = new Vector2(1, 0)
            };
            meshWriteData.SetNextVertex(v);

            // top-right
            v = new Vertex
            {
                position = new Vector3(xMax, yMax, 0),
                tint = Color.white,
                uv = new Vector2(1, 1)
            };
            meshWriteData.SetNextVertex(v);

            // top-left
            v = new Vertex
            {
                position = new Vector3(xMin, yMax, 0),
                tint = Color.white,
                uv = new Vector2(0, 1)
            };
            meshWriteData.SetNextVertex(v);

            meshWriteData.SetNextIndex(0);
            meshWriteData.SetNextIndex(1);
            meshWriteData.SetNextIndex(2);
            meshWriteData.SetNextIndex(2);
            meshWriteData.SetNextIndex(3);
            meshWriteData.SetNextIndex(0);
        }
    }
}
