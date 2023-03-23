using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;



namespace osero
{
    public class CPUOseroManager : MonoBehaviourPunCallbacks
    {
        public GameObject DiskPre;//オセロの駒のプレハブ
        public int EmptyCount;//どちらも駒をおいていない場所の数
        public int BlackCount;//黒の駒の数
        public int WhiteCount;//白の駒の数

        public enum DiskState
        {
            EMPTY,//石が空
            WHITE,//石の上が白
            BLACK//石の上が黒
        };

        public enum TrunState
        {
            BlackTrun,//黒のターン
            WhiteTurn//白のターン
        };

        public Vector3 Kaiten=new Vector3(0,0,180);//回転
        public GameObject MarkerManger;//マーカーの親オブジェクト
        public Transform[,] MarkerPos=new Transform[8,8];//マーカーのポジションを保持
        public GameObject[,] Marker=new GameObject[8,8];//マーカーのポジションを保持
        public GameObject[,] Disk=new GameObject[8,8]; //オセロの駒の位置管理
        private GameObject clickedGameObject;//クリックしたオブジェクトを保持 
        [Header("どっちのターンか")] public TrunState TrunStateManager;//ゲームの状態
        [Header("オセロの状態はどうか")] public DiskState[,] DiskStateManager=new DiskState[8,8];//石の状態
        private bool GamePlay;//ゲームが終わったチェックする
        public int PutDisktmp1;//置いた駒の横の場所
        public int PutDisktmp2;//置いた駒の縦の場所
        public GameObject clickedDisk;//押した駒本体
        public List<GameObject> ReverseDisk = new List<GameObject>();
        public List<int> ReverseDisktmp1 = new List<int>();
        public List<int> ReverseDisktmp2 = new List<int>();
        public bool PutOK;
        public bool Okeru;
        public TextMeshProUGUI tran;
        public bool[,] Check=new bool[8,8];
        public GameObject okerunPre;
        public GameObject[,] okerun=new GameObject[8,8];
        public bool SkipCheck;
        public bool EndCheck;
        public List<GameObject> AIDiskTMP = new List<GameObject>();
        public List<int> AIDisktmp1 = new List<int>();
        public List<int> AIDisktmp2 = new List<int>();



        
        

        void Awake()
        {
            MarkerSetUp();//マーカーのポジションを取得
            DiskInstantiate();//ディスクを生成する
            DiskPrepare();//ディスクを初期化する
        }
        
        void Update()
        {
            DiskPut();//オセロの駒を置く
        }
        
        /// <summary>
        /// マーカーのTransfromポジションを取得する
        /// </summary>
        public void MarkerSetUp()
        {
            int a=0;
            for(int y=0;y<8;y++)
            {
                for(int i=0;i<8;i++)
                {
                    Marker[y,i]=MarkerManger.transform.GetChild(a).gameObject;
                    MarkerPos[y,i]=MarkerManger.transform.GetChild(a).transform;
                    a+=1; 
                }
            }
        }
        
        /// <summary>
        /// ディスクを配置する
        /// </summary>
        void DiskInstantiate()
        {
            for(int y=0;y<8;y++)
            {
                for(int x=0;x<8;x++)
                {
                    Disk[y,x]=Instantiate(DiskPre,MarkerPos[y,x]);
                    okerun[y,x]=Instantiate(okerunPre,MarkerPos[y,x]);
                    Disk[y,x].transform.parent=Marker[y,x].transform;
                    okerun[y,x].transform.parent=Marker[y,x].transform;
                }        
            }        
        }

        /// <summary>
        /// ディスクを初期化する
        /// </summary>
        public void DiskPrepare()
        {
            EndCheck=false;
            SkipCheck=false;
            WhiteCount=2;//白の数を初期化
            BlackCount=2;//黒の数を初期化
            EmptyCount=60;//空の数を初期化
            TrunStateManager = TrunState.BlackTrun;
            tran.text="BlackTurn";
            for(int y=0;y<8;y++)
            {
                for(int x=0;x<8;x++)
                {
                    if(Disk[y,x].transform.localEulerAngles.z>=180.0f)Disk[y,x].transform.Rotate(Kaiten);//すべての駒を黒に
                    DiskStateManager[y,x]=DiskState.EMPTY;//すべての駒を空状態に
                    Disk[y,x].SetActive(false);//すべての駒を非アクティブに
                    okerun[y,x].SetActive(false);//置ける場所のマーカーを非表示に
                }        
            }

            for(int a=3;a<5;a++)
            {
                for(int b=3;b<5;b++)
                {
                    Disk[a,b].SetActive(true);//真ん中４つの駒をアクティブに
                    if(a==b)
                    {
                        DiskStateManager[a,b]=DiskState.BLACK;//２つを黒に
                        if(Disk[a,b].transform.localEulerAngles.z>=180.0f)
                        {
                            Disk[a,b].transform.Rotate(Kaiten);//白にする
                        }
                    }
                    else 
                    {
                        DiskStateManager[a,b]=DiskState.WHITE;//２つを白に
                        if(Disk[a,b].transform.localEulerAngles.z<180.0f)
                        {
                            Disk[a,b].transform.Rotate(Kaiten);//白にする
                        }
                    }
                }
            }
            muritest();
        }

        
        /// <summary>
        /// オセロの駒を置く
        /// </summary>
        void DiskPut()
        {
            if(TrunStateManager == TrunState.WhiteTurn&&AIDiskTMP.Count!=0)
            {
                clickedGameObject=AIDiskTMP[Random.Range ((int)0,AIDiskTMP.Count)];

                clickedDisk=clickedGameObject.transform.GetChild(0).gameObject;//クリックオブジェクトの子供の情報を取得
    
                DiskTurn(TrunStateManager == TrunState.BlackTrun ? DiskState.BLACK : DiskState.WHITE);
            }

            // //マウスの左ボタンを押したら 
            if (Input.GetMouseButtonDown(0)&&TrunStateManager == TrunState.BlackTrun)
            {
                GetClickObj();//クリックしたオブジェクトをclickedGameObjectに入れる

                if(clickedGameObject==null)return;//clickedGameObjectが空

                if(GamePlay)GamePlay=false;//ゲームを開始
            
                clickedDisk=clickedGameObject.transform.GetChild(0).gameObject;//クリックオブジェクトの子供の情報を取得
    
                DiskTurn(TrunStateManager == TrunState.BlackTrun ? DiskState.BLACK : DiskState.WHITE);
            }    
        }

