// prime factors
let n = 150

let factor = 2
while (factor * factor <= n) {
    while (n % factor == 0) {
        n = n / factor
        >>>factor
    }

    factor = factor + 1
}

if (n != 1) {
    >>>factor
}