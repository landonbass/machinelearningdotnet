#I @"..\packages\"
#r @"FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"

open FSharp.Data

type Data = CsvProvider<"..\Data\Ch4\day.csv">
let dataset = Data.Load("..\Data\Ch4\day.csv")
let data = dataset.Rows

#load @"FSharp.Charting.0.90.14\FSharp.Charting.fsx"
open FSharp.Charting

let all = Chart.Line [for obs in data -> obs.Cnt]

let windowedExample = [1..10] |> Seq.windowed 3 |> Seq.toList

let ma n  (series: float seq) =
    series
    |> Seq.windowed n
    |> Seq.map (fun xs -> xs |> Seq.average)
    |> Seq.toList

let count = 
    data |> Seq.map (fun x -> float x.Cnt)

Chart.Combine [
    Chart.Line count 
    Chart.Line (ma 7 count) 
    Chart.Line (ma 30 count)
    ]

let baseline =
    let avg = data |> Seq.averageBy (fun x -> float x.Cnt)
    data |> Seq.averageBy (fun x -> abs(float x.Cnt - avg))

