using System;
using UnityEngine;

namespace PirateSoftwareGameJam.Client.ToppingSpread
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ToppingSpreader : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Sprite _sprite;
        private Texture2D _texture;
        private Color32[] _texturePixels;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _texture = new Texture2D(512, 512);
            _texturePixels = _texture.GetPixels32();
            for (int i = 0; i < _texturePixels.Length; i++)
            {
                _texturePixels[i] = new Color32(0, 0, 0, 0);
            }
            _texture.SetPixels32(_texturePixels);
            _texture.Apply();
            _sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            _spriteRenderer.sprite = _sprite;
        }

        private Color _color = Color.red;

        public IToppingSpreaderLine StartLine(Vector2 worldPosition, int brushWidth, int brushHeight)
        {
            _color = _color == Color.red ? Color.green : _color == Color.green ? Color.blue : Color.red;
            return new ToppingSpreaderLine(worldPosition, brushWidth, brushHeight, _color, _texturePixels, _sprite.rect.width, _sprite.rect.height, WorldToPixelCoordinates, ApplyChanges);
        }

        private Vector2Int WorldToPixelCoordinates(Vector2 worldPosition)
        {
            var localPos = transform.InverseTransformPoint(worldPosition);
            var pixelWidth = _sprite.rect.width;
            var pixelHeight = _sprite.rect.height;
            var unitsToPixels = pixelWidth / _sprite.bounds.size.x * transform.localScale.x;
            var centeredX = localPos.x * unitsToPixels + pixelWidth / 2;
            var centeredY = localPos.y * unitsToPixels + pixelHeight / 2;
            var pixelPos = new Vector2Int(Mathf.RoundToInt(centeredX), Mathf.RoundToInt(centeredY));
            return pixelPos;
        }

        private void ApplyChanges()
        {
            _texture.SetPixels32(_texturePixels);
            _texture.Apply();
        }
    }

    public interface IToppingSpreaderLine
    {
        void UpdatePosition(Vector2 worldPosition);
    }

    public class ToppingSpreaderLine : IToppingSpreaderLine
    {
        private readonly int _brushWidth;
        private readonly int _brushHeight;
        private readonly float _maxWidth;
        private readonly float _maxHeight;
        private readonly Color32[] _texturePixels;
        private readonly Color32 _color;
        private readonly Func<Vector2, Vector2Int> _worldToPixelCoordinates;
        private readonly Action _applyChanges;

        private Vector2Int _previousPixelPos;

        public ToppingSpreaderLine(Vector2 worldPosition, int brushWidth, int brushHeight, Color32 color, Color32[] texturePixels, float maxWidth, float maxHeight, Func<Vector2, Vector2Int> worldToPixelCoordinates, Action applyChanges)
        {
            _brushWidth = brushWidth;
            _brushHeight = brushHeight;
            _color = color;
            _texturePixels = texturePixels;
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            _worldToPixelCoordinates = worldToPixelCoordinates;
            _applyChanges = applyChanges;

            var pixelPos = worldToPixelCoordinates(worldPosition);
            ColorPixels(pixelPos);
            applyChanges();
            _previousPixelPos = pixelPos;
        }

        public void UpdatePosition(Vector2 worldPosition)
        {
            var pixelPos = _worldToPixelCoordinates(worldPosition);

            var distance = Vector2.Distance(_previousPixelPos, pixelPos);
            var amountOfSteps = 1 / distance;

            var currentPosition = _previousPixelPos;
            for(float t = 0f; t <= 1f; t += amountOfSteps)
            {
                var newPosition =  Vector2.Lerp(_previousPixelPos, pixelPos, t);
                currentPosition = new Vector2Int((int)newPosition.x, (int)newPosition.y);
                ColorPixels(currentPosition);
            }
            _applyChanges();
            _previousPixelPos = pixelPos;
        }

        private void ColorPixels(Vector2Int centerPixel)
        {
            for(int x = centerPixel.x - _brushWidth; x <= centerPixel.x + _brushWidth; x++)
            {
                if(x >= _maxWidth - 1 || x <= 0)
                {
                    continue;
                }

                for(int y = centerPixel.y - _brushHeight; y <= centerPixel.y + _brushHeight; y++)
                {
                    if (y >= _maxHeight - 1 || y <= 0)
                    {
                        continue;
                    }

                    var index = y * (int)_maxWidth + x;
                    _texturePixels[index] = _color;
                }
            }
        }
    }
}