        /// <summary>
        /// クリックしたオブジェクトの情報を取得
        /// </summary>
        void GetClickObj()
        {
            clickedGameObject = null;
    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
    
            if (Physics.Raycast(ray, out hit)) 
            {
                clickedGameObject = hit.collider.gameObject;
            }

        }

        /// <summary>
        /// 置いた駒の周り８マスの色を調べる処理
        /// </summary>
        void FindAround()
        {
            //置いたマスの周り８マスを見る
            for(int a=-1;a<2;a++)
            {
                for(int b=-1;b<2;b++)
                {
                    //もし今置いたコマなら次に
                    if (a == 0 && b == 0)
                    {
                        continue;
                    }

                    //書くのがめんどくさいから置いたマスの横をｘ縦をｙと置く
                    int x = a + PutDisktmp1;
                    int y = b + PutDisktmp2;

                    //枠外からはみ出てたら次に
                    if (x < 0 || x > 7 || y < 0 || y > 7)
                    {
                        continue;
                    }

                    //同じく書くのがめんどいから
                    DiskState diskState = DiskStateManager[x, y];
                    DiskState putDiskState = DiskStateManager[PutDisktmp1, PutDisktmp2];

                    //置いたコマの状態が同じか空なら次に
                    if (diskState == putDiskState || diskState == DiskState.EMPTY)
                    {
                        continue;
                    }

                    int dx = a;
                    int dy = b;

                    while (true)
                    {
                        x += dx;
                        y += dy;

                        if (x < 0 || x > 7 || y < 0 || y > 7)
                        {
                            break;
                        }

                        diskState = DiskStateManager[x, y];

                        if (diskState == DiskState.EMPTY)
                        {
                            break;
                        }

                        if (diskState == putDiskState)
                        {
                            PutOK = true;
                            Okeru = true;

                            for (int i = x - dx, j = y - dy; (i != PutDisktmp1 || j != PutDisktmp2); i -= dx, j -= dy)
                            {
                                ReverseDisktmp1.Add(i);
                                ReverseDisktmp2.Add(j);
                                ReverseDisk.Add(Disk[i, j]);
                            }

                            break;
                        }
                    }

                }
            }

            if (PutOK)
            {
                DiskMove();
            }
        }

        /// <summary>
        /// 挟まれているオセロの駒をひっくり返す処理
        /// </summary>
        void DiskMove()
        {
            for(int i = 0; i < ReverseDisk.Count; i++) 
            {
                ReverseDisk[i].transform.Rotate(Kaiten);

                DiskStateManager[ReverseDisktmp1[i],ReverseDisktmp2[i]]
                =DiskStateManager[ReverseDisktmp1[i],ReverseDisktmp2[i]]==DiskState.BLACK?DiskState.WHITE:DiskState.BLACK;
                
            }
            ReverseDisk.Clear();
            ReverseDisktmp1.Clear();
            ReverseDisktmp2.Clear();
        }

