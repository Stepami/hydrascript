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
print("fact(" + toString(n) + ") = " + toString(fact(6)) + "\n")

n = 9
print("fib(" + toString(n) + ") = " + toString(fib(9)) + "\n")