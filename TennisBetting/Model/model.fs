namespace TennisBetting

open System

 type record ={  ATP :int32; Location:string;Tournament:string;Date:string;Series:string;Court:string;Surface:string;
                Round:string;Bestof:int32;Winner:string;Loser:string;WRank:int32;LRank:int32;W1:int32;L1:int32;W2:int32;L2:int32;W3:Nullable<int32>;L3:Nullable<int32>;W4:Nullable<int32>;L4:Nullable<int32>;W5:Nullable<int32>;L5:Nullable<int32>;
                Wsets:int32;Lsets:int32;Comment:string;
                
                CBW:Nullable<int32>;CBL:Nullable<int32>;GBW:Nullable<int32>;GBL:Nullable<int32>;IWW:Nullable<int32>;IWL:Nullable<int32>;SBW:Nullable<int32>;
                SBL:Nullable<int32>;B365W:Nullable<int32>;B365L:Nullable<int32>;BAndWW:Nullable<int32>;BAndWL:Nullable<int32>;EXW:Nullable<int32>;EXL:Nullable<int32>;PSW:Nullable<int32>;PSL:Nullable<int32>;
                WPts:Nullable<int32>;LPts:Nullable<int32>;UBW:Nullable<int32>;UBL:Nullable<int32>;LBW:Nullable<int32>;LBL:Nullable<int32>;SJW:Nullable<int32>;SJL:Nullable<int32>;

                MaxW:Nullable<int32>;MaxL:Nullable<int32>;AvgW:Nullable<int32>;AvgL:Nullable<int32>}

 type Tournament ={ atp:int64;location:string;name:string }