        /// <summary>
        /// 置いたオセロの駒を配列内で探し一時的に保持するクラス
        /// </summary>
        void DiskTmp()
        {
            for(int y=0;y<8;y++)
            {
                for(int x=0;x<8;x++)
                {
                    if(clickedDisk==Disk[y,x])
                    {
                        PutDisktmp1=y;
                        PutDisktmp2=x;    
                    }
                }        
            }
        }

        
        void DiskTurn(DiskState state)
        {
            //もし状態に合わせてターンが異なる、または子供が非アクティブなら
            if ((state == DiskState.BLACK && TrunStateManager != TrunState.BlackTrun) ||
                (state == DiskState.WHITE && TrunStateManager != TrunState.WhiteTurn) ||
                clickedDisk.activeInHierarchy)
                return;

            //置いたコマの配列での座標を一時的に保持する
            DiskTmp();
            //置いた駒の状態を変更
            DiskStateManager[PutDisktmp1, PutDisktmp2] = state;
            //置いたコマの周りの8マスの状態を確認
            FindAround();
            if (Okeru)
            {
                tran.text = TrunStateManager == TrunState.BlackTrun ? "whiteTurn" : "BlackTurn";
                if (state == DiskState.WHITE)
                {
                    //クリックした駒を白に変更
                    clickedDisk.transform.Rotate(Kaiten);
                }
                
                //子供の状態をアクティブする
                clickedDisk.SetActive(true);
                //空の駒の数を１つ減らす
                EmptyCount--;
                //ターンを切り替える
                TrunStateManager = state == DiskState.BLACK ? TrunState.WhiteTurn : TrunState.BlackTrun;
                muritest();//置ける場所どこ？
            
                Okeru = false;
            }
            else
            {
                DiskStateManager[PutDisktmp1, PutDisktmp2] = DiskState.EMPTY;
            }
        }

        /// <summary>
        /// 駒がおけないときの処理
        /// </summary>
        void muri(int tx,int ty)
        {           
            for(int a=-1;a<2;a++)
            {
                for(int b=-1;b<2;b++)
                {
                    //もし今置いたコマなら次に
                    if (a == 0 && b == 0)
                    {
                        continue;
                    }

                    //書くのがめんどくさいから置いたマスの横をｘ縦をｙと置く
                    int x = a + tx;
                    int y = b + ty;

                    //枠外からはみ出てたら次に
                    if (x < 0 || x > 7 || y < 0 || y > 7)
                    {
                        continue;
                    }

                    //同じく書くのがめんどいから
                    DiskState diskState=DiskStateManager[x, y];
                    DiskState checkDiskState=TrunStateManager == TrunState.BlackTrun?DiskState.BLACK:DiskState.WHITE;
                    

                    //置いたコマの状態が同じか空なら次に
                    if (diskState == checkDiskState || diskState == DiskState.EMPTY)
                    {
                        continue;
                    }

                    int dx = a;
                    int dy = b;

                    while (true)
                    {
                        x += dx;
                        y += dy;

                        if (x < 0 || x > 7 || y < 0 || y > 7)
                        {
                            break;
                        }

                        diskState = DiskStateManager[x, y];

                        if (diskState == DiskState.EMPTY)
                        {
                            break;
                        }

                        if (diskState == checkDiskState)
                        {
                            Check[tx,ty]=true;

                            break;
                        }
                    }
                }
            }    
        }

        void muritest()
        {
            for(int g=0;g<8;g++)
            {
                for(int h=0;h<8;h++)
                {
                    if(DiskStateManager[g, h]==DiskState.EMPTY)muri(g,h);
                    if(Check[g,h])SkipCheck=true;
                    okerun[g,h].SetActive(Check[g,h]);
                    if(Check[g,h]&&TrunStateManager == TrunState.WhiteTurn)AIDiskTMP.Add(Marker[g,h]);
                    if(TrunStateManager == TrunState.BlackTrun)AIDiskTMP.Clear();
                    Check[g,h]=false;
                }
            }
            
            if(!SkipCheck)
            {
                TrunStateManager = TrunStateManager == TrunState.BlackTrun ? TrunState.WhiteTurn : TrunState.BlackTrun;
                tran.text = TrunStateManager == TrunState.BlackTrun ? "BlackTurn" : "WhiteTurn";
                EndCheck=true;
                
                for(int g=0;g<8;g++)
                {
                    for(int h=0;h<8;h++)
                    {
                        if(DiskStateManager[g, h]==DiskState.EMPTY)muri(g,h);
                        if(Check[g,h])SkipCheck=true;
                        okerun[g,h].SetActive(Check[g,h]);
                        if(Check[g,h]&&TrunStateManager == TrunState.WhiteTurn)
                        {
                            AIDiskTMP.Add(Marker[g,h]);
                        }
                        if(TrunStateManager == TrunState.BlackTrun)
                        {
                            AIDiskTMP.Clear();
                        }
                        Check[g,h]=false;
                    }
                }
                if(!SkipCheck)GameEnd();
                
            }
            SkipCheck=false;
        }

        /// <summary>
        /// 次の相手のターンの置ける場所が最小になるような位置に打つ
        /// </summary>
        void AImin()
        {
            
        }

        

        /// <summary>
        /// ゲームが終わった時の処理
        /// </summary>
        void GameEnd()
        {
            if(GamePlay)return;
            
            int whiteCount = 0;
            int blackCount = 0;

            foreach (DiskState diskState in DiskStateManager)
            {
                if (diskState == DiskState.WHITE)
                {
                    whiteCount++;
                }
                else if (diskState == DiskState.BLACK)
                {
                    blackCount++;
                }
            }
            
            Debug.Log("白が"+whiteCount);
            Debug.Log("黒が"+blackCount);
            Invoke("DiskPrepare", 5.0f);
            GamePlay=true;
            
        }
    }
}

