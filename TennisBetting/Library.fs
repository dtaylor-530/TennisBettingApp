namespace TennisBetting

open System

module mod1 =
    open datapoints
    open RegressionSmooth

    //Get premium
    let GetPremium c =  let winProbability = (100.0 / (c.MaxW.Value |> double))
                        let loseProbability = (100.0 / (c.MaxL.Value |> double))
                        let premium = (winProbability+ loseProbability + -1.0)
                        if premium <= 0.0 then 0.01 else premium

    // Get Ratio between max and min odds
    let GetRatio c =  let max = Math.Max(c.MaxW.Value ,c.MaxL.Value)
                      let min = (match c.MaxW.Value.Equals( max) with 
                                                                | true -> c.MaxL.Value 
                                                                | false -> c.MaxW.Value) |> double
                      1.0 * (max |> double)/min
              
    // Get Profit
    let GetProfit c =  ( if c.WRank < c.LRank 
                                then c.MaxW.Value 
                                else 0)-100

    // Group and sort by ratio between min/max odds and extract variables
    let group ty = 
                     ty 
                            |> Seq.map ( fun c -> GetRatio c , c)
                            |> Seq.groupBy(fun ( a, b) -> a |>(int))
                            |> Seq.map (fun (ratio,c) -> 
                                                          let records = c |>  Seq.map (fun (a,b) ->  b) |> Seq.toArray
                                                          let avgPremium =records |> Seq.averageBy (fun b ->  GetPremium b)
                                                          let profit = records |> Seq.map (fun b ->  GetProfit b |> double) |> Seq.toArray
                                                          //get average premium for this ratio
                                                          let (mean, var) =  if profit.Length=1    
                                                                             then (profit |> Seq.head,1000.0) 
                                                                             else profit |> Seq.meanAndVariance
                                                          let (mean, var) =  if var = 0.0 then (mean,1000.0) else (mean, var)
                                                          (ratio,mean, var, avgPremium, records.Length))
                            |> Seq.sortBy (fun (ratio,_,_,_,_) -> ratio)
                            |> Seq.toArray
    
    let GetBetAmount (i:int) y = 
                    [for (a,b,c,d,e) in y do
                        let yo = Math.Abs(i - a) + 1 |> double
                        let r= b/(Math.Pow(yo,3.0)* c*Math.Pow(d,3.0)*10000000.0)
                        yield r *(e|> float)]
   
    let GetBetAmount2 (i:int)  (datapoints:DataPoint[]) = 
                    [for datapoint in datapoints do
                        let yo = Math.Abs(i - (datapoint.ratio|> int)) + 1 |> double
                        let r= (datapoint.mean |> float) / (Math.Pow(yo,3.0) *( datapoint.variance |> float) * Math.Pow( datapoint.weight |> float,3.0))
                        yield r *1000000.0* (datapoint.weight|> float)]

    let test (records:record[]) (variables:(int*double*float*float*int)[]) =
              seq { for record in records do
                    let ratio = GetRatio record |> int
                    let profit= GetProfit record |> float
                    let premium = GetPremium record
                    let combinedBetAmt = GetBetAmount ratio variables |> Seq.average
                    yield if combinedBetAmt > 0.0
                          then  (profit * combinedBetAmt /premium) 
                          else 0.0
                    } 
              |> Seq.sum

    let test2 (records:betfair.Odds[]) (variables:DataPoint[]) =
              seq { for record in records do
                    let ratio = betfair.GetRatio record |> int
                    //let profit= GetProfit record |> float
                    let premium = betfair.GetPremium record
                    let combinedBetAmt = GetBetAmount2 ratio variables |> Seq.average
                    yield if combinedBetAmt > 0.0
                          then  (combinedBetAmt /premium) 
                          else 0.0
                    } 
              |> Seq.sum


