namespace TennisBetting
open System.Data.SQLite
open Dapper
open common

module datapoints =
 open RegressionSmooth

 let insertPoints (data:seq<DataPoint>) =
    let databaseFilename = getFileName
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   

    if not (System.IO.File.Exists(databaseFilename))  then    
        SQLiteConnection.CreateFile(databaseFilename)


    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 



    let structureSql =  "create table if not exists DataPoint (atp integer,ratio integer,mean integer,variance integer,avgPremium integer,weight integer)"
    let structureCommand = new SQLiteCommand(structureSql, connection)
    let result = structureCommand.ExecuteNonQuery() 


   
    let insertTradeSql = 
        "insert into DataPoint(atp,ratio,mean,variance,avgPremium,weight)" + 
        "values(@atp,@ratio,@mean,@variance,@avgPremium,@weight)"

    //let outh = trainingSet |> Seq.collect (fun vl ->File2.GetOdds (fun c -> c.home) vl) |> Seq.map (fun g ->  mapq g (int64 0s)) |> Seq.toArray
    //Console.WriteLine(sprintf "Inserting %s of type home" (outh.Length.ToString()))
    connection.Execute (insertTradeSql, data)
    
 let getAllPoints =
    let databaseFilename = getFileName
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   
    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 
    let filteredSql = "select [atp],[ratio],[mean],[variance],[avgPremium],[weight] from DataPoint"
    let results = connection.Query<DataPoint> filteredSql
    results

 let getPoints (atp:int) =
    let databaseFilename =getFileName
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   
    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 
    let filteredSql = "select [atp],[ratio],[mean],[variance],[avgPremium],[weight] from DataPoint where atp = ?"
    let results = connection.Query<DataPoint>(filteredSql, atp)
    results