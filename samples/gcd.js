function abs(x: number): number {
    return x < 0 ? -x : x
}

function gcd(a: number, b: number): number {
    let aa = abs(a), bb = abs(b)
    if (bb == 0)
        return aa
    return gcd(bb, aa % bb)
}

let g = gcd(1280, 720)

print(g as string)