function fact(n: number): number {
    if (n < 2) {
        return n
    }
    let f = fact(n - 1)
    return n * f
}

function fib(n: number): number {
    if (n < 2) {
        return n
    }
    let f1 = fib(n - 1)
    let f2 = fib(n - 2)
    return f1 + f2
}

let n: number

n = 6
let fa6 = fact(6)
let fa6s = fa6 as string
print("fact(6) = " + fa6s)

n = 9
let fi9 = fib(9)
let fi9s = fi9 as string
print("fib(9) = " + fi9s)