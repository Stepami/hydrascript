function ceil(x: number): number {
    return x + (1 - x % 1)
}

>>> ceil(3.3)