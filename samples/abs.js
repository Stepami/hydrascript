function abs(x: number): number {
    if (x < 0)
        return -x
    return x
}

let x = -10
let a = abs(x)
print(a as string)