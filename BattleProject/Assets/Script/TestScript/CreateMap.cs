using System.Collections;
using System.Collections.Generic;
using GameSys;
using GameSys.Lib;

namespace GameSys
{
    namespace Map
    {
        /// <summary>
        /// 타일 데이터 클래스
        /// 타일의 타입과 높이 등 정보를 보유
        /// </summary>
        public class TileData
        {
            public eTileType eType;
            public eResource eUnderRes;
            public int sHeight;

            public TileData(eTileType eType,int sHeight)
            {
                this.eType = eType;
                this.sHeight = sHeight;
            }

            public int GetType()
            {
                return (int)eType;
            }
        }

        /// <summary>
        /// 맵 테이블
        /// 타일 뭉쳐둔 데이터 구조
        /// </summary>
        public class MapTable
        {
            public int nXSize;
            public int nYSize;
            public TileData[,] table;

            public MapTable(int nXSize, int nYSize)
            {
                this.nXSize = nXSize;
                this.nYSize = nYSize;
                table = new TileData[nXSize, nYSize];
            }

            public void SetTile(TileData tile, int nX, int nY)
            {
                table[nX, nY] = tile;
            }

            public TileData GetTile(int nX, int nY)
            {
                return table[nX, nY];
            }
        }

        /// <summary>
        /// 지형 생성기
        /// ver 1.0
        /// </summary>
        public static class CreateMap
        {
            private static MapTable mapTable;
            private static int rate = 40; // 높으면 땅 비율 높음
            private static int mergePoint = 25; // 낮으면 땅 비율 높음
            private static int mergeRange = 4; // 높으면 땅이 뭉침
            //황금비 40:25:4

            public static MapTable Create(int nXSize, int nYSize, int uiSeed = 0)
            {
                if (uiSeed == 0)
                {
                    uiSeed = (int)System.DateTime.Now.Ticks;
                }
                System.Random rand = new System.Random(uiSeed);

                mapTable = new MapTable(nXSize, nYSize);

                bool[,] workMap = new bool[nXSize, nYSize];

                for (int x = 0; x < nXSize; x++)
                {
                    for (int y = 0; y < nYSize; y++)
                    {
                        if (rand.Next(1, 101) < rate)
                        {
                            workMap[x, y] = true;
                        }
                    }
                }
                bool[,] tempMap = CreateGeometry(workMap, nXSize, nYSize);
                tempMap = CreateGeometry(tempMap, nXSize, nYSize);

                for (int x = 0; x < nXSize; x++)
                {
                    for (int y = 0; y < nYSize; y++)
                    {
                        workMap[x, y] = tempMap[x, y];
                    }
                }

                for (int x = 0; x < nXSize; x++)
                {
                    for (int y = 0; y < nYSize; y++)
                    {
                        TileData tempTile = new TileData((workMap[x, y]) ? eTileType.Grass : eTileType.Water, 0);
                        mapTable.SetTile(tempTile, x, y);
                    }
                }
                return mapTable;
            }

            private static bool[,] CreateGeometry(bool[,] orgMap,int nXSize,int nYSize,bool bOut = false)
            {
                bool[,] tempMap = new bool[nXSize, nYSize];
                for (int x = 0; x < nXSize; x++)
                {
                    for (int y = 0; y < nYSize; y++)
                    {
                        int cnt = 0;

                        for (int xFind = x - mergeRange; xFind < x + mergeRange; xFind++)
                        {
                            if (xFind < 0 || xFind >= nXSize)
                            {
                                if (bOut)
                                {
                                    cnt+=mergeRange*2;
                                }
                                continue;
                            }
                            for (int yFind = y - mergeRange; yFind < y + mergeRange; yFind++)
                            {
                                if (yFind < 0 || yFind >= nYSize)
                                {
                                    if (bOut)
                                    {
                                        cnt ++;
                                    }
                                    continue;
                                }
                                if (orgMap[xFind, yFind])
                                {
                                    cnt++;
                                    if (cnt >= mergePoint) break;
                                }
                            }
                            if (cnt >= mergePoint) break;
                        }
                        tempMap[x, y] = cnt >= mergePoint;
                    }
                }
                return tempMap;
            }
        }
    }
}