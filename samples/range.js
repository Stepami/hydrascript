type int = number
type ints = number[]

function range(n: int): ints {
    let r = [0,]
    let i = 1
    while (i <= n) {
        r = r ++ [i,]
        i = i + 1
    }
    r::0
    return r
}

>>> range(6)