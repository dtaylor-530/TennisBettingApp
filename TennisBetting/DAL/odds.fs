namespace TennisBetting

open System.Data.SQLite
open Dapper
open betfair


module odds =


    //System.String event_title, System.String link, System.Int64 event_date, System.String A_name, System.Int64 A_odd, System.String B_name, System.Int64 B_odd, System.Int64 datetime, System.Int64 amount

 let insertOdds (data:Odds) (amount:int) (premium:float)=
    let databaseFilename = "../../../Data/recommendations.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
    let famount = (amount |> float) 
    let prem =System.Math.Pow( premium, 3.0)
    let f2amount =(  famount / prem) |> int
    let odds= { event_title = data.event_title;link=data.link;event_date=data.event_date;A_name=data.A_name;A_odd=data.A_odd;B_name=data.B_name;B_odd=data.B_odd;datetime=data.datetime;amount=f2amount |> int64}

    if not (System.IO.File.Exists(databaseFilename))  then    
        SQLiteConnection.CreateFile(databaseFilename)

    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 

    //let structureSql =  "drop table  Odds2"
    //let structureCommand = new SQLiteCommand(structureSql, connection)
    //let result = structureCommand.ExecuteNonQuery() 

    let structureSql =  "create table if not exists  Odds2(event_title TEXT, link TEXT, event_date integer, A_name TEXT, A_odd integer, B_name TEXT, B_odd integer, datetime integer, amount integer)"
    let structureCommand = new SQLiteCommand(structureSql, connection)
    let result = structureCommand.ExecuteNonQuery() 

    let insertTradeSql = 
        "insert into Odds2(event_title, link, event_date, A_name, A_odd, B_name, B_odd, datetime, amount) " + 
        "values (@event_title, @link, @event_date, @A_name, @A_odd, @B_name, @B_odd, @datetime, @amount)"

    connection.Execute (insertTradeSql, odds)
   
 let getOdds() =
    let databaseFilename = "../../../TennisBettingApp/Data/recommendations.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   
    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 
    let filteredSql = "select [event_title],[link],[event_date],[A_name],[A_odd],[B_name],[B_odd],[datetime],[amount] from Odds2"
    let results = connection.Query<Odds2>(filteredSql)
    results
