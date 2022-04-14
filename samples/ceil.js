function ceil(x: number): number {
    return x + (1 - x % 1)
}

let c = ceil(3.3)
print(c as string)