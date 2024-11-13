using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionalTilemap : MonoBehaviour, IDimensional
{
    private Tilemap           _tilemap;
    private TilemapRenderer   _tilemapRenderer;
    private TilemapCollider2D _tilemapCollider;

    [SerializeField] private int _dimension;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnEnable()
    {
        (this as IDimensional).Register();
    }

    private void OnDisable()
    {
        (this as IDimensional).Unregister();
    }

    public void UpdateDimension(int currentDimension)
    {
        if (_dimension == currentDimension)
        {
            _tilemap.color = new Color(_tilemap.color.r, _tilemap.color.g, _tilemap.color.b, 1);
            _tilemapRenderer.sortingOrder = 1;
            _tilemapCollider.enabled = true;
        }
        else
        {
            _tilemap.color = new Color(_tilemap.color.r, _tilemap.color.g, _tilemap.color.b, 0.1f);
            _tilemapCollider.enabled = false;
            _tilemapRenderer.sortingOrder = 0;
        }
    }
}