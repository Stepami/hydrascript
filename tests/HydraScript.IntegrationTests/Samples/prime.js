function prime(x: number): boolean {
    if (x % 1 != 0 || x <= 3 || x % 2 == 0) {
        return false
    }

    let div = 3
    while (div * div <= x) {
        if (x % div == 0) {
            return false
        }
        div = div + 2
    }
    
    return true
}

>>> prime(13)