function fact(n: number): number {
    if (n < 2) {
        return n
    }
    return n * fact(n - 1)
}

function fib(n: number): number {
    if (n < 2) {
        return n
    }
    return fib(n - 1) + fib(n - 2)
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