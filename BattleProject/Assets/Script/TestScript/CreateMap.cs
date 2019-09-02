using System.Collections;
using System.Collections.Generic;
using GameSys;
using GameSys.Lib;

namespace GameSys
{
    namespace Map
    {
        public class Point
        {
            public int nX;
            public int nY;

            public Point(int nX,int nY)
            {
                this.nX = nX;
                this.nY = nY;
            }
        }

        /// <summary>
        /// 타일 데이터 클래스
        /// 타일의 타입과 높이 등 정보를 보유
        /// </summary>
        public class TileData
        {
            public eTileType eType;
            public eResource eUnderRes;
            public int nHeight;

            public TileData(eTileType eType,int nHeight)
            {
                this.eType = eType;
                this.nHeight = nHeight;
            }

            public int GetType()
            {
                return (int)eType;
            }

            public void SetType(eTileType eType)
            {
                this.eType = eType;
            }

            public int GetHeight()
            {
                return nHeight;
            }

            public void SetHeight(int nHeight)
            {
                this.nHeight = nHeight;
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
            private static int mergeRepeat = 2; // 반복 횟수
            //황금비 40:25:4
            private static System.Random rand;

            /// <summary>
            /// 지형 생성 메서드
            /// </summary>
            /// <param name="nXSize">X사이즈</param>
            /// <param name="nYSize">Y사이즈</param>
            /// <param name="uiSeed">생성 시드</param>
            /// <returns></returns>
            public static MapTable Create(int nXSize, int nYSize, int uiSeed = 0)
            {
                if (uiSeed == 0)
                {
                    uiSeed = (int)System.DateTime.Now.Ticks;
                }
                rand = new System.Random(uiSeed);

                mapTable = new MapTable(nXSize, nYSize);

                CreateGeometry(mergeRepeat);
                HeigherGeometry();
                
                return mapTable;
            }

            /// <summary>
            /// 대륙 모양 생성 메서드
            /// </summary>
            /// <param name="orgMap">원본 모양</param>
            /// <param name="nXSize">X사이즈</param>
            /// <param name="nYSize">Y사이즈</param>
            /// <param name="nRepeat">반복횟수</param>
            /// <param name="bOut">바깥쪽 취급 설정</param>
            /// <returns></returns>
            private static bool CreateGeometry(int nRepeat,bool bOut = false)
            {
                for (int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {
                        TileData tempTile;
                        if (rand.Next(1, 101) < rate)
                        {
                             tempTile = new TileData(eTileType.Grass, 0);
                        }
                        else
                        {
                             tempTile = new TileData(eTileType.Water, 0);
                        }
                        mapTable.SetTile(tempTile, x, y);
                    }
                }
                bool[,] tempMap = new bool[mapTable.nXSize, mapTable.nYSize];

                for (int c = 0; c < nRepeat; c++)
                {
                    for (int x = 0; x < mapTable.nXSize; x++)
                    {
                        for (int y = 0; y < mapTable.nYSize; y++)
                        {
                            
                            tempMap[x, y] = Aproach(x,y,mergePoint,mergeRange,bOut,eTileType.Grass);
                        }
                    }

                    for (int x = 0; x < mapTable.nXSize; x++)
                    {
                        for (int y = 0; y < mapTable.nYSize; y++)
                        {
                            if(tempMap[x, y])
                            {
                                mapTable.GetTile(x, y).SetType(eTileType.Grass);
                            }
                            else
                            {
                                mapTable.GetTile(x, y).SetType(eTileType.Water);
                            }
                        }
                    }
                }
                return true;
            }

            private static bool Aproach( int x,int y ,int point,int range,bool bOut,eTileType type)
            {
                int cnt = 0;

                for (int xFind = x - range; xFind < x + range; xFind++)
                {
                    if (xFind < 0 || xFind >= mapTable.nXSize)
                    {
                        if (bOut)
                        {
                            cnt += range * 2;
                        }
                        continue;
                    }
                    for (int yFind = y - range; yFind < y + range; yFind++)
                    {
                        if (yFind < 0 || yFind >= mapTable.nYSize)
                        {
                            if (bOut)
                            {
                                cnt++;
                            }
                            continue;
                        }
                        if (mapTable.GetTile(xFind, yFind).GetType()==(int)type)
                        {
                            cnt++;
                            if (cnt >= point) break;
                        }
                    }
                    if (cnt >= point) break;
                }
                return point <= cnt;
            }
            
            private static bool HeigherGeometry()
            {
                int Height = 10;
                int Weight = 10;
                int nX = 50;
                int nY = 50;

                int[,] workMap = new int[mapTable.nXSize, mapTable.nYSize];
                List<Point> top = new List<Point>();

                for(int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {
                        workMap[x, y] = 0;
                    }
                }

                for (int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {
                        if (Aproach(x, y, 64, 4, false, eTileType.Grass))
                        {
                            top.Add(new Point(x, y));//정상 지정 다음에 할일은 확률적으로 정상 선별하고 ㅡㄱ것을 바탕으로 산맥 생성
                            mapTable.GetTile(x, y).SetHeight(10);
                        }
                    }
                }

                return true;
            }
        }
    }
}