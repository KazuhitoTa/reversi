using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using osero;
using System;

namespace osero
{
    public class OseroManageMent : MonoBehaviourPunCallbacks
    {
        ExitGames.Client.Photon.Hashtable roomHash;
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
        public Vector3 Black=new Vector3(0,0,0);//黒
        public Vector3 Kaiten=new Vector3(0,0,180);//白
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
        public bool ClickLink;
        public GameObject t1;
        public GameObject t2;

        
        

        void Awake()
        {
            MarkerSetUp();//マーカーのポジションを取得
            DiskInstantiate();//ディスクを生成する
            DiskPrepare();//ディスクを初期化する
        }
        
       
        void Update()
        {
            DiskPut();//オセロの駒を置く
            GameEnd();//ゲームが終わった時の処理
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
        public void test()
        {
            int g=0;
            for(int y=0;y<8;y++)
            {
                for(int x=0;x<8;x++)
                {
                    Disk[y,x]=GameObject.FindGameObjectsWithTag("Disk")[g];
                    Disk[y,x].transform.parent=Marker[y,x].transform;
                    g+=1;
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
                    Disk[y,x].transform.parent=Marker[y,x].transform;
                }        
            }        
        }

        /// <summary>
        /// ディスクを初期化する
        /// </summary>
        public void DiskPrepare()
        {
            WhiteCount=2;//白の数を初期化
            BlackCount=2;//黒の数を初期化
            EmptyCount=60;//空の数を初期化
            for(int y=0;y<8;y++)
            {
                for(int x=0;x<8;x++)
                {
                    DiskStateManager[y,x]=DiskState.EMPTY;//すべての駒を空状態に
                    Disk[y,x].SetActive(false);//すべての駒を非アクティブに
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
        }

        [PunRPC]
        void Link(bool CL)
        {
            ClickLink=CL;
            Debug.Log(CL);
        }
        
        /// <summary>
        /// オセロの駒を置く
        /// </summary>
        
        
        void DiskPut()
        {
            // //マウスの左ボタンを押したら 
            if (!Input.GetMouseButtonDown(0))return;
          
            GetClickObj();//クリックしたオブジェクトをclickedGameObjectに入れる

            if(clickedGameObject==null)return;//clickedGameObjectが空

            GamePlay=false;//ゲームを開始
            //クリックオブジェクトの子供の情報を取得
            clickedDisk=clickedGameObject.transform.GetChild(0).gameObject; 
            DiskTurnBlack();
            DiskTurnWhite();
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
            for(int a=-1;a<2;a++)
            {
                for(int b=-1;b<2;b++)
                {
                    //周りに駒があって自分の駒以外なら
                    if(a+PutDisktmp1>=0&&a+PutDisktmp1<8
                    &&b+PutDisktmp2>=0&&b+PutDisktmp2<8
                    &&!(a==0&&b==0))
                    {
                        if(DiskStateManager[a+PutDisktmp1,b+PutDisktmp2]!=DiskStateManager[PutDisktmp1,PutDisktmp2]
                        &&(DiskStateManager[a+PutDisktmp1,b+PutDisktmp2]!=DiskState.EMPTY))
                        {
                            int k=a+PutDisktmp1;
                            int l=b+PutDisktmp2;

                            //横
                            if(a!=0&&b==0)
                            {
                                for(int h=a+PutDisktmp1;h>=0&&h<=7;h=h+a)
                                {
                                    if(DiskStateManager[h,b+PutDisktmp2]==DiskStateManager[PutDisktmp1,PutDisktmp2])
                                    {
                                        PutOK=true;
                                        Okeru=true;
                                    }
                                }
                            }
                            //縦
                            else if(a==0&&b!=0)
                            {
                                for(int h=b+PutDisktmp2;h>=0&&h<=7;h+=b)
                                {
                                    if(DiskStateManager[a+PutDisktmp1,h]==DiskStateManager[PutDisktmp1,PutDisktmp2])
                                    {
                                        PutOK=true;
                                        Okeru=true;
                                    }
                                }
                            }
                            //斜め
                            else if(a!=0&&b!=0)
                            {
                               for(int g=a+PutDisktmp1,h=b+PutDisktmp2;g>=0&&g<=7&&h>=0&&h<=7;g+=a,h+=b)
                               {
                                   if(DiskStateManager[g,h]==DiskStateManager[PutDisktmp1,PutDisktmp2])
                                    {
                                        PutOK=true;
                                        Okeru=true;
                                    }
                               }
                            }

                            if(PutOK==true)
                            {
                                while(DiskStateManager[k,l]==DiskStateManager[a+PutDisktmp1,b+PutDisktmp2])
                                {
                                    ReverseDisktmp1.Add(k);
                                    ReverseDisktmp2.Add(l);
                                    ReverseDisk.Add(Disk[k,l]);
                                    k+=a;
                                    l+=b;
                                }
                                DiskMove();
                                PutOK=false;
                            }


                        }
                    }

                }
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
                if(DiskStateManager[ReverseDisktmp1[i],ReverseDisktmp2[i]]==DiskState.BLACK)
                DiskStateManager[ReverseDisktmp1[i],ReverseDisktmp2[i]]=DiskState.WHITE;
                else DiskStateManager[ReverseDisktmp1[i],ReverseDisktmp2[i]]=DiskState.BLACK;
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

        /// <summary>
        /// もし黒のTurnなら
        /// </summary>
        void DiskTurnBlack()
        {
            //もし黒のターンで子供が非アクティブなら
            if(TrunStateManager!=TrunState.BlackTrun||clickedDisk.activeInHierarchy)return;

            //置いたコマの配列での座標を一時的に保持する
            DiskTmp();
            //置いた駒の状態を黒に変更
            DiskStateManager[PutDisktmp1,PutDisktmp2]=DiskState.BLACK;
            //置いたコマの周りの8マスの状態を確認
            FindAround();
            if(Okeru)
            {
                tran.text=TrunStateManager==TrunState.BlackTrun?"whiteTurn":"BlackTurn";
                //子供の状態をアクティブする
                clickedDisk.SetActive(true);
                //黒の駒の数を１つ増やす
                BlackCount++;
                //空の駒の数を１つ減らす
                EmptyCount--;
                //白のターンにする
                TrunStateManager=TrunState.WhiteTurn;
                Okeru=false;
            }
            else DiskStateManager[PutDisktmp1,PutDisktmp2]=DiskState.EMPTY;


        }
        /// <summary>
        /// もし白のTurnなら
        /// </summary>
        void DiskTurnWhite()
        {
            
             //もし白のターンで子供が非アクティブなら
            if(TrunStateManager!=TrunState.WhiteTurn||clickedDisk.activeInHierarchy)return;


            //置いたコマの配列での座標を一時的に保持する
            DiskTmp();
            //置いた駒の状態を白に変更
            DiskStateManager[PutDisktmp1,PutDisktmp2]=DiskState.WHITE;
            //置いたコマの周りの8マスの状態を確認
            FindAround();
            if(Okeru)
            {
                tran.text=TrunStateManager==TrunState.BlackTrun?"whiteTurn":"BlackTurn";
                //クリックした駒を白に変更
                clickedDisk.transform.Rotate(Kaiten);
                //子供の状態をアクティブする
                clickedDisk.SetActive(true);
                //白の駒の数を１つ増やす
                WhiteCount++;
                //空の駒の数を１つ減らす
                EmptyCount--;
                //黒のターンにする
                TrunStateManager=TrunState.BlackTrun;
                Okeru=false;
            }
            else DiskStateManager[PutDisktmp1,PutDisktmp2]=DiskState.EMPTY;


        }


        /// <summary>
        /// ゲームが終わった時の処理
        /// </summary>
        void GameEnd()
        {
            if(EmptyCount==0&&!GamePlay)
            {
                Debug.Log("白が"+WhiteCount);
                Debug.Log("黒が"+BlackCount);
                DiskPrepare();
                GamePlay=true;
            }
        }
    }
}

