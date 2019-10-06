module tournaments

open System.Data.SQLite
open Dapper
open common

type Tournament ={ atp:int64;location:string;name:string }

 let insertTournaments data=
    let databaseFilename = getFileName
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   
    if not (System.IO.File.Exists(databaseFilename))  then    
        SQLiteConnection.CreateFile(databaseFilename)

    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 

    let structureSql =  "drop table Tournament"
    let structureCommand = new SQLiteCommand(structureSql, connection)
    let result = structureCommand.ExecuteNonQuery() 

    let structureSql =  "create table if not exists Tournament (atp integer, location text, name text)"
    let structureCommand = new SQLiteCommand(structureSql, connection)
    let result = structureCommand.ExecuteNonQuery() 

    let insertTradeSql = 
        "insert into Tournament(atp, location, name) " + 
        "values (@atp, @location, @name)"

    connection.Execute (insertTradeSql, data)
    
  let getTournaments =
    let databaseFilename =getFileName
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      
   
    let connection =  new SQLiteConnection(connectionStringFile)
    connection.Open() 
    let filteredSql = "select [atp],[location],[name] from Tournament"
    let results = connection.Query<Tournament>(filteredSql)
    results

   let getTournamentName atp =
     match  getTournaments |> Seq.tryFind (fun x -> x.atp =atp) with | Some x->x.name | None-> System.String.Empty