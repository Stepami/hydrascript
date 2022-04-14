// prime factors
let n = 150

let factor = 2
while (factor * factor <= n) {
    while (n % factor == 0) {
        n = n / factor
        print(factor as string)
    }

    factor = factor + 1
}

if (n != 1) {
    print(factor as string)
}