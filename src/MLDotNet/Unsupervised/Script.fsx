
open System
open System.IO

let file = @"C:\projects\machinelearningdotnet\src\MLDotNet\Unsupervised\userprofiles-toptags.txt"


let headers, observations =
    let raw =
        file
        |> File.ReadAllLines

    let headers = (raw.[0].Split ',').[1..]
    let observations =
        raw.[1..]
        |> Array.map (fun line -> (line.Split ',').[1..])
        |> Array.map (Array.map float)

    headers, observations

printfn "%16s %8s %8s %8s" "Tag Name" "Avg" "Min" "Max"
headers
    |> Array.iteri (fun i name ->
        let col = observations |> Array.map (fun obs -> obs.[i])
        let avg = col |> Array.average
        let min = col |> Array.min
        let max = col |> Array.max

        printfn "%16s %8.1f %8.1f %8.1f" name avg min max)

#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#load @"..\packages\FSharp.Charting.0.90.14\FSharp.Charting.fsx"
open FSharp.Charting
let labels = ChartTypes.LabelStyle(Interval=0.25)

headers
|> Seq.mapi (fun i name ->
    name,
    observations
    |> Seq.averageBy (fun obs -> obs.[i]))
|> Chart.Bar
|> fun chart -> chart.WithXAxis(LabelStyle=labels)