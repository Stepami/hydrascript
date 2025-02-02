function abs(x: number): number {
    return x < 0 ? -x : x
}

function gcd(a: number, b: number): number {
    let aa = abs(a), bb = abs(b)
    if (bb == 0)
        return aa
    return gcd(bb, aa % bb)
}

function lcm(a: number, b: number): number {
    if (a == 0 || b == 0)
        return 0
    return (abs(a * b)) / (gcd(a, b))
}

>>> lcm(1280, 720)