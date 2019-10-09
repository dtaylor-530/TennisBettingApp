namespace TennisBetting

open System.Data.SQLite
open Dapper
open System


type result ={ link:string; event_date:int64; winner:string}


module results = 


  let GetData =
    let databaseFilename = @"C:\Users\declan.taylor\Source\Repos\BetfairScraper\BetfairScraper\event_data.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename      

    let connection =  new SQLiteConnection(connectionStringFile) 
    connection.Open() 

    let filteredSql = "select [link],[event_date],[winner] from Results"
    let results = connection.Query<result> filteredSql
    results

