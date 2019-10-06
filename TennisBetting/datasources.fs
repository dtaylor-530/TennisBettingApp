namespace TennisBetting

open System

type DataSource ={Betfair:string; TennisData:string}
open FSharp.Data
open tournaments
open Dapper
open common

module datasources =
    open System.Data.SQLite
    


    let data = CsvFile.Load(getFileName2 "DataSources.csv").Cache()

    let convertToDouble (x:string) = match String.IsNullOrEmpty x with
                                        | false -> let y = x |> float 
                                                   let z = (int32)( 100.0 * y)
                                                   Nullable(z)
                                        | true -> Nullable<int32>()
     
    let convertToNullableInt (x:string) = match String.IsNullOrEmpty x with
                                        | false ->  Nullable(x |> int32)
                                        | true -> Nullable<int32>()

    let getline (line:CsvRow) =
          let record  = null
          try
            let record= {   
                Betfair = line.GetColumn "Betfair" ;
                TennisData = line.GetColumn "TennisData";
            }
            Some(record)
            with
               | Exception -> None


    let dataset =
        [| for line in data.Rows -> 
            getline line |]


     /////////////////////////////////
     /// Fix
    let getPoints source=
      let connectionStringFile = sprintf "Data Source=%s;Version=3;" getFileName       
   
      let connection =  new SQLiteConnection(connectionStringFile) 
      connection.Open() 
      let filteredSql = "select [atp], [location], [name] from Tournament"
      let result = connection.Query<Tournament>(filteredSql,source)
      result |> Seq.where (fun x -> x.name = source)
           
              //let ds row = match row with 
              //                  | Some value -> value
              //                  | None -> Array.empty<DataSource>()
              //ds
              //Array.map 

    let getATP (tournament:string) =
      let fn x =  x.Betfair = (tournament)

      let a =  Array.tryFind fn (dataset |> Array.choose(fun u -> u)) 
      match a with 
                | Some x -> (match (getPoints x.TennisData |> Seq.tryHead) with 
                                                                | Some x-> x.atp
                                                                | None -> 0L) 
                | None -> 0L
            


    //let GetDataSources
    
     //    "insert into Tournament(atp, location, name) " + 
      //  "values (@atp, @location, @name)"

    //let GetAtp 


