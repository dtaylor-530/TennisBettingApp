namespace TennisBetting

open System.Data.SQLite
open Dapper
open System



module betfair =

  type Odds ={event_title:string; link:string; event_date:int64; A_name:string;A_odd:int64; B_name:string; B_odd:int64; datetime:int64}

  type Odds2 = {event_title:string; link:string; event_date:int64; A_name:string;A_odd:int64; B_name:string; B_odd:int64; datetime:int64; amount:int64}
 
      //Get premium
  let GetPremium (c:Odds)=  let winProbability = (100.0 / (c.A_odd |> double))
                            let loseProbability = (100.0 / (c.B_odd |> double))
                            let premium = (winProbability+ loseProbability + -1.0)
                            if premium <= 0.0 then 0.01 else premium

    // Get Ratio between max and min odds
  let GetRatio (c:Odds) =  let max = Math.Max(c.A_odd ,c.B_odd) |> float
                           let min = Math.Min(c.A_odd ,c.B_odd) |> float
                           1.0 * (max)/min
              

  let GetData =
    let databaseFilename = @"C:\Users\declan.taylor\Source\Repos\BetfairScraper\BetfairScraper\event_data.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   

    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 

    let filteredSql = "select [event_title],[link],[event_date],[A_name],[A_odd],[B_name],[B_odd],[datetime] from Odds where
                                strftime('%Y-%m-%d %H:%M:%S', event_date/10000000 - 62135596800, 'unixepoch')> datetime('now');"
    let results = connection.Query<Odds> filteredSql
    results


