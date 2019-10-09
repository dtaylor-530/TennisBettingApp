open System
open TennisBetting
open tournaments
open datapoints
open betfair
open RegressionSmooth

[<EntryPoint>]
let main argv =
    let arg1 = if(argv.Length>0) then  argv.[0] else String.Empty
    if(String.IsNullOrEmpty(arg1)) then
        let data= betfair.GetData

        let op = Seq.toArray datapoints.getAllPoints 

        let r = [for (oo:betfair.Odds) in data do 
                    let atp = datasources.getATP oo.event_title
                    if not(atp=0L) then
                        let points = op |> Array.filter (fun x -> (x.atp |> int64)= atp)
                        let amount = TennisBetting.mod1.GetBetAmount2 (GetRatio oo |> int) points |> Seq.average |> int
                        let premium = betfair.GetPremium oo
                        yield odds.insertOdds oo amount premium
                ]
        Console.WriteLine(r.Length)
    elif(arg1.Equals("profit")) then
        let sum = profit.getProfitSum
        Console.WriteLine(sum)
    elif(arg1.Equals("test")) then
        let t = csv.dataset  |> Seq.choose id |> Seq.filter (fun c-> c.MaxL.HasValue)
        let result = t
                      |> Seq.groupBy (fun k -> k.ATP) 
                      |> Seq.map (fun (k, sequence) ->       
                                            let list = sequence |> Seq.toList
                                            let (cut,cut2)= Seq.gencutTailRecursive (3*(List.length list)/4) list
                                            mod1.test (cut2 |> Seq.toArray) (mod1.group cut))
                      |> Seq.toArray
        let sum = result |> Array.sum |> string
        Console.WriteLine(sum)
     elif(arg1.Equals("tournament")) then
        let t = csv.dataset  |> Seq.choose id |> Seq.distinctBy(fun x-> x.ATP) 
                |> Seq.map(fun x->{ atp =x.ATP |> int64; location= x.Location; name =x.Tournament})
                |> Seq.toArray
        let inserted = insertTournaments t |> string
        Console.WriteLine("Tournaments inserted" + inserted )
    elif(arg1.Equals("data")) then

        let result =  csv.dataset  |> Seq.choose id |> Seq.filter (fun c-> c.MaxL.HasValue)
                      |> Seq.groupBy (fun k -> k.ATP) 
                      |> Seq.map (fun (k, sequence) -> k, mod1.group sequence)  
        let mappedData = result |> Seq.map( fun (k, po) -> [for (k, d:float, r:float, l:float, p) in po do
                                                              let mean= d *100.0 |> int
                                                              let variance = r*100.0 |> int
                                                              let averagePremium =l*1000.0 |> int
                                                              yield { 
                                                              atp = k |> int64;
                                                              ratio = k  |> int64;
                                                              mean= mean |> int64;
                                                              variance = variance |> int64;
                                                              avgPremium = averagePremium |> int64;
                                                              weight = p |> int64 }])
                         |> Seq.collect(fun x -> x)
        let points = datapoints.insertPoints(mappedData) |> string
        Console.WriteLine("Points inserted" + points )
    let key = Console.ReadKey()
    0 // return an integer exit code
