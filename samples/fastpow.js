function floor(x: number): number{
    return x - (x % 1)
}

function fastPow(base: number, power: number): number {
    if (power == 0) {
        return 1
    }

    if (power % 2 == 0) {
        const multiplier = fastPow(base, power / 2)
        return multiplier * multiplier
    }
    
    const f = floor(power / 2)
    const multiplier = fastPow(base, f)
    return multiplier * multiplier * base
}

let p = fastPow(13, 3)

print(p as string)
