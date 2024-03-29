﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys;
using GameSys.Lib;
using GameSys.Item;

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
            private int nX;
            private int nY;
            private int nHeight;
            private byte[] b_Stratum = new byte[12];
            //0~1심층 2~3지하(바다) 4~7평지 8~11산


            public TileData(int nX,int nY)
            {
                this.nX = nX;
                this.nY = nY;
               this.nHeight = 4;
               for(int z = 0;z < 12; z++)
                {
                    b_Stratum[z] = 0;
                }
            }
            
            public int Height
            {
                get { return nHeight; }
                set
                {
                    if (value > 11)
                        nHeight = 11;
                    else
                        nHeight = value;
                    for (int z = 0; z < nHeight; z++)
                    {
                        b_Stratum[z] = 2;
                    }
                }
            }
            
            public void CoverDirt()
            {
                for (int z = nHeight-2; z < nHeight; z++)
                {
                    b_Stratum[z] = 1;
                }
            }

            public int X
            {
                get { return nX; }
            }
            public int Y
            {
                get { return nY; }
            }

            public List<byte> Stratum
            {
                get
                {
                    List<byte> stratum = new List<byte>();
                    for(int z = 0; z < Height; z++)
                    {
                        stratum.Add(b_Stratum[z]);
                    }
                    return stratum;
                }
            }
        }

        public class Chunk
        {
            private int nX;
            private int nY;
            private eGeoType eGeo;
            private TileData[,] tiles;

            public Chunk(int nX,int nY)
            {
                this.nX = nX;
                this.nY = nY;
                tiles = new TileData[8, 8];
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        tiles[x, y] = new TileData(nX * 8 + x, nY * 8 + y);
                    }
                }
                eGeo = eGeoType.Flat;
            }

            public void GenerateTile(eGeoType eGeo)
            {
                this.eGeo = eGeo;
                switch (eGeo)
                {
                    case eGeoType.Flat:
                        for(int x = 0; x < 8; x++)
                        {
                            for(int y = 0; y < 8; y++)
                            {
                                tiles[x, y].Height=5;
                                tiles[x, y].CoverDirt();
                            }
                        }
                        break;
                    case eGeoType.Hill:
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                tiles[x, y].Height = 11;
                                tiles[x, y].CoverDirt();
                            }
                        }
                        break;
                    case eGeoType.Sea:
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                tiles[x, y].Height = 2;
                                tiles[x, y].CoverDirt();
                            }
                        }
                        break;
                    case eGeoType.Water:
                        break;
                }
            }

            public List<TileData> TileList
            {
                get
                {
                    List<TileData> tiles = new List<TileData>();

                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            tiles.Add(this.tiles[x, y]);
                        }
                    }

                    return tiles;
                }
            }
        }

        /// <summary>
        /// 맵 테이블
        /// 타일 뭉쳐둔 데이터 구조
        /// </summary>
        public class MapTable
        {
            private int nXSize;
            private int nYSize;

            private Chunk[,] Chunks;

            public MapTable(int nXSize, int nYSize)
            {
                this.nXSize = nXSize / 8;
                this.nYSize = nYSize / 8;
                Chunks = new Chunk[this.nXSize, this.nYSize];
                for (int x = 0; x < this.nXSize; x++)
                {
                    for (int y = 0; y < this.nYSize; y++)
                    {
                        Chunks[x, y] = new Chunk(x,y);
                    }
                }
            }

            public int XSize
            {
                get { return nXSize; }
            }

            public int YSize
            {
                get { return nYSize; }
            }
            
            public List<TileData> TileList
            {
                get
                {
                    List<TileData> tiles = new List<TileData>();

                    for (int x = 0; x < this.nXSize; x++)
                    {
                        for (int y = 0; y < this.nYSize; y++)
                        {
                            tiles.AddRange(this.Chunks[x, y].TileList);
                        }
                    }

                    return tiles;
                }
            }

            public void GenerateChunk()
            {
                int[,] temp = new int[XSize, YSize];

                for (int x = 0; x < XSize; x++)
                {
                    for (int y = 0; y < YSize; y++)
                    {
                        int num = Random.Range(0, 100);
                        if (num >= 35)
                            num = 3;
                        else if (num > 25)
                            num = 0;
                        else
                            num = 1;
                        temp[x, y] = num;
                    }
                }
                temp = CreateMap.CellularAutomata(temp, XSize, YSize, 0, 2,1, true);

                int cnt = 0;
                foreach(Chunk chunk in Chunks)
                {
                    chunk.GenerateTile((eGeoType)temp[cnt%XSize,cnt/XSize]);
                    cnt++;
                }
            }

            public void Smoothing()
            {
                int[,] nGeo = new int[nXSize * 8, nYSize * 8];
                foreach (TileData tile in TileList)
                {
                    nGeo[tile.X, tile.Y] = tile.Height;
                }

                nGeo = CreateMap.CellularAutomata(nGeo, nXSize * 8, nYSize * 8,2, 2, 4, true);

                foreach (TileData tile in TileList)
                {
                    tile.Height = nGeo[tile.X, tile.Y];
                    tile.CoverDirt();
                }
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
            private static int mergeRepeat = 3; // 반복 횟수
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

                mapTable.GenerateChunk();

                mapTable.Smoothing();
                
                return mapTable;
            }

            public static int[,] CellularAutomata(int[,] matrix,int xSize,int ySize, int defaultValue, int repeat ,int range , bool isBorder)
            {
                int[,] temp = new int[xSize, ySize];

                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        temp[x, y] = defaultValue;
                    }
                }

                for (int xPos = 0; xPos < xSize; xPos++)
                {
                    for (int yPos = 0; yPos < ySize; yPos++)
                    {
                        if ((xPos == 0 || yPos == 0 || xPos == xSize - 1 || yPos == ySize - 1) && isBorder)
                        {
                            temp[xPos, yPos] = defaultValue;
                            continue;
                        }
                        List<int> valueList = new List<int>();
                        for (int x = xPos - range; x <= xPos + range; x++)
                        {
                            for (int y = yPos - range; y <= yPos + range; y++)
                            {
                                if(x <= 0 || y <= 0 || x >= xSize-1 || y >= ySize-1)
                                {
                                    valueList.Add(defaultValue);
                                    continue;
                                }
                                valueList.Add(matrix[x, y]);
                            }
                        }
                        int total =0 ;
                        foreach(int value in valueList)
                        {
                            total += value;
                        }
                        temp[xPos, yPos] = (int)(total / valueList.Count);
                    }
                }

                if (repeat <= 1)
                    return temp;
                else
                    return CellularAutomata(temp, xSize, ySize, defaultValue, --repeat,range, isBorder);
            }



            /*
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
                             tempTile = new TileData(eTileType.Ground, 0);
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
                            
                            tempMap[x, y] = Aproach(eTileType.Ground | eTileType.Stone,x, y,mergePoint,mergeRange,bOut);
                        }
                    }

                    for (int x = 0; x < mapTable.nXSize; x++)
                    {
                        for (int y = 0; y < mapTable.nYSize; y++)
                        {
                            if(tempMap[x, y])
                            {
                                mapTable.GetTile(x, y).SetTileType(eTileType.Ground);
                            }
                            else
                            {
                                mapTable.GetTile(x, y).SetTileType(eTileType.Water);
                            }
                        }
                    }
                }
                return true;
            }

            /// <summary>
            /// 인접 특정 타일 분포율 검사 메서드
            /// </summary>
            /// <param name="x">검사 원점x</param>
            /// <param name="y">검사 원점y</param>
            /// <param name="point">기준 분포율</param>
            /// <param name="range">검사 범위</param>
            /// <param name="bOut">맵 외각 처리 결정 변수</param>
            /// <param name="type">검사 타일 타입</param>
            /// <returns>인접 특정 타일 분포율이 기준 분포율 초과시 true</returns>
            private static bool Aproach(eTileType type, int x,int y ,int point,int range,bool bOut)
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
                        if (type == eTileType.GroundOrStone)
                        {
                            if (mapTable.GetTile(xFind, yFind).GetTileType() == (int)eTileType.Ground || mapTable.GetTile(xFind, yFind).GetTileType() == (int)eTileType.Stone)
                            {
                                cnt++;
                                if (cnt >= point) break;
                            }
                        }
                        else
                        {
                            if (mapTable.GetTile(xFind, yFind).GetTileType() == (int)type)
                            {
                                cnt++;
                                if (cnt >= point) break;
                            }
                        }
                    }
                    if (cnt >= point) break;
                }
                return point <= cnt;
            }
            
            /// <summary>
            /// 인접 특정 요소 분포율 검사 메서드
            /// </summary>
            /// <param name="map">검사 배열</param>
            /// <param name="nType">검사 요소 값</param>
            /// <param name="x">원점x</param>
            /// <param name="y">원점y</param>
            /// <param name="nXSize">x사이즈</param>
            /// <param name="nYSize">y사이즈</param>
            /// <param name="point">기준 분포율</param>
            /// <param name="range">검사 범위</param>
            /// <returns></returns>
            private static bool Aproach(int[,] map, int nType,int x,int y,int nXSize,int nYSize,int point,int range)
            {
                int nCnt = 0;
                for(int nX=(x-range<0?0:x-range);nX < (x + range >= nXSize ? nXSize : x + range); nX++)
                {
                    for (int nY = (y - range < 0 ? 0 : y - range); nY < (y + range >= nYSize ? nYSize : y + range); nY++)
                    {
                        if(map[nX,nY]== nType)
                        {
                            nCnt++;
                        }
                    }
                }
                if (nCnt > point)
                    return true;
                else
                    return false;
            }

            /// <summary>
            /// 지형 높낮이 조정 메서드
            /// </summary>
            /// <returns>문제 없으면 true</returns>
            private static bool HeigherGeometry()
            {
                float Height ;
                float Weight ;
                float nX ;
                float nY ;
                
                List<Vector2> top = new List<Vector2>();
                
                for (int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {
                        if (Aproach(eTileType.Ground | eTileType.Stone,x, y, 64,4, false))
                        {
                            if (rand.Next(1, 101) < 8)
                            {
                                top.Add(new Vector2(x, y));//정상 지정 다음에 할일은 확률적으로 정상 선별하고 ㅡㄱ것을 바탕으로 산맥 생성
                            }
                        }
                    }
                }

                
                //산 생성
                foreach(Vector2 topPoint in top)
                {
                    Height = rand.Next(1, 8);//정상지점 높이
                    Weight = rand.Next(4, 14);//산의 반지름
                    nX = topPoint.x;//정상의 x좌표
                    nY = topPoint.y;//정상의 y좌표
                    float gradent = -Height / Weight;
                    float dis;//선택된 점과 정상의 거리
                    for (int x = (int)(nX-Weight);x < nX + Weight; x++)
                    {
                        if (x < 0 || x >= mapTable.nXSize)
                            continue;
                        for (int y = (int)(nY - Weight); y < nY + Weight; y++)
                        {
                            if (y < 0 || y >= mapTable.nYSize)
                                continue;
                            TileData curTile = mapTable.GetTile(x, y);
                            if (curTile.GetTileType()== (int)eTileType.Water)
                                continue;
                            Vector2 temp = new Vector2(x, y);
                            dis = Vector2.Distance(temp, topPoint);
                            int thisHeight = (int)(dis * gradent+Height);

                            

                            if (thisHeight < 0)
                                thisHeight = 0;

                            if (curTile.GetHeight()!=0)
                            {
                                if(curTile.GetHeight() > thisHeight)
                                {
                                    curTile.SetHeight(curTile.GetHeight());
                                }
                                else
                                {
                                    curTile.SetHeight(thisHeight);
                                    if (thisHeight > 4 && thisHeight >= Height - 1)
                                    {
                                        curTile.SetTileType(eTileType.Stone);
                                    }
                                }
                            }
                            else
                            {
                                curTile.SetHeight(thisHeight);
                                if( thisHeight>1 && thisHeight >= Height - 1)
                                {
                                    curTile.SetTileType(eTileType.Stone);
                                }
                            }

                            
                        }
                    }
                    
                }

                return true;
            }

            /// <summary>
            /// 강생성 메서드
            /// </summary>
            /// <param name="nAmount">강의 최대 갯수</param>
            /// <returns></returns>
            private static bool CreateRiver(int nAmount)
            {
                List<Vector2> top = new List<Vector2>();

                for (int x = 10; x < mapTable.nXSize-10 ; x++)
                {
                    for (int y = 10; y < mapTable.nYSize-10; y++)
                    {
                        if(mapTable.GetTile(x,y).GetTileType() ==(int) eTileType.Stone)
                        {
                            top.Add(new Vector2(x, y));
                        }
                    }
                }
                
                for(int i = 0; i < top.Count; i++)
                {
                    int r1 = rand.Next(0, top.Count);
                    int r2 = rand.Next(0, top.Count);

                    Vector2 temp = top[r1];
                    top[r1] = top[r2];
                    top[r2] = temp;
                }

                while(top.Count > nAmount)
                {
                    top.RemoveAt(0);
                }

                foreach(Vector2 orgP in top)
                {
                    Vector2 curP = new Vector2(orgP.x,orgP.y);
                    Vector2 preP = new Vector2(-1,-1);
                    int nX;
                    int nY;
                    int[] bDir = { 0, 0, 0, 0 };


                    int nHeight;
                    int nFind;

                    int nCnt = 0;
                    while (true)
                    {
                        nCnt++;
                        nX = -1;
                        nY = -1;
                        nFind = 0;
                        if (nCnt > 400)
                        {
                            break;
                        }
                        nHeight = mapTable.GetTile((int)curP.x, (int)curP.y).GetHeight();
                        if (nHeight == 0 && mapTable.GetTile((int)curP.x, (int)curP.y).GetTileType()==(int)eTileType.Water)
                        {

                            break;
                        }
                        for(int i = 0; i < 4; i++)
                        {
                            bDir[i] = 0;
                            switch (i)
                            {
                                case 0:
                                    nX = -1;
                                    nY = 0;
                                    break;
                                case 1:
                                    nX = 0;
                                    nY = -1;
                                    break;
                                case 2:
                                    nX = 1;
                                    nY = 0;
                                    break;
                                case 3:
                                    nX = 0;
                                    nY = 1;
                                    break;
                            }
                           
                            if (curP.x + nX < 0 || curP.x + nX >= mapTable.nXSize || curP.y + nY < 0 || curP.y + nY >= mapTable.nYSize)
                            {
                                bDir[i] = 1000;
                                continue;
                            }
                             int tempH = mapTable.GetTile((int)curP.x + nX, (int)curP.y + nY).GetHeight();
                            if (curP.x + nX == preP.x && curP.y +nY == preP.y)
                            {
                                bDir[i] = 500;
                                continue;
                            }
                            if (mapTable.GetTile((int)curP.x + nX, (int)curP.y + nY).GetTileType() == (int)eTileType.Water)
                            {
                                bDir[i] = 100;
                                continue;
                            }
                            bDir[i] = tempH;
                        }

                        if (bDir[0] > 90 && bDir[1] > 90 && bDir[2] > 90 && bDir[3] > 90)
                            break;
                        if (bDir[0] > nHeight && bDir[1] > nHeight && bDir[2] > nHeight && bDir[3] > nHeight)
                            break;
                        for (int i = 0; i < 4; i++)
                        {
                            if (bDir[nFind] > bDir[i])
                            {
                                nFind = i;
                            }
                            else if (bDir[nFind] == bDir[i])
                            {
                                if (rand.Next(1, 101) < 33)
                                {
                                    nFind = i;
                                }
                            }
                        }
                        switch (nFind)
                        {
                            case 0:
                                nX = -1;
                                nY = 0;
                                break;
                            case 1:
                                nX = 0;
                                nY = -1;
                                break;
                            case 2:
                                nX = 1;
                                nY = 0;
                                break;
                            case 3:
                                nX = 0;
                                nY = 1;
                                break;
                        }
                        mapTable.GetTile((int)curP.x, (int)curP.y).SetTileType(eTileType.Water);
                        mapTable.GetTile((int)curP.x, (int)curP.y).SetHeight(mapTable.GetTile((int)curP.x, (int)curP.y).GetHeight()!=0? mapTable.GetTile((int)curP.x, (int)curP.y).GetHeight()-1:0);
                        preP = new Vector2(curP.x,curP.y);
                        curP = new Vector2(curP.x + nX, curP.y + nY);

                    }

                }

                return true;
            }

            /// <summary>
            /// 식물 심기 메서드
            /// </summary>
            /// <returns></returns>
            private static bool PlantsPlant(int nRepeat)
            {
                int[,] PlantsMap = new int[mapTable.nXSize, mapTable.nYSize];
                for (int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {

                        mapTable.GetTile(x, y).SetPlants(ePlants.Null);
                        int nTType = mapTable.GetTile(x, y).GetTileType();
                        if (nTType == (int)eTileType.Ground)
                        {
                            if (Aproach(eTileType.Water,x, y, 12,8, false))
                            {
                                if (rand.Next(1, 101) < 50)
                                {
                                    PlantsMap[x, y] = 1;
                                }
                                
                            }
                        }
                        else if (nTType == (int)eTileType.Water)
                        {
                            PlantsMap[x, y] = 1;
                        }
                    }
                }

                int[,] tempMap = new int[mapTable.nXSize, mapTable.nYSize];

                for (int c = 0; c < nRepeat; c++)
                {
                    for (int x = 0; x < mapTable.nXSize; x++)
                    {
                        for (int y = 0; y < mapTable.nYSize; y++)
                        {
                            if (mapTable.GetTile(x, y).GetTileType() == (int)eTileType.Ground)
                            {
                                if (Aproach(PlantsMap, 1, x, y, mapTable.nXSize, mapTable.nYSize, 8, 2))
                                {
                                    tempMap[x, y] = 1;
                                }
                            }
                        }
                    }

                    for (int x = 0; x < mapTable.nXSize; x++)
                    {
                        for (int y = 0; y < mapTable.nYSize; y++)
                        {
                            PlantsMap[x, y] = tempMap[x, y];
                        }
                    }
                }

                for (int x = 0; x < mapTable.nXSize; x++)
                {
                    for (int y = 0; y < mapTable.nYSize; y++)
                    {
                        if (PlantsMap[x, y] == 1)
                        {
                            if(mapTable.GetTile(x, y).GetTileType()!=(int)eTileType.Water)
                                mapTable.GetTile(x, y).SetPlants(ePlants.Grass);
                        }
                    }
                }
                

                return true;
            }

            */
        }
    }
}