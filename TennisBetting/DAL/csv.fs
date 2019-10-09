namespace TennisBetting

open System

module csv =
    open FSharp.Data

    let path = match System.IO.File.Exists(@"..\..\..\..\TennisBettingApp\Data\Data.csv") with
                    | true -> (new System.IO.FileInfo(@"..\..\..\..\TennisBettingApp\Data\Data.csv")).FullName
                    | false -> (new System.IO.FileInfo(@"..\..\..\..\..\Data\Data.csv")).FullName

    let data = CsvFile.Load(path).Cache()

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
                ATP = line.GetColumn "ATP" |>int32;
                Location = line.GetColumn "Location";
                Tournament = line.GetColumn "Tournament";
                Date = line.GetColumn "Date";
                Series = line.GetColumn "Series";
                Court = line.GetColumn "Court";
                Surface = line.GetColumn "Surface";
                Round= line.GetColumn "Round";
                Bestof = line.GetColumn "Best of"|> int32;
                Winner = line.GetColumn "Winner";
                Loser = line.GetColumn "Loser";
                WRank = line.GetColumn "WRank" |> int32;
                LRank = line.GetColumn "LRank"|> int32;
                W1 = line.GetColumn "W1"|> int32;
                L1 = line.GetColumn "L1"|> int32;
                W2 = line.GetColumn "W2"|> int32;
                L2 = line.GetColumn "L2"|> int32;
                W3 = convertToNullableInt (line.GetColumn "W3");
                L3 = convertToNullableInt(line.GetColumn "L3");
                W4 = convertToNullableInt(line.GetColumn "W4");
                L4 = convertToNullableInt(line.GetColumn "L4");
                W5 = convertToNullableInt(line.GetColumn "W5");
                L5 = convertToNullableInt( line.GetColumn "L5");
                Wsets = line.GetColumn "Wsets"|> int32;
                Lsets = line.GetColumn "Lsets"|> int32;
                Comment = line.GetColumn "Comment";
                CBW = convertToDouble( line.GetColumn "CBW");
                CBL = convertToDouble( line.GetColumn "CBL");
                GBW = convertToDouble( line.GetColumn "GBW");
                GBL = convertToDouble( line.GetColumn "GBL");
                IWW = convertToDouble( line.GetColumn "IWW");
                IWL = convertToDouble( line.GetColumn "IWL");
                SBW = convertToDouble( line.GetColumn "SBW");
                SBL = convertToDouble( line.GetColumn "SBL");
                B365W = convertToDouble( line.GetColumn "B365W");
                B365L = convertToDouble( line.GetColumn "B365L");
                BAndWW = convertToDouble( line.GetColumn "B&WW");
                BAndWL = convertToDouble( line.GetColumn "B&WL");
                EXW = convertToDouble( line.GetColumn "EXW");
                EXL = convertToDouble( line.GetColumn "EXL");
                PSW = convertToDouble( line.GetColumn "PSW");
                PSL = convertToDouble( line.GetColumn "PSL");
                WPts = convertToDouble( line.GetColumn "WPts");
                LPts = convertToDouble( line.GetColumn "LPts");
                UBW = convertToDouble( line.GetColumn "UBW");
                UBL = convertToDouble( line.GetColumn "UBL");
                LBW = convertToDouble( line.GetColumn "LBW");
                LBL = convertToDouble( line.GetColumn "LBL");
                SJW = convertToDouble( line.GetColumn "SJW");
                SJL = convertToDouble( line.GetColumn "SJL");
                MaxW = convertToDouble( line.GetColumn "MaxW");
                MaxL = convertToDouble( line.GetColumn "MaxL");
                AvgW = convertToDouble( line.GetColumn "AvgW");
                AvgL = convertToDouble( line.GetColumn "AvgL");
            }
            Some(record)
            with
               | Exception -> None


    let dataset =
        [| for line in data.Rows -> 
            getline line |]