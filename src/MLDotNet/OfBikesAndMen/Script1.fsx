
#I @"..\packages\"
#r @"FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#load @"FSharp.Charting.0.90.14\FSharp.Charting.fsx"
#r @"MathNet.Numerics.3.11.1\lib\net40\MathNet.Numerics.dll"
#r @"MathNet.Numerics.FSharp.3.11.1\lib\net40\MathNet.Numerics.FSharp.dll"

open FSharp.Charting
open FSharp.Data
open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

let A = vector[1.;2.;3.]
let B = matrix [[1.;2.]
                [3.;4.]
                [5.;6.]]

let C = A * A
let D = A * B
let E = A * B.Column(1)


type Data = CsvProvider<"..\Data\Ch4\day.csv">
let dataset = Data.Load("..\Data\Ch4\day.csv")
let data = dataset.Rows

type Vec = Vector<float>
type Mat = Matrix<float>

let cost (theta: Vec) (Y: Vec) (X: Mat) =
    let ps = Y - (theta * X.Transpose())
    ps * ps |> sqrt

let predict (theta: Vec) (v: Vec) = theta * v

let X = matrix [for obs in data -> [1.; float obs.Instant]]
let Y = vector [for obs in data -> float obs.Cnt]

let theta = vector [6000.; -4.5]
cost theta Y X

let estimate (Y:Vec) (X:Mat) =
    (X.Transpose() * X).Inverse() * X.Transpose() * Y


let seed = 314159
let rng = System.Random(seed)

//fisher yates
let shuffle (arr: 'a[]) =
    let arr = Array.copy arr
    let l = arr.Length
    for i in (l - 1) .. -1 .. 1 do
        let temp = arr.[i]
        let j = rng.Next(0, i+1)
        arr.[i] <- arr.[j]
        arr.[j] <- temp
    arr

let myArray = [| 1..5 |]
myArray |> shuffle