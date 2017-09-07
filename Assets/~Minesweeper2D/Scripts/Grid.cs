﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper2D {
    public class Grid : MonoBehaviour {

        public GameObject tilePrefab;
        public int width = 10;
        public int height = 10;
        public float spacing = .155f;

        private Tile[,] tiles;

        // Use this for initialization
        void Start() {

            GenerateTiles();

            //GetAdjacentMineCountAt(tiles[3, 7]);
        }

        // Update is called once per frame
        void FixedUpdate() {

            RevealTile();
        }

        Tile SpawnTile(Vector3 pos) {

            GameObject clone = Instantiate(tilePrefab);
            clone.transform.position = pos;

            Tile currentTile = clone.GetComponent<Tile>();
            return currentTile;
        }

        void GenerateTiles() {

            tiles = new Tile[width, height];

            for (int x = 0; x < width; x++) {

                for (int y = 0; y < height; y++) {

                    Vector2 halfsize = new Vector2(width / 2, height / 2);
                    Vector2 pos = new Vector2((x - halfsize.x) + .5f, (y - halfsize.y) + .5f);
                    pos *= spacing;

                    Tile tile = SpawnTile(pos);
                    tile.transform.SetParent(transform);

                    tile.x = x;
                    tile.y = y;

                    tiles[x, y] = tile;
                }
            }
        }

        public int GetAdjacentMineCountAt(Tile t) {

            int count = 0;

            for (int x = -1; x <= 1; x++) {

                for (int y = -1; y <= 1; y++) {

                    int desiredX = t.x + x;
                    int desiredY = t.y + y;

                    if (desiredX >= 0 && desiredY >= 0 && desiredX < width && desiredY < height) {

                        Tile tile = tiles[desiredX, desiredY];

                        if (tile.isMine) {

                            count += 1;
                        }
                    }
                }
            }

            return count;
        }

        void RevealTile() {

            if (Input.GetMouseButtonDown(0)) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null) {

                    Tile tile = hit.collider.gameObject.GetComponent<Tile>();

                    if (tile) {

                        tile.Reveal(GetAdjacentMineCountAt(tile));
                    }
                }
            }
        }
    }
}
